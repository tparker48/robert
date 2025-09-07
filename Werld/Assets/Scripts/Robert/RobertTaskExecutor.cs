using UnityEngine;

// Execute tasks that take time. 
// Report "busy" status while a task is running.
public abstract class RobertTaskExecutor : MonoBehaviour
{
    public string busyText = "";

    protected bool busy = false;

    public void Halt()
    {
        busy = false;
    }

    public bool IsBusy()
    {
        return busy;
    }

    public string GetBusyText()
    {
        return busyText;
    }

    public abstract float PercentComplete();
}
