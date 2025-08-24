import robapi

robert = robapi.RobertController()

print(robert.get_item_count(4))

beacons = robert.scan_beacons()
for beacon_name, relative_position in beacons:
    robert.move(relative_position, relative=True)

robert.plant(4)

robert.wait_until_free()

print(robert.get_item_count(4))