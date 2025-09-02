from robapi import RobertController
import time

robert = RobertController()

def scan():
    scan_map = robert.scan_mine(pretty_print=True)
    print('\n'*10)
    for line in scan_map:
        print(line)

robert.printer_queue_job('Carbon Chunk', 10)
while(robert.get_printer_status()['busy']):
    time.sleep(0.1)
robert.printer_retrieve({}, False, True)

time.sleep(1)
print(robert.get_printer_status())
print(robert.get_inventory())

exit(0)
robert.teleport_to_mine()

robert.halt()
robert.wait_until_free()

for i in range(5):
    robert.mine(direction="left")
    robert.wait_until_free()

    robert.mine(direction="right")
    robert.wait_until_free()
    
    robert.mine(direction="center")
    robert.wait_until_free()
    
    robert.move([1,0])
    robert.wait_until_free()

    scan()
    print(robert.get_inventory())


#robert.send_command(cmd_id='mine_return')
#time.sleep(3)
#beacons = robert.send_command(cmd_id='beacon_query', args={'relative':False})['beacons']
#for name in beacons:
#    print(name, beacons[name])

#robert.halt()
#robert.move(beacons['Room 1'])
#scan = robert.scan_mine()
#for line in scan:
#    line = ' '.join([{-1: 'R', 0:'.', 1:'#', 2:'!', 3:'T', 4:'_'}[c] for c in line])
#    print(line)