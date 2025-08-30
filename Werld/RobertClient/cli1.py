from robapi import RobertController

robert = RobertController()

robert.send_command(cmd_id='teleport_to_mine')

scan = robert.scan_mine()
for line in scan:
    line = ' '.join([{-1: 'R', 0:'.', 1:'#', 2:'!', 3:'_'}[c] for c in line])
    print(line)