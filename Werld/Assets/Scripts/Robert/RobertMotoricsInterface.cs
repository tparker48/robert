using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class MoveTask
{
    public Move moveCmd = null;
    public Vector3 targetPosition;

    public Rotate rotateCommand = null;
    public float targetRotation;
    public float rotationDir;

    public float initialDiff = 0.0f;
}

public class RobertMotoricsInterface : RobertProgressiveTaskExecutor<MoveTask>
{
    float speed = 3.5f;
    float posDeadzoneMult = 0.4f;
    float positionDeadzone = 0.3f;
    float positionTolerance = 0.01f;

    float rotSpeed = 100.0f;
    float rotDeadzoneMult = 0.08f;
    float rotDeadZone = 1.25f;
    float rotationTolerance = 0.30f;


    // Update is called once per frame
    void Update()
    {
        UpdateTask();
    }

    public void HandleMove(string rawCmd)
    {
        Move cmd = CommandParser.Parse<Move>(rawCmd);
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
        busyText = "Moving";
        StartProgressiveTask(moveTask);
    }

    public void HandleRotate(string rawCmd)
    {
        Rotate cmd = CommandParser.Parse<Rotate>(rawCmd);

        float newTarget = cmd.angle;
        if (cmd.relative)
        {
            newTarget += transform.localEulerAngles.y;
        }
        if (newTarget > 360.0f) newTarget -= 360.0f;
        else if (newTarget < 0) newTarget += 360.0f;

        MoveTask rotateTask = new MoveTask();
        rotateTask.rotateCommand = cmd;
        rotateTask.targetRotation = newTarget;
        rotateTask.rotationDir = cmd.angle > 0.0 ? 1.0f : -1.0f;
        rotateTask.initialDiff = GetRotationDiff(transform.localEulerAngles.y, newTarget);
        busyText = "Rotating";
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
            transform.position = new Vector3(task.targetPosition.x, transform.position.y, task.targetPosition.z);
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
        float rotDiff = GetRotationDiff(transform.localEulerAngles.y, task.targetRotation);
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
            //transform.rotation = Quaternion.Euler(0.0f, transform.localEulerAngles.y + rotDir * rotSpeed * speedMult * Time.deltaTime, 0.0f);
            float rotationAmount = task.rotationDir * rotSpeed * speedMult * Time.deltaTime;
            transform.Rotate(Vector3.up * rotationAmount, Space.Self);
        }
    }

    protected Vector3 GetPositionDiff(Vector3 sourcePosition, Vector3 targetPosition)
    {
        sourcePosition.y = 0;
        targetPosition.y = 0;
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
            return task.targetRotation == transform.localEulerAngles.y;
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
            currentDiff = GetRotationDiff(transform.localEulerAngles.y, task.targetRotation);
        }

        if (task.initialDiff != 0.0)
        {
            return (task.initialDiff - currentDiff) / task.initialDiff;
        }

        return 0.0f;
    }

    public Response HandleGetPosition(string _)
    {
        GetPositionResponse position_response = new GetPositionResponse();
        position_response.position = new float[3] {
                    transform.position.x,
                    transform.position.y,
                    transform.position.z
                };
        position_response.rotation = new float[3] {
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y,
                    transform.rotation.eulerAngles.z
                };
        return position_response;
    }

    public Response HandleGetShipFloor(string _) {
        GetFloorResponse shipFloorResponse = new GetFloorResponse();
        shipFloorResponse.floor = Ship.GetFloor(transform.position);
        if (shipFloorResponse.floor == -1)
        {
            return Response.ErrorResponse("Not on ship!");
        }
        else
        {  
            return shipFloorResponse;
        }
    }
}
