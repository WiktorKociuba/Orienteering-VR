using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class pauseMenuPC : MonoBehaviour
{
    public InputAction pause;
    public GameObject pauseMenuUI;
    public MonoBehaviour moveScript;

    public bool ifPaused = false;
    public bool flag = false;
    private void Start()
    {
        pause = InputSystem.actions.FindAction("Pause");
    }

    private void Update()
    {
        float input = pause.ReadValue<float>();
        if(input != 0 && !flag)
        {
            flag = true;
            if(!ifPaused){
                Pause();
                ifPaused = true;
            }
            else
            {
                Resume();
                ifPaused = false;
            }
        }
        else if(input == 0)
        {
            flag = false;
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        moveScript.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        moveScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
