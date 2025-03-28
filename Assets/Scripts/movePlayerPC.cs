using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class MovePlayerPC : MonoBehaviour
{
    [Header("Settings")]
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
    public float mouseSensitivity = 5.0f;
    public AudioSource runningAudioSource;
    public float maxSpeedConst;
    private Vector3 movementDirection;
    private float speed = 0.0f;
    private float slopeRaycastOffset = 0.5f;
    private InputAction moveValue;
    private InputAction sprint;
    private float verticalRotation = 0.0f;
    private float slopeAngle;
    private float calcSlopeForce;
    private float maxSpeedSprint;
    private void Start()
    {
        maxSpeedConst = maxSpeed;
        maxSpeedSprint = maxSpeed;
        moveValue = InputSystem.actions.FindAction("Move");
        sprint = InputSystem.actions.FindAction("sprint");
        Cursor.lockState = CursorLockMode.Locked;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FixedUpdate()
    {
        float ifSprint = sprint.ReadValue<float>();
        Vector2 input = moveValue.ReadValue<Vector2>();

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
                    calcSlopeForce = slopeForce * Mathf.Sqrt(slopeAngle)/0.6f;
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
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.fixedDeltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private bool OnSlope()
    {
        Vector3 rayOrigin = body.position * slopeRaycastOffset;
        if (Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, slopeRayLength) && hit.transform.tag == "Terrain"){
            slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            //print(slopeAngle);
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

