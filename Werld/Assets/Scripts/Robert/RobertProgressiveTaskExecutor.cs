public abstract class RobertProgressiveTaskExecutor<T> : RobertTaskExecutor
{
    private T pendingTask;

    protected void StartProgressiveTask(T task)
    {
        if (!IsBusy())
        {
            pendingTask = task;
            busy = true;
        }
    }

    protected void UpdateTask()
    {
        if (busy)
        {
            ExecuteWhileTaskRunning(pendingTask);

            if (CheckTaskComplete(pendingTask))
            {
                busy = false;
                ExecuteOnTaskEnd(pendingTask);
            }
        }
    }

    override public float PercentComplete()
    {
        return CalculateCompletionPercentage(pendingTask);
    }
    
    protected virtual void ExecuteOnTaskEnd(T task) { }
    protected virtual void ExecuteWhileTaskRunning(T task) { }
    protected abstract bool CheckTaskComplete(T task);
    protected abstract float CalculateCompletionPercentage(T task);
}
