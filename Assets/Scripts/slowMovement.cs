using UnityEngine;

public class slowMovement : MonoBehaviour
{
    [Header("Settings")]
    public MovePlayer movePlayer1;
    public MovePlayerPC movePlayerPC1;
    public MovePlayer movePlayer2;
    public MovePlayerPC movePlayerPC2;
    public GameObject Player1;
    public GameObject PlayerPC1;
    public GameObject Player2;
    public GameObject PlayerPC2;
    public float newSpeed;
    public int slowZone;

    private void OnTriggerEnter(Collider other)
    {
        print("here");
        if (other.gameObject.CompareTag("Player"))
        {
            if(Player1.activeSelf)
            {
                if (movePlayer1.slowZone == slowZone)
                {
                    movePlayer1.maxSpeed = movePlayer1.maxSpeedConst;
                    movePlayer1.slowZone = 0;
                    movePlayer1.boost = movePlayer1.boostConst;
                }
                else
                {
                    movePlayer1.slowZone = slowZone;
                    movePlayer1.maxSpeed = newSpeed;
                    movePlayer1.boost = movePlayer1.boostConst + 3 + slowZone;
                }
            }
            if(PlayerPC1.activeSelf)
            {
                if (movePlayerPC1.slowZone == slowZone)
                {
                    movePlayerPC1.maxSpeed = movePlayerPC1.maxSpeedConst;
                    movePlayerPC1.slowZone = 0;
                }
                else
                {
                    movePlayerPC1.slowZone = slowZone;
                    movePlayerPC1.maxSpeed = newSpeed + 2;
                }
            }
            if(Player2 != null){
                if(Player2.activeSelf)
                {
                    if (movePlayer2.slowZone == slowZone)
                    {
                        movePlayer2.maxSpeed = movePlayer2.maxSpeedConst;
                        movePlayer2.slowZone = 0;
                        movePlayer1.boost = movePlayer1.boostConst;
                    }
                    else
                    {
                        movePlayer2.slowZone = slowZone;
                        movePlayer2.maxSpeed = newSpeed;
                        movePlayer1.boost = movePlayer1.boostConst + 3 + slowZone;
                    }
                }}
            if(PlayerPC2 != null && PlayerPC2.activeSelf)
            {
                if (movePlayerPC2.slowZone == slowZone)
                {
                    movePlayerPC2.maxSpeed = movePlayerPC2.maxSpeedConst;
                    movePlayerPC2.slowZone = 0;
                }
                else
                {
                    movePlayerPC2.slowZone = slowZone;
                    movePlayerPC2.maxSpeed = newSpeed + 2;
                }
            }
        }
    }
}
