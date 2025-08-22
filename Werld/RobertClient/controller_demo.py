import tkinter as tk
from robapi import RobertController

# Robert Controller
rob = RobertController(0)


# Window
WIDTH = 200
HEIGHT = 250
root = tk.Tk()
root.title("Robert Controller Demo")
root.geometry(f"{WIDTH}x{HEIGHT}")


# Bot ID Set/Get
bot_id_label = tk.Label(root, text="Bot Id")
bot_id_input = tk.Entry(root, width=5)
bot_id_label.pack()
bot_id_input.pack()

def get_bot_id():
    bot_id_entry = bot_id_input.get()
    return int(bot_id_entry) if bot_id_entry.isdigit() else 0


# Movement
def move(move_coords):
    rob.bot_id = get_bot_id()
    rob.move(move_coords, relative=True)

def rotate(direction):
    rob.bot_id = get_bot_id()
    rob.rotate(direction, relative=True)

def halt():
    rob.bot_id = get_bot_id()
    rob.halt()

movement_label = tk.Label(root, text="Movement")
forward_button = tk.Button(root, text='  ^  ', command=lambda: move([1,0]))
back_button = tk.Button(root, text='  v  ', command=lambda: move([-1,0]))
left_button = tk.Button(root, text='  <  ', command=lambda: move([0,-1]))
right_button = tk.Button(root, text='  >  ', command=lambda: move([0,1]))
rotate_left_button = tk.Button(root, text='  ⤸  ', command=lambda: rotate(-15))
rotate_right_button = tk.Button(root, text='  ⤹  ', command=lambda: rotate(15))
halt_button = tk.Button(root, text='halt', command=halt)

x = WIDTH*0.5
y = HEIGHT*0.5
margin = HEIGHT*0.12

movement_label.place(x=x-margin, y=y-2*margin)
forward_button.place(x=x, y=y - margin)
back_button.place(x=x, y=y + margin)
left_button.place(x=x - margin, y=y)
right_button.place(x=x + margin, y=y)
rotate_left_button.place(x=x - margin, y=y-margin)
rotate_right_button.place(x=x + margin, y=y-margin)
halt_button.place(x=x, y=y)

# Start the Tkinter event loop
root.mainloop()
