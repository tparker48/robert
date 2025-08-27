from robapi import RobertController

robert = RobertController()

robert.printer_fill(items={"Lettuce Seeds": 5})

print(robert.get_printer_status())
