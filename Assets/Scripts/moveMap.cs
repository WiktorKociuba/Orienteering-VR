using UnityEngine;
using UnityEngine.InputSystem;

public class moveMap : MonoBehaviour
{
    public float moveSpeed = 0.7f;

    private InputAction move;

    void Start()
    {
        move = InputSystem.actions.FindAction("moveMap");
    }
    void Update()
    {
        float movement = move.ReadValue<float>() * moveSpeed * Time.deltaTime;
        transform.Translate(movement,0,0);
    }
}
