using UnityEngine;
using UnityEngine.InputSystem;

public class rotateMap : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private InputAction rotate;

    void Start()
    {
        rotate = InputSystem.actions.FindAction("Rotate");
    }
    void Update()
    {
        float rotation = rotate.ReadValue<float>() * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, rotation);
    }
}
