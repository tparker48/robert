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

    

    void Sell(ref ItemContainer sellerInventory, Item itemToSell, uint quantity)
    {
        ItemGroup itemsToSell = new ItemGroup(){{itemToSell, quantity}};
        if (sellerInventory.RemoveItems(itemsToSell))
        {
            Ship.Instance.bits += quantity * itemToSell.value;
        }
    }
}
