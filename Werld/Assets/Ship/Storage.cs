using UnityEngine;

public class Storage : MonoBehaviour
{
    public ItemContainer container;
    private uint level = 1;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool HandleDepositToStorageCommand(DepositToStorageCommand cmd, ref ItemContainer other)
    {
        return container.TakeItemsFrom(ref other, new ItemGroup(cmd.items_to_deposit));
        
    }

    public bool HandleWithdrawFromStorageCommand(WithdrawFromStorageCommand cmd, ref ItemContainer other)
    {
        return other.TakeItemsFrom(ref container, new ItemGroup(cmd.items_to_withdraw));
    }
}
