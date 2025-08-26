import robapi

robert = robapi.RobertController()

robert.printer_fill(items={'LettuceSeeds': 5})
print(robert.get_printer_status())