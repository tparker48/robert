import robapi

robert = robapi.RobertController()

scan_results = robert.scan_mine()
for line in scan_results:
    print([int(ch) for ch in line])