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

    public float maxSpeedConst;
    private Vector3 movementDirection;
    private float speed = 0.0f;
    private float slopeRaycastOffset = 0.5f;
    private InputAction moveValue;
    private float verticalRotation = 0.0f;
    private void Start()
    {
        maxSpeedConst = maxSpeed;
        moveValue = InputSystem.actions.FindAction("Move");
        Cursor.lockState = CursorLockMode.Locked;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    private void FixedUpdate()
    {
        Vector2 input = moveValue.ReadValue<Vector2>();
        if (input.sqrMagnitude > 0.05f)
        {
            Vector3 forward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z).normalized;
            movementDirection = new Vector3(input.x,0,input.y).normalized;
            movementDirection = Quaternion.LookRotation(forward) * movementDirection;
            speed = Mathf.Lerp(speed, maxSpeed * input.magnitude * sensitivity, Time.fixedDeltaTime * acceleration);
            if (OnSlope())
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(movementDirection, GetGroundNormal()).normalized;
                body.AddForce(slopeDirection * speed * slopeForce, ForceMode.Acceleration);
                body.AddForce(Vector3.up, ForceMode.Acceleration);
            }
            else
            {
                Vector3 targetVelocity = new Vector3(movementDirection.x * speed, body.linearVelocity.y, movementDirection.z * speed);
                body.linearVelocity = Vector3.Lerp(body.linearVelocity, targetVelocity, Time.fixedDeltaTime * deceleration);
            }
        }
        else
        {
            speed = Mathf.Lerp(speed,0,Time.fixedDeltaTime * deceleration);
            body.linearVelocity = new Vector3(0, body.linearVelocity.y, 0);
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
        Vector3 rayOrigin = body.position + Vector3.up * slopeRaycastOffset;
        if (Physics.Raycast(body.position, Vector3.down, out RaycastHit hit, slopeRayLength) && hit.transform.tag == "Terrain"){
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

