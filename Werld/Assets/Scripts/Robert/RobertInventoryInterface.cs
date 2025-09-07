using UnityEngine;

public class RobertInventoryInterface : MonoBehaviour
{
    private RobertSensorInterface sensors;
    private ItemContainer inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensorInterface>();
    }

    public void HandleDepositCommand(string rawCmd)
    {
        DepositToStorage cmd = CommandParser.Parse<DepositToStorage>(rawCmd);
        Storage storage = GetStorageInRange();
        if (storage == null) return;
        storage.HandleDepositToStorage(cmd, ref inventory);
    }

    public void HandleWithdrawCommand(string rawCmd)
    {
        WithdrawFromStorage cmd = CommandParser.Parse<WithdrawFromStorage>(rawCmd);
        Storage storage = GetStorageInRange();
        if (storage == null) return;
        storage.HandleWithdrawFromStorage(cmd, ref inventory);
    }

    private Storage GetStorageInRange()
    {
        Storage printer = null;
        if (!sensors.GetObjectOfType(ref printer))
        {
            Debug.Log("No Storage Container In Range!");
        }
        return printer;
    }

    public Response HandleGetItemCount(string rawCmd)
    {
        GetItemCount query = CommandParser.Parse<GetItemCount>(rawCmd);
        if (Items.ItemExists(query.item_name))
        {
            Item item = Items.Lookup(query.item_name);
            GetItemCountResponse item_response = new GetItemCountResponse();
            item_response.amount = inventory.GetItemCount(item);
            return item_response;
        }
        else
        {
            return Response.ErrorResponse($"Item '{query.item_name}' does not exist!");
        }
    }

    public Response HandleGetFullInventory(string _)
    {
        GetFullInventoryResponse inv_response = new GetFullInventoryResponse();
        inv_response.inventory = inventory.GetInventory().ToStringKeys();
        return inv_response;
    }

    public Response HandleCheckSellValue(string rawCmd)
    {
        CheckSellValue query = CommandParser.Parse<CheckSellValue>(rawCmd);
        Tradepost tradepost = null;
        if (sensors.GetObjectOfType(ref tradepost))
        {
            return tradepost.HandleCheckSellValue(query);
        }
        else
        {
            return Response.ErrorResponse("Not at a tradepost!");
        }
    }

    public void HandleSellItem(string rawCmd)
    {
        SellItem sellCmd = CommandParser.Parse<SellItem>(rawCmd);
        Tradepost tradepost = null;
        if (sensors.GetObjectOfType(ref tradepost))
        {
            tradepost.HandleSellItem(ref inventory, sellCmd);
        }
    }
}
