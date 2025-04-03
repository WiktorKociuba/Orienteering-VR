using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    public Vector3 initialPosition;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position;
    }
    
    void Update()
    {
        if(mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            Vector3 direction = cameraPosition - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(-direction);
        }

        Vector3 newPosition = initialPosition;
        newPosition.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = newPosition;
    }
}
