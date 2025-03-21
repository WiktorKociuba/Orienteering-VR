using UnityEngine;

public class enableRoute : MonoBehaviour
{
    public GameObject[] routes;
    public GameObject grass;
    void Start()
    {
        routes[routeManager.SelectedRouteIndex].SetActive(true);
        grass.SetActive(true);
    }
}
