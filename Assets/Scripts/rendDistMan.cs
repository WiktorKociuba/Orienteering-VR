using UnityEngine;
using UnityEngine.UI;

public class rendDistMan : MonoBehaviour
{
    public Slider renderDistanceSlider;
    public float renderDistance = 300f;
    void Start()
    {
        if (renderDistanceSlider != null)
        {
            renderDistanceSlider.value = renderDistance; 
        }
        Camera.main.farClipPlane = renderDistance;
        renderDistanceSlider.onValueChanged.AddListener(UpdateRenderDistance);
    }
    public void UpdateRenderDistance(float value)
    {
        renderDistance = value;
        Camera.main.farClipPlane = renderDistance;
        routeManager.SelectRendDist(value);
    }
}
