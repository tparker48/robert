from robapi import RobertController

robert = RobertController()

robert.halt()
#robert.rotate(-30, relative=True)
robert.move([3,0], relative=True)