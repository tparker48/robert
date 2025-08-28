from robapi import RobertController

robert = RobertController()

beacons = robert.scan_beacons(relative=False)
print(beacons)

pos = beacons['3D Printer']
robert.move(pos, relative=False)