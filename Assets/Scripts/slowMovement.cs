using UnityEngine;

public class slowMovement : MonoBehaviour
{
    [Header("Settings")]
    public MovePlayer movePlayer;
    public MovePlayerPC movePlayerPC;
    public GameObject Player;
    public GameObject PlayerPC;
    public float newSpeed;
    public int slowZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(Player.activeSelf)
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
            if(PlayerPC.activeSelf)
            {
                if (movePlayerPC.slowZone == slowZone)
                {
                    movePlayerPC.maxSpeed = movePlayerPC.maxSpeedConst;
                    movePlayerPC.slowZone = 0;
                    print("out");
                }
                else
                {
                    movePlayerPC.slowZone = slowZone;
                    movePlayerPC.maxSpeed = newSpeed;
                    print("in");
                }
            }
        }
    }
}
