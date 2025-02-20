using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class pauseMenu : MonoBehaviour
{
    [Header("Settings")]
    public SteamVR_Action_Boolean pauseGame;
    public GameObject pauseMenuUI;
    public MonoBehaviour moveScript;

    private bool paused = false;
    private void Update()
    {
        if (pauseGame.GetState(SteamVR_Input_Sources.Any))
        {
            if (paused == false)
            {
                Pause();
                paused = true;
            }
            else
            {
                Resume();
                paused = false;
            }
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        moveScript.enabled = false;
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        moveScript.enabled = true;
    }
}
