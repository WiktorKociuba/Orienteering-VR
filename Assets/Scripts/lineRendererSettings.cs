using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.Newtonsoft.Json.Bson;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;

public class lineRendererSettings : MonoBehaviour
{
    [SerializeField] LineRenderer rend;
    [SerializeField] LineRenderer rend2;

    [Header("Settings")]
    public SteamVR_Action_Boolean Submit;
    public LayerMask layerMask;
    public GameObject panel;
    public Image img;
    public Button btn;
    public Slider slider;
    public TMP_Dropdown dropdown;
    public Button start;
    public Button exit;
    public Button demoMap;
    public Button map1;
    public bool menu;
    public pauseMenu pauseMenuScript;
    public GameObject player;
    public GameObject level1;
    public GameObject map1course1;
    public GameObject map1course2;
    public GameObject nightmode;
    public GameObject nightmodeenable;
    public GameObject settingsButton;
    public GameObject settingsMenu;
    public GameObject soundSettings;
    public GameObject screenSettings;
    public GameObject soundSettingsButton;
    public GameObject graphicSettingsButton;
    private float lastClickTime = 0f;
    private float clickCooldown = 0.2f;
    Vector3[] points;
    public bool AlignLineRenderer(LineRenderer rend) // Make the line follow Ray
    {
        bool hitBtn = false;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            points[1] = transform.forward + new Vector3(0, 0, hit.distance);
            rend.startColor = Color.red;
            rend.endColor = Color.red;
            btn = hit.collider.gameObject.GetComponent<Button>();
            slider = hit.collider.gameObject.GetComponent<Slider>();
            dropdown = hit.collider.gameObject.GetComponent<TMP_Dropdown>();
            hitBtn = true;
        }
        else
        {
            points[1] = transform.forward + new Vector3(0, 0, 20);
            rend.startColor = Color.blue;
            rend.endColor = Color.blue;
            btn = null;
            slider = null;
            dropdown = null;
        }
        rend.SetPositions(points);
        rend.material.color = rend.startColor;
        return hitBtn;
    }
    public void OnClick()
    {
        if (btn != null && Time.time - lastClickTime > clickCooldown)
        {
            lastClickTime = Time.time;
            if (btn.name == "Start")
            {
                start.gameObject.SetActive(false);
                settingsButton.SetActive(false);
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
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("mainMenu");
                Destroy(player);
            }
            else if(btn.name == "Resume"){
                rend.enabled = false;
                rend2.enabled = false;
                if(pauseMenuScript != null)
                {
                    pauseMenuScript.paused = false;
                    pauseMenuScript.Resume();
                    pauseMenuScript.flag = false;
                }
            }
            else if(btn.name == "map1")
            {
                map1.gameObject.SetActive(false);
                demoMap.gameObject.SetActive(false);
                nightmode.SetActive(true);
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
                Destroy(player);
                SceneManager.LoadScene("tutorial");
            }
        }
    }
    private void Start() // Set up the initial line
    {
        img = panel.GetComponent<Image>();
        rend = gameObject.GetComponent<LineRenderer>();
        points = new Vector3[2];
        points[0] = Vector3.zero;
        points[1] = transform.position + new Vector3(0, 0, 20);
        rend.SetPositions(points);
        if(menu)
        {
            rend.enabled = true;
        }
        else
        {
            rend.enabled = false;
        }
    }
    private void Update()
    {
        if (Submit == null)
        {
            Debug.LogError("Submit is null");
            return;
        }
        bool submitValue = Submit.GetState(SteamVR_Input_Sources.Any);
        AlignLineRenderer(rend);
        if (AlignLineRenderer(rend) && submitValue)
        {
            if(btn != null)
            {
                btn.onClick.Invoke();
            }
            else if(slider != null)
            {
                UpdateSliderValue();
            }
            else if(dropdown != null)
            {
                HandleDropdownInteraction();
            }
        }
        if (pauseMenuScript != null && pauseMenuScript.paused == true){
            rend.enabled = true;
        }
    }

    private void UpdateSliderValue()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            RectTransform sliderRect = slider.GetComponent<RectTransform>();
            Vector3 localHitPoint = sliderRect.InverseTransformPoint(hit.point);
            float normalizedValue = Mathf.Clamp01((localHitPoint.x - sliderRect.rect.xMin) / sliderRect.rect.width);
            slider.value = normalizedValue * slider.maxValue;
        }
    }
    
    private int dropdownIndex = 0;
    private bool isDropdownOpen = false;
    public SteamVR_Action_Vector2 moveValue;
    private float scrollCooldown = 0.2f;
    private float lastScrollTime = 0f;
    private void HandleDropdownInteraction()
    {

        if(!dropdown.gameObject.activeSelf)
        {
            dropdown.Show();
            dropdownIndex = dropdown.value;
            isDropdownOpen = true;
        }
        else
        {
            float verticalInput = moveValue.axis.y;
            if(Time.time - lastScrollTime > scrollCooldown)
            {
                if(verticalInput > 0.5f)
                {
                    dropdownIndex = Mathf.Max(0,dropdownIndex-1);
                    lastScrollTime = Time.time;
                }
                else if(verticalInput < -0.5f)
                {
                    dropdownIndex = Mathf.Min(dropdown.options.Count -1, dropdownIndex + 1);
                    lastScrollTime = Time.time;
                }
                dropdown.value = dropdownIndex;
            }
            if(Submit.GetStateDown(SteamVR_Input_Sources.Any))
            {
                dropdown.value = dropdownIndex;
                dropdown.Hide();
                isDropdownOpen = false;
            }
        }
    }
}
