using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.Collections;


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
    public int slowZone;
    public Camera VRCamera;

    public float maxSpeedConst;
    private Vector3 movementDirection;
    private float speed = 0.0f;
    private float slopeRaycastOffset = 0.5f;
    private float calcSlopeForce;
    private float slopeAngle;
    private bool isMoving = false;
    private bool isBoosting = false;
    private float boost = 5.0f;
    private void Start()
    {
        maxSpeedConst = maxSpeed;
    }

    private void FixedUpdate()
    {
        Vector2 input = moveValue.axis;
        if (input.sqrMagnitude > 0.05f)
        {
            print(input.x + " " + input.y);
            Vector3 forward = new Vector3(VRCamera.transform.forward.x, 0,VRCamera.transform.forward.z).normalized;
            movementDirection = new Vector3(input.x,0,input.y).normalized;
            movementDirection = Quaternion.LookRotation(forward) * movementDirection;
            //speed = Mathf.Lerp(speed, maxSpeed * input.magnitude * sensitivity, Time.fixedDeltaTime * acceleration);
            speed = maxSpeed * input.magnitude * sensitivity;
            if (OnSlope())
            {
                slopeAngle /= 100;
                Vector3 slopeNormal = GetGroundNormal();
                calcSlopeForce = slopeForce * Mathf.Clamp01(slopeAngle / 30f);
                calcSlopeForce = Mathf.Max(calcSlopeForce, 1.0f); 
                Vector3 slopeDirection = Vector3.ProjectOnPlane(movementDirection, slopeNormal).normalized;
                if(!isMoving && !isBoosting)
                {
                    StartCoroutine(ApplyBoost(slopeDirection));
                    isMoving = true;
                }
                body.AddForce(slopeDirection * speed * calcSlopeForce, ForceMode.Acceleration);
            }
            else
            {
                body.linearVelocity = new Vector3(movementDirection.x * speed, body.linearVelocity.y, movementDirection.z * speed);
                isMoving = true;
            }
        }
        else
        {
            body.linearVelocity = new Vector3(0,0,0);
            isMoving = false;
        }
    }

    private IEnumerator ApplyBoost(Vector3 slopeDirection)
    {
        isBoosting = true;
        float boostDuration = 1f;
        float elapsedTime = 0f;
        while(elapsedTime<boostDuration)
        {
            body.AddForce(slopeDirection * boost, ForceMode.Acceleration);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        isBoosting = false;
    }

    private bool OnSlope()
    {
        Vector3 rayOrigin = body.position + Vector3.up * slopeRaycastOffset;
        if (Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, slopeRayLength) && hit.transform.tag == "Terrain"){
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return Vector3.Angle(hit.normal, Vector3.up) > 10 && Vector3.Angle(hit.normal, Vector3.up) < 80;
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
