import socket
import json
import time
from typing import List

ROBERT_TCP_SERVER_IP = "127.0.0.1"
ROBERT_TCP_SERVER_PORT = 3003

class RobertController:
    def __init__(self, bot_id: int = 0):
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.socket.settimeout(5.0)
        self.socket.connect((ROBERT_TCP_SERVER_IP, ROBERT_TCP_SERVER_PORT))
        self.bot_id = bot_id
    
    def __del__(self):
        print('deleting')
        self.socket.close()
    
    def send_command(self, cmd_id: str, args: dict = {}, log_errors: bool = True):
        raw_cmd = json.dumps({
            "cmd_id":cmd_id,
            "bot_id": self.bot_id,
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

    def rotate(self, angle: float, relative: bool = False):
        return self.send_command('rotate', args={
            "angle": angle,
            "relative": relative
        })

    def mine(self, direction: str = "center"):
        return self.send_command('mine', args={
            "direction": direction
        })
    
    def halt(self, clear_command_buffer=True):
        return self.send_command('halt', args={
            'clear_command_buffer': clear_command_buffer
        })

    def get_position(self):
        response = self.send_command('position_query')
        return json.loads(response)['position']

    def get_rotation(self):
        response = self.send_command('position_query')
        return json.loads(response)['rotation']

    def check_sensors(self):
        response = self.send_command('sensor_query')
        return json.loads(response)['readings']

    def get_item_count(self, item_id: int):
        response = self.send_command('inventory_query', args={
            "cmd_id": "inventory_query",
            "item_id": item_id
        })
        return response['message'] if response['error'] else response['amount']

    def is_busy(self):
        response = self.send_command(cmd_id='busy_query')
        if response:
            return response['busy']
        else:
            return False

    def wait_until_free(self):
        while(self.is_busy()):
            time.sleep(0.04)

