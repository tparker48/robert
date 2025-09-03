from robapi import RobertController
import time
import os

robert = RobertController()

robert.plant_seed('Lettuce Seeds')
print('planting seeds...')
robert.wait_until_free()

print('waiting for plant to grow...')
while(not robert.check_growbox_status()['ready_to_harvest']):
    time.sleep(0.1)

robert.harvest_plant()
print('harvesting...')
robert.wait_until_free()

print('Inventory:')
print(robert.get_full_inventory())