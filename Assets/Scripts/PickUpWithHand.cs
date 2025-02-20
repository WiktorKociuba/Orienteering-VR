using UnityEngine;
using Valve.VR;

public class PickUpWithHand : MonoBehaviour
{
    public float distToPick;
    bool handClosed = false;
    public LayerMask pickupLayer;
    
    public SteamVR_Input_Sources handSource;

    Rigidbody holdingObject;

    private Quaternion grabOffset;

    private void FixedUpdate()
    {
        handClosed = SteamVR_Actions.default_GrabGrip.GetState(handSource) ? true : false;

        if (!handClosed)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, distToPick, pickupLayer);
            holdingObject = colliders.Length > 0 ? colliders[0].transform.root.GetComponent<Rigidbody>() : null;
            if (holdingObject)
            {
                // Store initial rotation offset when grabbing
                grabOffset = Quaternion.Inverse(transform.rotation) * holdingObject.transform.rotation;
            }
        }
        else
        {
            holdingObject.linearVelocity = (transform.position - holdingObject.transform.position) / Time.fixedDeltaTime;
            
            holdingObject.maxAngularVelocity = 20;
            Quaternion targetRotation = transform.rotation * grabOffset;
            Quaternion deltaRot = targetRotation * Quaternion.Inverse(holdingObject.transform.rotation);
            Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
            eulerRot *= 0.95f;
            eulerRot *= Mathf.Deg2Rad;
            holdingObject.angularVelocity = eulerRot / Time.fixedDeltaTime;
        }
    }
}
