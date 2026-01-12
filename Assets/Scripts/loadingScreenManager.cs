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
    IEnumerator onSceneLoad(){
        if(loadingScreen !=null)
            loadingScreen.SetActive(true);
        fadeToBlack.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        if(loadingScreen != null)
            loadingScreen.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        fadeToBlack.SetActive(false);
    }
}
