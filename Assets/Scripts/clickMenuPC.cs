using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;
using System.Linq;
using System;
using TMPro;

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
    public GameObject generateMapMenu;
    public TextMeshProUGUI generateMapErr;
    public TextMeshProUGUI mapFileStr;
    public TextMeshProUGUI courseFileStr;
    public TextMeshProUGUI imageFileStr;
    public GameObject mapGeneration;
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
                mapGeneration.SetActive(true);
            }
            else if (btn.name == "Exit")
            {
                Application.Quit();
            }
            else if(btn.name == "demoMap")
            {
                map1.gameObject.SetActive(false);
                demoMap.gameObject.SetActive(false);
                mapGeneration.SetActive(false);
                level1.gameObject.SetActive(true);
            }
            else if(btn.name == "map"){
                StartCoroutine(loadSceneAsync("demoMap"));
            }
            else if(btn.name == "exitMenu"){
                Time.timeScale = 1f;
                StartCoroutine(loadSceneAsync("mainMenu"));
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
                mapGeneration.SetActive(false);
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
                generateMapMenu.SetActive(false);
                start.gameObject.SetActive(true);
                settingsButton.SetActive(true);
                exit.gameObject.SetActive(true);
            }
            else if(btn.name == "tutorial")
            {
                StartCoroutine(loadSceneAsync("tutorial"));
            }
            else if(btn.name == "loadMapFile")
            {
                FileBrowser.SetFilters(true, new FileBrowser.Filter("OMAP", ".omap"));
                FileBrowser.SetDefaultFilter(".omap");
                FileBrowser.ShowLoadDialog(
                    (paths) => {
                        routeManager.mapFilePath = paths[0];
                        mapFileStr.text = paths[0];
                    },
                    () => {Debug.Log("Cancelled");},
                    FileBrowser.PickMode.Files,
                    false,
                    null,
                    null,
                    "Select Map File",
                    "Select"
                );
            }
            else if(btn.name == "loadCourseFile")
            {
                FileBrowser.SetFilters(true, new FileBrowser.Filter("OMAP",".omap"));
                FileBrowser.SetDefaultFilter(".omap");
                FileBrowser.ShowLoadDialog(
                    (paths) => {
                        routeManager.courseFilePath = paths[0];
                        courseFileStr.text = paths[0];
                    },
                    () => {Debug.Log("Cancelled");},
                    FileBrowser.PickMode.Files,
                    false,
                    null,
                    null,
                    "Select Course File",
                    "Select"
                );
            }
            else if(btn.name == "loadMapImage")
            {
                FileBrowser.SetFilters(true, new FileBrowser.Filter("Image",".png",".jpg",".jpeg"));
                FileBrowser.SetDefaultFilter(".png");
                FileBrowser.ShowLoadDialog(
                    (paths) => {
                        routeManager.mapImagePath = paths[0];
                        imageFileStr.text = paths[0];
                    },
                    () => {Debug.Log("Cancelled");},
                    FileBrowser.PickMode.Files,
                    false,
                    null,
                    null,
                    "Select Map Image",
                    "Select" 
                );
            }
            else if(btn.name == "generateMap"){
                string errorMsg = "";
                if(routeManager.mapFilePath == null){
                    errorMsg = errorMsg + "Map File Path not set! ";
                }
                if(routeManager.courseFilePath == null){
                    errorMsg = errorMsg +"Course File Path not set! ";
                }
                if(routeManager.mapImagePath == null){
                    errorMsg = errorMsg + "Map Image Path not set! ";
                }
                if(errorMsg != ""){
                    generateMapErr.text = errorMsg;
                    btn = null;
                    return;
                }
                StartCoroutine(loadSceneAsync("generatedMap"));
            }
            else if(btn.name == "mapGeneration"){
                demoMap.gameObject.SetActive(false);
                map1.gameObject.SetActive(false);
                mapGeneration.SetActive(false);
                generateMapMenu.SetActive(true);
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
    public IEnumerator loadSceneAsync(string scene, int route = -1){
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
