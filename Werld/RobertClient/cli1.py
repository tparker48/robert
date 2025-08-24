import robapi

robert = robapi.RobertController()

beacons = robert.scan_beacons()
for beacon_name, relative_position in beacons:
    print('Moving to', beacon_name)
    print(relative_position)
    robert.move(relative_position, relative=True)