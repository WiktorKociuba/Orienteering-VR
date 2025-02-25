using UnityEngine;

public class slowMovement : MonoBehaviour
{
    [Header("Settings")]
    public MovePlayer movePlayer;
    public float newSpeed;
    public int slowZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (movePlayer.slowZone == slowZone)
            {
                movePlayer.maxSpeed = movePlayer.maxSpeedConst;
                movePlayer.slowZone = 0;
            }
            else
            {
                movePlayer.slowZone = slowZone;
                movePlayer.maxSpeed = newSpeed;
            }
        }
    }
}
