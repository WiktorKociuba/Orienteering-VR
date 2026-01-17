using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class loadSceneVR : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBar;
    public GameObject fadeToBlack;
    public destroyPlayer destroyPla;
    public GameObject cam;
    public IEnumerator loadSceneAsync(string scene, int route = -1){
        if(destroyPla != null){
            destroyPla.destroyVR();
            cam.SetActive(true);
        }
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
