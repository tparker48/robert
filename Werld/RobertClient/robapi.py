import socket
import json
import time
from typing import List


ROBERT_TCP_SERVER_IP = "127.0.0.1"
ROBERT_TCP_SERVER_PORT = 3001


class RobertController:
    def __init__(self, bot_id: int = 0):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.settimeout(5.0)
        self.socket.connect((ROBERT_TCP_SERVER_IP, ROBERT_TCP_SERVER_PORT))
        self.bot_id = bot_id
    
    def __del__(self):
        print('Closing TCP Connection')
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
        return self.send_command('mine_teleport')
    
    def return_from_mine(self):
        return self.send_command('mine_return')
    
    def plant(self, seed_item: str):
        return self.send_command('plant', args={
            "seed_item": seed_item
        })
    
    def printer_fill(self, items: dict):
        return self.send_command('printer_fill', args={
            'items_to_add': items
        })
    
    def printer_queue_job(self, item_to_print: str, quantity: int):
        return self.send_command('printer_queue_job', args={
            'item_to_print': item_to_print,
            'quantity': quantity
        })

    def printer_stop(self):
        return self.send_command('printer_stop')

    def printer_retrieve(self, items_to_collect: dict, from_input: bool = False, collect_all: bool = False):
        return self.send_command('printer_retrieve', args={
            'from_input': from_input,
            'collect_all': collect_all,
            'items_to_collect': items_to_collect
        })

    def is_printer_in_range(self):
        response = self.send_command('printer_status_query')
        return not response['error']
    
    def get_printer_status(self):
        response = self.send_command('printer_status_query')
        return response

    def deposit_to_storage(self, items: dict[str, int]):
        return self.send_command('deposit_to_storage', args={
            'items_to_deposit': items
        })
    
    def withdraw_from_storage(self, items: dict[str, int]):
        return self.send_command('withdraw_from_storage', args={
            'items_to_withdraw': items
        })

    def halt(self, clear_command_buffer=True):
        return self.send_command('halt', args={
            'clear_command_buffer': clear_command_buffer
        })

    def get_position(self):
        response = self.send_command('position_query')
        return response['position']

    def get_rotation(self):
        response = self.send_command('position_query')
        return response['rotation']

    def get_floor(self):
        response = self.send_command('ship_floor_query')
        return response['floor']

    def check_sensors(self):
        response = self.send_command('sensor_query')
        return response['readings']

    def scan_beacons(self, relative: bool = True):
        response = self.send_command('beacon_query', args = {
            'relative': relative
        })
        return response['beacons']

    def get_item_count(self, item_name: str):
        response = self.send_command('item_query', args={
            "item_name": item_name
        })
        return response['message'] if response['error'] else response['amount']

    def get_inventory(self):
        response = self.send_command('inventory_query')
        return response['message'] if response['error'] else response['inventory']

    def is_busy(self):
        response = self.send_command(cmd_id='busy_query')
        return response['busy']
        
    def scan_mine(self, pretty_print : bool = False):
        response = self.send_command(cmd_id='mine_scan_query')
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

