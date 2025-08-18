using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertMotorics : MonoBehaviour
{
    Vector3 targetPosition;
    float targetRotation;

    float speed = 3.5f;
    float posDeadzoneMult = 0.4f;
    float positionDeadzone = 0.3f;
    float positionTolerance = 0.01f;

    float rotSpeed = 120.0f;
    float rotDir = 1.0f;
    float rotDeadzoneMult = 0.1f;
    float rotDeadZone = 1.0f;
    float rotationTolerance = 0.1f;

    bool moving = false;
    bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            Vector3 posDiff = targetPosition - transform.position;
            if (posDiff.magnitude <= positionTolerance)
            {
                transform.position = targetPosition;
                targetPosition = transform.position;
                moving = false;
            }
            else
            {
                float speedMult = 1.0f;
                if (posDiff.magnitude <= positionDeadzone)
                {
                    //Debug.Log("IN DEADZONE");
                    speedMult = posDeadzoneMult;
                }

                transform.position += posDiff.normalized * speed * speedMult * Time.deltaTime;
            }
        }

        if (rotating)
        {
            float rotDiff = Mathf.Abs(targetRotation - transform.rotation.eulerAngles.y);
            if (rotDiff > 360) rotDiff -= 360.0f;

            if (rotDiff <= rotationTolerance)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation, transform.rotation.eulerAngles.z);
                targetRotation = transform.rotation.eulerAngles.y;
                rotating = false;
            }
            else
            {
                float speedMult = 1.0f;
                if (rotDiff <= rotDeadZone)
                {
                    Debug.Log("IN DEADZONE");
                    speedMult = rotDeadzoneMult;
                }
                transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + rotDir * rotSpeed * speedMult * Time.deltaTime, 0.0f);
            }
        }

    }

    public void HandleMoveCommand(MoveCommand cmd)
    {
        moving = true;
        Vector3 newTarget = new Vector3(cmd.position[0], 0.0f, cmd.position[1]);
        if (cmd.relative)
        {
            newTarget = transform.position + transform.forward * cmd.position[0] + transform.right * cmd.position[1];
        }
        newTarget[1] = transform.position.y;
        targetPosition = newTarget;
    }

    public void HandleRotateCommand(RotateCommand cmd)
    {
        rotating = true;
        float newTarget = cmd.angle;
        if (cmd.relative)
        {
            newTarget += transform.rotation.eulerAngles.y;
        }
        if (newTarget > 360.0f) newTarget -= 360.0f;
        else if (newTarget < 0) newTarget += 360.0f;
        targetRotation = newTarget;
        rotDir = cmd.angle > 0.0 ? 1.0f : -1.0f;
    }

    public bool inMotion()
    {
        return moving || rotating;
    }

    public void Halt()
    {
        moving = false;
        rotating = false;
        targetPosition = transform.position;
        targetRotation = transform.rotation.eulerAngles.y;
    }
}
