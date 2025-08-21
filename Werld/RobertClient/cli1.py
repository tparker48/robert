import robapi

robert = robapi.RobertController()

while True:
    robert.move([1,0])
    robert.wait_until_free()
    robert.move([-1,0])