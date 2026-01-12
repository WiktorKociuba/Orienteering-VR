using UnityEngine;
using System.Collections;
using UnityEditor.UI;

public class loadingScreenManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(onSceneLoad());
    }
    public GameObject loadingScreen;
    public GameObject fadeToBlack;
    public GameObject cam;
    public GameObject playerCam;
    IEnumerator onSceneLoad(){
        if(loadingScreen !=null)
            loadingScreen.SetActive(true);
        yield return null;
        fadeToBlack.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        if(playerCam!=null)
            playerCam.SetActive(true);
        if(loadingScreen != null)
            loadingScreen.SetActive(false);
        if(cam != null)
            cam.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        fadeToBlack.SetActive(false);
    }
}
