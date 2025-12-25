using UnityEngine;
using UnityEngine.InputSystem;

public class moveCompass : MonoBehaviour
{
    public float moveSpeed = 0.000001f;
    private InputAction moveComp;
    void Start()
    {
        moveComp = InputSystem.actions.FindAction("moveCompass");
    }

    void Update()
    {
        Vector2 input = moveComp.ReadValue<Vector2>();
        Vector3 movement = new Vector3(input.x, 0, input.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}
