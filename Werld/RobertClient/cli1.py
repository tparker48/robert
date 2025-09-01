from robapi import RobertController
import time

robert = RobertController()
#

print(robert.get_floor())

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