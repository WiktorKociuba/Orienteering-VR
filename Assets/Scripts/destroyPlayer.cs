using UnityEngine;

public class destroyPlayer : MonoBehaviour
{
    public GameObject player;
    public void destroyVR(){
        if(player != null)
            Destroy(player);
    }   
}