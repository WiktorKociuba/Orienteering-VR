using UnityEngine;
using System.Collections;

public class enableRoute : MonoBehaviour
{
    public GameObject[] routes;
    public GameObject grass;
    public Material Day;
    public Material Night;
    public GameObject spotLight1;
    public GameObject spotLight2;
    public Light directionalLight;
    public SO_GrassSettings grassSettings;
    public GameObject loadingScreen;
    public GameObject fadeToBlack;
    public GameObject loadCam;
    void Start()
    {
        StartCoroutine(onLoad());
    }
    IEnumerator onLoad()
    {
        if(loadingScreen !=null)
            loadingScreen.SetActive(true);
        if(fadeToBlack != null)
            fadeToBlack.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        if(loadingScreen != null)
            loadingScreen.SetActive(false);
        routes[routeManager.SelectedRouteIndex].SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        if(loadCam != null)
            loadCam.SetActive(false);
        if(fadeToBlack != null)
            fadeToBlack.SetActive(false);
        if (!routeManager.SelectedTime)
        {
            RenderSettings.skybox = Day;
            spotLight1.SetActive(false);
            spotLight2.SetActive(false);
            grassSettings.minFadeDistance = 40f;
            grassSettings.maxDrawDistance = 125f;
            grass.SetActive(false);
            grass.SetActive(true);
        }
        else
        {
            if (routeManager.SelectedRouteIndex == 0)
            {
                spotLight2.SetActive(false);
                spotLight1.SetActive(true);
            }
            else
            {
                spotLight1.SetActive(false);
                spotLight2.SetActive(true);   
            }
            routeManager.SelectDay();
            RenderSettings.skybox = Night;
            directionalLight.intensity = 0.2f;
            RenderSettings.fogColor = Color.black;
            RenderSettings.fogDensity = 0.1f;
            grassSettings.minFadeDistance = 10f;
            grassSettings.maxDrawDistance = 25f;
            grass.SetActive(false);
            grass.SetActive(true);
        }
        grass.SetActive(false);
        grass.SetActive(true);
        Camera.main.farClipPlane = routeManager.renderDistance;
    }
}
