using System.Collections.Generic;
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

    public void HandleDepositRobertCommand(string rawCmd)
    {
        DepositToRobert cmd = CommandParser.Parse<DepositToRobert>(rawCmd);
        Robert other = null;
        if (RobertInRange(cmd.bot_id, ref other))
        {
            ItemGroup itemsToDeposit = new ItemGroup(cmd.items_to_deposit);
            if (inventory.RemoveItems(itemsToDeposit))
            {
                if (other.inventory.AddItems(itemsToDeposit))
                {
                    return;
                }
                else
                {
                    inventory.AddItems(itemsToDeposit);
                }
            }
        }
    }

    public void HandleWithdrawRobertCommand(string rawCmd)
    {
        DepositToRobert cmd = CommandParser.Parse<DepositToRobert>(rawCmd);
        Robert other = null;
        if (RobertInRange(cmd.bot_id, ref other))
        {
            ItemGroup itemsToDeposit = new ItemGroup(cmd.items_to_deposit);
            if (other.inventory.RemoveItems(itemsToDeposit))
            {
                if (inventory.AddItems(itemsToDeposit))
                {
                    return;
                }
                else
                {
                    other.inventory.AddItems(itemsToDeposit);
                }
            }
        }
    }

    private Storage GetStorageInRange()
    {
        Storage storage = null;
        if (!sensors.GetObjectOfType(ref storage))
        {
            Debug.Log("No Storage Container In Range!");
        }
        return storage;
    }

    private bool RobertInRange(int bot_id, ref Robert found)
    {
        foreach (GameObject obj in sensors.GetSensedObjects())
        {
            Robert other = obj.GetComponent<Robert>();
            if (other != null && other.id == bot_id)
            {
                found = other;
                return true;
            }
        }
        found = null;
        return false;
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
