import socket
import json
import time
from typing import List

client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

server_ip = "127.0.0.1"
server_port = 30120

client_socket.connect((server_ip, server_port))


def move(position: List[float], relative: bool = False):
    raw_cmd = json.dumps({
        "cmd_id": "move",
        "position": position,
        "relative": relative
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    print(f"Recieved from server: {recieved_data.decode('utf-8')}")

def rotate(angle: float, relative: bool = False):
    raw_cmd = json.dumps({
        "cmd_id": "rotate",
        "angle": angle,
        "relative": relative
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    print(f"Recieved from server: {recieved_data.decode('utf-8')}")

def halt(clear_command_buffer=True):
    raw_cmd = json.dumps({
        "cmd_id": "halt",
        "clear_command_buffer": clear_command_buffer
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(10234)
    print(f"Recieved from server: {recieved_data.decode('utf-8')}")

def get_position():
    raw_cmd = json.dumps({
        "cmd_id": "position_query"
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['position']

def get_rotation():
    raw_cmd = json.dumps({
        "cmd_id": "position_query"
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['rotation']

def rob_is_busy():
    raw_cmd = json.dumps({
        "cmd_id": "busy_query"
    })
    client_socket.send(raw_cmd.encode('utf-8'))
    recieved_data = client_socket.recv(1024)
    response = recieved_data.decode('utf-8')
    return json.loads(response)['busy']

def wait_for_rob():
    while(rob_is_busy()):
        time.sleep(0.04)

#----
# do something
#----

client_socket.close()