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
    public void OnClick()
    {
        if (btn != null)
        {
            if (btn.name == "Start")
            {
                start.gameObject.SetActive(false);
                exit.gameObject.SetActive(false);
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
                Destroy(player);
                SceneManager.UnloadSceneAsync("mainMenu");
                SceneManager.LoadScene("demoMap");
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
            }
            else if(btn.name == "map1course1")
            {
                Destroy(player);
                routeManager.SelectRoute(0);
                SceneManager.LoadScene("map1");
            }
            else if(btn.name == "map1course2"){
                Destroy(player);
                routeManager.SelectRoute(1);
                SceneManager.LoadScene("map1");
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
}
