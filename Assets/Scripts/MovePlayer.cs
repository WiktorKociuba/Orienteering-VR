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
    public GameObject camera;
    public AudioSource runningAudioSource;

    public float maxSpeedConst;
    private Vector3 movementDirection;
    private float speed = 0.0f;
    private float slopeRaycastOffset = 0.5f;
    private float calcSlopeForce;
    private float slopeAngle;
    private bool isMoving = false;
    private bool isBoosting = false;
    public float boost = 1.3f;
    public float boostConst;
    private void Start()
    {
        maxSpeedConst = maxSpeed;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        boostConst = boost;
    }

    private void FixedUpdate()
    {
        Vector2 input = moveValue.axis;
        if (input.sqrMagnitude > 0.05f)
        {
            if(!runningAudioSource.isPlaying)
            {
                runningAudioSource.Play();
            }
            Vector3 forward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z).normalized;
            movementDirection = new Vector3(input.x,0,input.y).normalized;
            movementDirection = Quaternion.LookRotation(forward) * movementDirection;
            speed = Mathf.Lerp(speed, maxSpeed * input.magnitude * sensitivity, Time.fixedDeltaTime * acceleration);
            if (OnSlope())
            {
                slopeAngle /= 100;
                if(slopeAngle < 30)
                {
                    calcSlopeForce = slopeForce * Mathf.Sqrt(slopeAngle)/1.4f;
                }
                else
                {
                    calcSlopeForce = (slopeForce * Mathf.Sqrt(slopeAngle)/0.6f)*boost;
                }
                Vector3 slopeDirection = Vector3.ProjectOnPlane(movementDirection, GetGroundNormal()).normalized;
                body.AddForce(slopeDirection * speed * calcSlopeForce, ForceMode.Acceleration);
            }
            else
            {
                Vector3 targetVelocity = new Vector3(movementDirection.x * speed, body.linearVelocity.y, movementDirection.z * speed);
                body.linearVelocity = Vector3.Lerp(body.linearVelocity, targetVelocity, Time.fixedDeltaTime * deceleration);
            }
        }
        else
        {
            if(runningAudioSource.isPlaying)
            {
                runningAudioSource.Stop();
            }
            body.linearVelocity = new Vector3(0,0,0);
        }
    }

    private IEnumerator ApplyBoost(Vector3 slopeDirection)
    {
        isBoosting = true;
        float boostDuration = 1f;
        float elapsedTime = 0f;
        while(true)
        {
            body.AddForce(slopeDirection * boost, ForceMode.Acceleration);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private bool OnSlope()
    {
        Vector3 rayOrigin = body.position * slopeRaycastOffset;
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
