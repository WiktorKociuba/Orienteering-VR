using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class clickMenuPC : MonoBehaviour
{
    [Header("Settings")]
    public Button btn;
    public Button start;
    public Button exit;
    public Button demoMap;
    public Button map1;
    public bool menu;
    public GameObject player;
    public GameObject level1;
    public GameObject map1course1;
    public pauseMenuPC pauseMenuScript;
    public GameObject map1course2;
    public GameObject nightmode;
    public GameObject nightmodeenable;
    public GameObject settingsButton;
    public GameObject settingsMenu;
    public GameObject soundSettings;
    public GameObject screenSettings;
    public GameObject soundSettingsButton;
    public GameObject graphicSettingsButton;
    public void OnClick()
    {
        if (btn != null)
        {
            if (btn.name == "Start")
            {
                start.gameObject.SetActive(false);
                exit.gameObject.SetActive(false);
                settingsButton.SetActive(false);
                demoMap.gameObject.SetActive(true);
                map1.gameObject.SetActive(true);
            }
            else if (btn.name == "Exit")
            {
                Application.Quit();
            }
            else if(btn.name == "demoMap")
            {
                map1.gameObject.SetActive(false);
                demoMap.gameObject.SetActive(false);
                level1.gameObject.SetActive(true);
            }
            else if(btn.name == "map"){
                StartCoroutine(loadSceneAsync("test"));
            }
            else if(btn.name == "exitMenu"){
                Time.timeScale = 1f;
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("mainMenu");
                Destroy(player);
            }
            else if(btn.name == "Resume"){
                if(pauseMenuScript != null)
                {
                    pauseMenuScript.ifPaused = false;
                    pauseMenuScript.Resume();
                    pauseMenuScript.flag = false;
                }
            }
            else if(btn.name == "map1")
            {
                map1.gameObject.SetActive(false);
                demoMap.gameObject.SetActive(false);
                map1course1.gameObject.SetActive(true);
                map1course2.gameObject.SetActive(true);
                nightmode.gameObject.SetActive(true);
            }
            else if(btn.name == "map1course1")
            {
                StartCoroutine(loadSceneAsync("map1",0));
            }
            else if(btn.name == "map1course2"){
                StartCoroutine(loadSceneAsync("map1",1));
            }
            else if(btn.name == "nightModeDisabled")
            {
                routeManager.SelectNight();
                nightmode.SetActive(false);
                nightmodeenable.SetActive(true);
            }
            else if(btn.name == "nightModeEnabled")
            {
                routeManager.SelectDay();
                nightmodeenable.SetActive(false);
                nightmode.SetActive(true);
            }
            else if(btn.name == "settingsButton")
            {
                start.gameObject.SetActive(false);
                exit.gameObject.SetActive(false);
                settingsButton.SetActive(false);
                settingsMenu.SetActive(true);
                soundSettings.SetActive(true);
            }
            else if(btn.name == "soundSettingsButton")
            {
                screenSettings.SetActive(false);
                soundSettings.SetActive(true);
            }
            else if(btn.name == "graphicSettingsButton")
            {
                screenSettings.SetActive(true);
                soundSettings.SetActive(false);
            }
            else if(btn.name == "backSettings")
            {
                screenSettings.SetActive(false);
                soundSettings.SetActive(false);
                settingsMenu.SetActive(false);
                start.gameObject.SetActive(true);
                settingsButton.SetActive(true);
                exit.gameObject.SetActive(true);
            }
            else if(btn.name == "backDemoMap")
            {
                level1.SetActive(false);
                demoMap.gameObject.SetActive(true);
                map1.gameObject.SetActive(true);
            }
            else if(btn.name == "backMap1")
            {
                map1course1.SetActive(false);
                map1course2.SetActive(false);
                nightmode.SetActive(false);
                nightmodeenable.SetActive(false);
                demoMap.gameObject.SetActive(true);
                map1.gameObject.SetActive(true);
            }
            else if(btn.name == "backMapMenu")
            {
                map1.gameObject.SetActive(false);
                demoMap.gameObject.SetActive(false);
                start.gameObject.SetActive(true);
                settingsButton.SetActive(true);
                exit.gameObject.SetActive(true);
            }
            else if(btn.name == "tutorial")
            {
                StartCoroutine(loadSceneAsync("tutorial"));
            }
        }
        btn = null;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Button clickedButton = hit.collider.gameObject.GetComponent<Button>();
                if(clickedButton != null)
                {
                    btn = clickedButton;
                    clickedButton.onClick.Invoke();
                }
            }
        }
    }
    public GameObject loadingScreen;
    public Image loadingBar;
    public GameObject fadeToBlack;
    IEnumerator loadSceneAsync(string scene, int route = -1){
        fadeToBlack.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        if(loadingScreen != null)
            loadingScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        fadeToBlack.SetActive(false);
        if(route > -1)    
            routeManager.SelectRoute(route);
        loadingBar.fillAmount = 0f;
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        while(!operation.isDone){
            float progressVall = Mathf.Clamp01(operation.progress/0.9f);
            loadingBar.fillAmount = progressVall;
            yield return null;
        }
    }
}
