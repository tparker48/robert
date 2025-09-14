import socket
import json
import time
from typing import List


ROBERT_TCP_SERVER_IP = "127.0.0.1"
ROBERT_TCP_SERVER_PORT = 3000


class ShipContoller:
    def __init__(self):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.settimeout(5.0)
        self.socket.connect((ROBERT_TCP_SERVER_IP, ROBERT_TCP_SERVER_PORT))

    def __del__(self):
        print('Closing ShipController TCP Connection')
        self.socket.close()

    def send_command(self, cmd_id: str, args: dict = {}, log_errors: bool = True):
        raw_cmd = json.dumps({
            "cmd_id":cmd_id,
            "ship_command": True,
            "is_query": False,
            **args
        })
        self.socket.send(raw_cmd.encode('utf-8'))
        response_data = json.loads(self.socket.recv(1024))

        if log_errors and response_data['error']:
            print(response_data['message'])

        return response_data

class RobertController:
    def __init__(self, bot_id: int = 0):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.settimeout(5.0)
        self.socket.connect((ROBERT_TCP_SERVER_IP, ROBERT_TCP_SERVER_PORT))
        self.bot_id = bot_id
    
    def __del__(self):
        print('Closing RobertController TCP Connection')
        self.socket.close()
    
    def send_command(self, cmd_id: str, args: dict = {}, log_errors: bool = True):
        raw_cmd = json.dumps({
            "cmd_id":cmd_id,
            "bot_id": self.bot_id,
            "ship_command": False,
            **args
        })
        self.socket.send(raw_cmd.encode('utf-8'))
        response_data = json.loads(self.socket.recv(1024))

        if log_errors and response_data['error']:
            print(response_data['message'])

        return response_data

    # Commands
    def move(self, position: List[float], relative: bool = True):
        return self.send_command(cmd_id='move', args={
            "position": position,
            "relative": relative
        })

    def rotate(self, angle: float, relative: bool = True):
        return self.send_command('rotate', args={
            "angle": angle,
            "relative": relative
        })

    def mine(self, direction: str = "center"):
        return self.send_command('mine', args={
            "direction": direction
        })
    
    def teleport_to_mine(self):
        return self.send_command('teleport')
    
    def return_from_mine(self):
        return self.send_command('teleport_return')
    
    def plant_seed(self, seed_item: str):
        return self.send_command('plant', args={
            "seed_item": seed_item
        })

    def harvest_plant(self):
        return self.send_command('harvest')

    def printer_queue_job(self, item_to_print: str, quantity: int):
        return self.send_command('printer_queue_job', args={
            'item_to_print': item_to_print,
            'quantity': quantity
        })
    
    def printer_fill(self, items: dict):
        return self.send_command('printer_fill', args={
            'items_to_add': items
        })

    def printer_stop(self):
        return self.send_command('printer_stop')
    
    def printer_retrieve(self, items_to_collect: dict, from_input: bool = False, collect_all: bool = False):
        return self.send_command('printer_retrieve', args={
            'from_input': from_input,
            'collect_all': collect_all,
            'items_to_collect': items_to_collect
        })
    
    def halt(self, clear_command_buffer=True):
        return self.send_command('halt', args={
            'clear_command_buffer': clear_command_buffer
        })
    
    def scan_mine(self, pretty_print : bool = False):
        response = self.send_command(cmd_id='scan_mine')
        filepath = response['scan_output_path']
        pretty_charmap = {
            -1: '🆁 ', 
            0:'  ', # AIR
            1:'██',  # WALL
            2:'░░',  # BORDER
            3:'△c', # COPPER
            4:'▲i', # IRON
            5:'◆g', # GOLD
            6:'✪d', # DIAMOND
        }
        with open(filepath, 'r') as file:
            lines = file.readlines()
        lines = [line.strip() for line in lines]
        for i in range(len(lines)):
            chars = lines[i].split(' ')
            lines[i] = [int(c) for c in chars]
            if (pretty_print):
                lines[i] = ''.join([pretty_charmap[c] for c in lines[i]])
        return lines
    
    def wait_until_free(self):
        while(self.is_busy()):
            time.sleep(0.04)

    # Queries
    def get_position(self):
        response = self.send_command('get_position')
        return response['position']

    def get_rotation(self):
        response = self.send_command('get_position')
        return response['rotation']

    def get_floor(self):
        response = self.send_command('get_floor')
        return response['floor']
    
    def check_sensors(self):
        response = self.send_command('check_sensors')
        return response['readings']

    def scan_beacons(self, relative: bool = True):
        response = self.send_command('scan_beacons', args = {
            'relative': relative
        })
        return response['beacons']

    def get_item_count(self, item_name: str):
        response = self.send_command('get_item_count', args={
            "item_name": item_name
        })
        return response['message'] if response['error'] else response['amount']

    def get_full_inventory(self):
        response = self.send_command('get_full_inventory')
        return response['message'] if response['error'] else response['inventory']

    def is_busy(self):
        response = self.send_command(cmd_id='check_busy')
        return response['busy']

    def check_growbox_status(self):
        return self.send_command('check_growbox_status')

    def check_printer_status(self):
        response = self.send_command('check_printer_status')
        return response

    def deposit_to_storage(self, items: dict[str, int]):
        return self.send_command('deposit_to_storage', args={
            'items_to_deposit': items
        })
    
    def withdraw_from_storage(self, items: dict[str, int]):
        return self.send_command('withdraw_from_storage', args={
            'items_to_withdraw': items
        })

