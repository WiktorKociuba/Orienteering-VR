using UnityEngine;

public class compassNeedle : MonoBehaviour
{
    public Transform needle;
    public Rigidbody parent;

    private Vector3 lastPos;
    private Vector3 rotation;
    void Start()
    {
        rotation = needle.eulerAngles;
        lastPos = new Vector3(-parent.rotation.eulerAngles.x, rotation.y, rotation.z);
    }
    void Update()
    {
        if((parent.rotation.eulerAngles.x > 90 && parent.rotation.eulerAngles.x < 280) || (parent.rotation.eulerAngles.z > 30 && parent.rotation.eulerAngles.z < 330))
        {
            needle.eulerAngles = parent.rotation.eulerAngles;
        }
        else
        {
            lastPos = new Vector3(-parent.rotation.eulerAngles.x, rotation.y, parent.rotation.eulerAngles.z);
            needle.eulerAngles = lastPos;
        }
    }
}
