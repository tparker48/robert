using UnityEngine;

public class Teleporter : RobertTimedTaskExecutor<bool>
{
    public void TeleportRobertToMine(Robert robert)
    {
        if (!IsBusy())
        {
            Cave.Instance.AddBot(robert);
        }   
    }

    public void Refresh()
    {
        if (!IsBusy() && Cave.Instance.roberts.Count == 0)
        {
            StartTimedTask(true, 25.0f);
        }
    }

    protected override void ExecuteOnTaskEnd(bool _)
    {
        if (Cave.Instance.roberts.Count == 0)
        {
            StartTimedTask(true, 25.0f);
        }
    }

}
