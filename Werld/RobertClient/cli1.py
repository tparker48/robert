from robapi import RobertController, ShipContoller

robert = RobertController(0)
print(robert.get_item_count(item_name='Lettuce'))