using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class MovePlayer : MonoBehaviour
{
    [Header("Settings")]
    public SteamVR_Action_Vector2 moveValue;
    public float acceleration;
    public float deceleration;
    public float rotationRate;
    public float maxSpeed;
    public float sensitivity;
    public float slopeForce;
    public float slopeRayLength;
    public Rigidbody body;

    private Vector3 movementDirection;
    private float speed = 0.0f;
    private float slopeRaycastOffset = 0.5f;

    private void FixedUpdate()
    {
        Vector2 input = moveValue.axis;
        if (input.sqrMagnitude > 0.05f)
        {
            movementDirection = Player.instance.hmdTransform.TransformDirection(new Vector3(moveValue.axis.x, 0, moveValue.axis.y));
            speed = Mathf.Lerp(speed, maxSpeed * input.magnitude * sensitivity, Time.fixedDeltaTime * acceleration);
            if (OnSlope())
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(movementDirection, GetGroundNormal()).normalized;
                body.AddForce(slopeDirection * speed * slopeForce, ForceMode.Acceleration);
            }
            else
            {
                body.linearVelocity = new Vector3(movementDirection.x * speed, body.linearVelocity.y, movementDirection.z * speed);
            }
        }
        else
        {
            speed = Mathf.Lerp(speed,0,Time.fixedDeltaTime * deceleration);
            body.linearVelocity = new Vector3(0, body.linearVelocity.y, 0);
        }
    }

    private bool OnSlope()
    {
        Vector3 rayOrigin = body.position + movementDirection * slopeRaycastOffset;
        if (Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, slopeRayLength)){
            return Vector3.Angle(hit.normal, Vector3.up) > 5 && Vector3.Angle(hit.normal, Vector3.up) < 45;
        }
        return false;
    }

    private Vector3 GetGroundNormal()
    {
        if (Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, slopeRayLength))
        {
            return hit.normal;
        }
        return Vector3.up;
    }
}
