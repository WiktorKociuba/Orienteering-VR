using UnityEngine;

public class doNotRotate : MonoBehaviour
{
    public Transform parent;
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation(new Vector3(0, parent.eulerAngles.y, parent.eulerAngles.z));
    }
}
