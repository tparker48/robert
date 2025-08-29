using UnityEngine;

public class RobertStorageInterface : MonoBehaviour
{
    private RobertSensors sensors;
    private ItemContainer inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensors>();
    }

    public void HandleDepositCommand(DepositToStorageCommand cmd)
    {
        Storage storage = GetStorageInRange();
        if (storage == null) return;
        storage.HandleDepositToStorageCommand(cmd, ref inventory);
    }

    public void HandleWithdrawCommand(WithdrawFromStorageCommand cmd)
    {
        Storage storage = GetStorageInRange();
        if (storage == null) return;
        storage.HandleWithdrawFromStorageCommand(cmd, ref inventory);
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
}
