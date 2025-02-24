using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.Threading;
public class pauseMenu : MonoBehaviour
{
[SerializeField] LineRenderer rend;
[SerializeField] LineRenderer rend2;

    [Header("Settings")]
    public SteamVR_Action_Boolean pauseGame;
    public GameObject pauseMenuUI;
    public MonoBehaviour moveScript;

    public bool paused = false;
    public bool flag = false;
    private void Update()
    {
        if (pauseGame.GetState(SteamVR_Input_Sources.Any) && flag == false)
        {
            flag = true;
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
        else if (!pauseGame.GetState(SteamVR_Input_Sources.Any))
        {
            flag = false;
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
        rend.enabled = false;
        rend2.enabled = false;
    }
}
