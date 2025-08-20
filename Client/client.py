import socket
import json
import time
from typing import List

client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_ip = "127.0.0.1"
server_port = 30120

client_socket.connect((server_ip, server_port))

def move(bot_id: int, position: List[float], relative: bool = False):
    raw_cmd = json.dumps({
        "cmd_id": "move",
        "bot_id": bot_id,
        "position": position,
        "relative": relative
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    print(f"Recieved from server: {recieved_data.decode('utf-8')}")

def rotate(bot_id: int, angle: float, relative: bool = False):
    raw_cmd = json.dumps({
        "cmd_id": "rotate",
        "bot_id": bot_id,
        "angle": angle,
        "relative": relative
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    print(f"Recieved from server: {recieved_data.decode('utf-8')}")

def halt(bot_id: int, clear_command_buffer=True):
    raw_cmd = json.dumps({
        "cmd_id": "halt",
        "bot_id": bot_id,
        "clear_command_buffer": clear_command_buffer
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(10234)
    print(f"Recieved from server: {recieved_data.decode('utf-8')}")

def get_position(bot_id: int):
    raw_cmd = json.dumps({
        "cmd_id": "position_query",
        "bot_id": bot_id,
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['position']

def get_rotation(bot_id: int):
    raw_cmd = json.dumps({
        "cmd_id": "position_query",
        "bot_id": bot_id,
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['rotation']

def check_sensors(bot_id: int):
    raw_cmd = json.dumps({
        "cmd_id": "sensor_query",
        "bot_id": bot_id,
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['readings']

def rob_is_busy(bot_id: int):
    raw_cmd = json.dumps({
        "cmd_id": "busy_query",
        "bot_id": bot_id,
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['busy']

def wait_for_rob(bot_id: int):
    while(rob_is_busy(bot_id)):
        time.sleep(0.04)

#----
id = 0
#rotate(id, -350, relative=True)
move(id, [1,0], relative=True)
time.sleep(1)
halt(id)

#----

client_socket.close()