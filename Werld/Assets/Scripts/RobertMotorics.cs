using UnityEngine;

public class MoveTask
{
    public MoveCommand moveCmd = null;
    public Vector3 targetPosition;

    public RotateCommand rotateCommand = null;
    public float targetRotation;
    public float rotationDir;

    public float initialDiff = 0.0f;
}

public class RobertMotorics : RobertProgressiveTaskExecutor<MoveTask>
{
    float speed = 3.5f;
    float posDeadzoneMult = 0.4f;
    float positionDeadzone = 0.3f;
    float positionTolerance = 0.01f;

    float rotSpeed = 100.0f;
    float rotDir = 1.0f;
    float rotDeadzoneMult = 0.08f;
    float rotDeadZone = 1.25f;
    float rotationTolerance = 0.30f;


    // Update is called once per frame
    void Update()
    {
        UpdateTask();
    }

    public void HandleMoveCommand(MoveCommand cmd)
    {
        Vector3 newTarget = new Vector3(cmd.position[0], 0.0f, cmd.position[1]);
        if (cmd.relative)
        {
            newTarget = transform.position + transform.forward * cmd.position[0] + transform.right * cmd.position[1];
        }
        newTarget[1] = transform.position.y;

        MoveTask moveTask = new MoveTask();
        moveTask.moveCmd = cmd;
        moveTask.targetPosition = newTarget;
        moveTask.initialDiff = GetPositionDiff(transform.position, newTarget).magnitude;
        StartProgressiveTask(moveTask);
    }

    public void HandleRotateCommand(RotateCommand cmd)
    {
        float newTarget = cmd.angle;
        if (cmd.relative)
        {
            newTarget += transform.rotation.eulerAngles.y;
        }
        if (newTarget > 360.0f) newTarget -= 360.0f;
        else if (newTarget < 0) newTarget += 360.0f;

        MoveTask rotateTask = new MoveTask();
        rotateTask.rotateCommand = cmd;
        rotateTask.targetRotation = newTarget;
        rotateTask.rotationDir = cmd.angle > 0.0 ? 1.0f : -1.0f;
        rotateTask.initialDiff = GetRotationDiff(transform.rotation.eulerAngles.y, newTarget);
        StartProgressiveTask(rotateTask);
    }

    protected override void ExecuteWhileTaskRunning(MoveTask task)
    {
        if (task.moveCmd != null)
        {
            UpdatePosition(task);
        }
        else if (task.rotateCommand != null)
        {
            UpdateRotation(task);
        }
    }

    private void UpdatePosition(MoveTask task)
    {
        Vector3 posDiff = GetPositionDiff(transform.position, task.targetPosition);
        if (posDiff.magnitude <= positionTolerance)
        {
            transform.position = task.targetPosition;
            task.targetPosition = transform.position;
        }
        else
        {
            float speedMult = 1.0f;
            if (posDiff.magnitude <= positionDeadzone)
            {
                speedMult = posDeadzoneMult;
            }

            transform.position += posDiff.normalized * speed * speedMult * Time.deltaTime;
        }
    }

    private void UpdateRotation(MoveTask task)
    {
        float rotDiff = GetRotationDiff(transform.rotation.eulerAngles.y, task.targetRotation);
        if (rotDiff <= rotationTolerance)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, task.targetRotation, transform.rotation.eulerAngles.z);
            task.targetRotation = transform.rotation.eulerAngles.y;
        }
        else
        {
            float speedMult = 1.0f;
            if (rotDiff <= rotDeadZone)
            {
                // ramp speed down as we get closer
                speedMult = rotDeadzoneMult;
            }
            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + rotDir * rotSpeed * speedMult * Time.deltaTime, 0.0f);
        }
    }

    protected Vector3 GetPositionDiff(Vector3 sourcePosition, Vector3 targetPosition)
    {
        return targetPosition - sourcePosition;
    }

    protected float GetRotationDiff(float sourceRotation, float targetRotation)
    {
        float rotDiff = Mathf.Abs(targetRotation - sourceRotation);
        if (rotDiff > 360) rotDiff -= 360.0f;
        return rotDiff;
    }

    protected override bool CheckTaskComplete(MoveTask task)
    {
        if (task.moveCmd != null)
        {
            return task.targetPosition == transform.position;
        }
        else if (task.rotateCommand != null)
        {
            return task.targetRotation == transform.rotation.eulerAngles.y;
        }
        else return true;
    }

    protected override float CalculateCompletionPercentage(MoveTask task)
    {
        float currentDiff = 0.0f;
        if (task.moveCmd != null)
        {
            currentDiff = GetPositionDiff(transform.position, task.targetPosition).magnitude;
        }
        else if (task.rotateCommand != null)
        {
            currentDiff = GetRotationDiff(transform.rotation.eulerAngles.y, task.targetRotation);
        }

        if (task.initialDiff != 0.0)
        {
            return (task.initialDiff - currentDiff) / task.initialDiff;
        }

        return 0.0f;
    }
}
