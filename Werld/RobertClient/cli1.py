from robapi import RobertController
import time

robert = RobertController()

def scan():
    scan_map = robert.scan_mine(pretty_print=True)
    print('\n'*10)
    for line in scan_map:
        print(line)

robert.teleport_to_mine()

for i in range(10):
    scan()
    robert.mine(direction="left")
    robert.wait_until_free()

    scan()
    robert.mine(direction="right")
    robert.wait_until_free()

    scan()
    robert.mine(direction="center")
    robert.wait_until_free()
    
    robert.move([1,0])
    robert.wait_until_free()


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