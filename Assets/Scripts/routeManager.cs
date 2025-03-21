using UnityEngine;

public class routeManager
{
    public static int SelectedRouteIndex {get; private set; } = -1;
    public static void SelectRoute(int routeIndex)
    {
        SelectedRouteIndex = routeIndex;
        Debug.Log("Route " + SelectedRouteIndex);
    }
}
