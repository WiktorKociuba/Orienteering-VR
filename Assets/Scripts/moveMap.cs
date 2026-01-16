using UnityEngine;
using UnityEngine.InputSystem;

public class moveMap : MonoBehaviour
{
    public float moveSpeed = 0.7f;

    private InputAction move;
    private InputAction move2D;

    void Start()
    {
        move = InputSystem.actions.FindAction("moveMap");
        move2D = InputSystem.actions.FindAction("moveMap2D");
    }
    void Update()
    {
        float movement = move.ReadValue<float>() * moveSpeed * Time.deltaTime;
        transform.Translate(movement,0,0);
        Vector2 input = move2D.ReadValue<Vector2>();
        Vector3 movement2D = new Vector3(input.x,0,input.y)*moveSpeed*Time.deltaTime;
        transform.Translate(movement2D);
    }
}
