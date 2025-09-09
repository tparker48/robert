from robapi import RobertController
import time
import os

robert = RobertController()

#robert.send_command('build_room_equipment', {'equipment': '3D Printer'}, True)
print(robert.check_printer_status())