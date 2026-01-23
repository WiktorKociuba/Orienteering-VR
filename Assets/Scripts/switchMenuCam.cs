using UnityEngine;
using UnityEngine.InputSystem;
public class switchMenuCam : MonoBehaviour
{
    public GameObject moveCam;
    public GameObject staticCam;
    public GrassComputeScript grassCompute;
    private InputAction changeStateBut;
    private bool flag;
    void Start(){
        changeStateBut = InputSystem.actions.FindAction("Pause");
    }
    void Update(){
        float input = changeStateBut.ReadValue<float>();
        if(input!=0 && !flag){
            flag = true;
            if(moveCam.activeSelf == true){
                moveCam.SetActive(false);
                staticCam.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else{
                staticCam.SetActive(false);
                moveCam.SetActive(true);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        if(input==0){
            flag = false;
        }
    }
}
