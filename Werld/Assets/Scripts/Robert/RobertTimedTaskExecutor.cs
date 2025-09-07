// Execute tasks that take time. 
// Report "busy" status while a task is running.

public abstract class RobertTimedTaskExecutor<T> : RobertTaskExecutor
{
    private float taskTimer = 0.0f;
    private float taskTimerFull = 0.0f;
    private T pendingTask;

    protected void StartTimedTask(T task, float taskTime)
    {
        if (!IsBusy())
        {
            pendingTask = task;
            busy = true;
            taskTimer = taskTime;
            taskTimerFull = taskTime;

        }
    }

    protected void UpdateTask(float elapsedTime)
    {
        if (busy)
        {
            ExecuteWhileTaskRunning(pendingTask);

            taskTimer -= elapsedTime;
            if (taskTimer <= 0.0f)
            {
                ExecuteOnTaskEnd(pendingTask);
                busy = false;
            }
        }
    }

    protected virtual void ExecuteOnTaskEnd(T task) { }
    protected virtual void ExecuteWhileTaskRunning(T task) { }

    override public float PercentComplete()
    {
        if (!busy)
        {
            return 0.0f;
        }
        else
        {
            return (taskTimerFull-taskTimer) / taskTimerFull;
        }
    }
}
