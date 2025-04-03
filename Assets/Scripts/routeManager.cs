using UnityEngine;

public class routeManager
{
    public static int SelectedRouteIndex {get; private set; } = -1;
    public static bool SelectedTime {get; private set; } = false;
    public static float renderDistance {get; private set; } = 300f;
    public static void SelectRoute(int routeIndex)
    {
        SelectedRouteIndex = routeIndex;
        Debug.Log("Route " + SelectedRouteIndex);
    }
    public static void SelectDay()
    {
        SelectedTime = false;
    }
    public static void SelectNight()
    {
        SelectedTime = true;
    }
    public static void SelectRendDist(float rendDist)
    {
        renderDistance = rendDist;
    }
}
