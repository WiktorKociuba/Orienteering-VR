using UnityEngine;

public class doNotRotate : MonoBehaviour
{
    public Transform parent;
    private void FixedUpdate()
    {
        Vector3 northDirection = new Vector3(0, 0, 1);
        Vector3 parentForward = new Vector3(parent.forward.x, 0, parent.forward.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(northDirection, Vector3.up);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
