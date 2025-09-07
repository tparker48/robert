using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tradepost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Buy(ref ItemContainer buyerInventory, Item itemToBuy, uint quantity)
    {
        ItemGroup itemsToBuy = new ItemGroup() { { itemToBuy, quantity } };
    }

    public void Sell(ref ItemContainer sellerInventory, Item itemToSell, uint quantity)
    {
        ItemGroup itemsToSell = new ItemGroup() { { itemToSell, quantity } };
        if (sellerInventory.RemoveItems(itemsToSell))
        {
            Ship.Instance.bits += quantity * itemToSell.value;
        }
    }

    public void HandleSellItem(ref ItemContainer other, SellItem cmd)
    {
        Sell(ref other, Items.Lookup(cmd.item), cmd.quantity);
    }

    public Response HandleCheckSellValue(CheckSellValue query)
    {
        CheckSellValueResponse response = new CheckSellValueResponse();
        Item item = Items.Lookup(query.item);
        if (item != null)
        {
            response.valid_item = true;
            response.sell_value = (int)item.value;
        }
        else
        {
            response.valid_item = false;
            response.sell_value = -1;
        }
        return response;
    }
}
