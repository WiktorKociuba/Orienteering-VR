using UnityEditor.UI;
using UnityEngine;

public class syncCam : MonoBehaviour
{
    public Camera playCam;
    void Update()
    {
        if(enabled == false){
            playCam.transform.rotation = transform.rotation;
        }
    }
}
