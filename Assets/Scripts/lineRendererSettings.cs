using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.Newtonsoft.Json.Bson;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class lineRendererSettings : MonoBehaviour
{
    [SerializeField] LineRenderer rend;

    [Header("Settings")]
    public SteamVR_Action_Boolean Submit;
    public LayerMask layerMask;
    public GameObject panel;
    public Image img;
    public Image demoMapImg;
    public Button btn;
    public Button start;
    public Button exit;
    public Button demoMap;

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
            hitBtn = true;
            if(btn.name == "demoMap")
            {
                demoMapImg.gameObject.SetActive(true);
            }
        }
        else
        {
            points[1] = transform.forward + new Vector3(0, 0, 20);
            rend.startColor = Color.blue;
            rend.endColor = Color.blue;
            demoMapImg.gameObject.SetActive(false);
        }
        rend.SetPositions(points);
        rend.material.color = rend.startColor;
        return hitBtn;
    }
    public void OnClick() // Do smth when button is clicked
    {
        if (btn != null)
        {
            if (btn.name == "Start")
            {
                start.gameObject.SetActive(false);
                exit.gameObject.SetActive(false);
                demoMap.gameObject.SetActive(true);
            }
            else if (btn.name == "Exit")
            {
                Application.Quit();
            }
            else if(btn.name == "demoMap")
            {
                SceneManager.LoadScene("demoMap");
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
        rend.enabled = true;
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
            btn.onClick.Invoke();
        }
    }
}
