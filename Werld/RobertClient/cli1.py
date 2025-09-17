import time
from robapi import RobertController, ShipContoller

robert = RobertController(0)
robert.send_command("build_room_equipment", {"equipment": "GrowBox"})
time.sleep(1)
print(robert.plant_seed('Tomato Seeds'))