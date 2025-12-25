using UnityEngine;
using Valve.VR;

public class PickUpWithHand : MonoBehaviour
{
    public float distToPick;
    public bool handClosed = false;
    public LayerMask pickupLayer;
    
    public SteamVR_Input_Sources handSource;

    public Rigidbody holdingObject;

    private Quaternion grabOffset;
    private Vector3 grabPositionOffset;

    private void FixedUpdate()
    {
        handClosed = SteamVR_Actions.default_GrabGrip.GetState(handSource) ? true : false;
        if (!handClosed)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, distToPick, pickupLayer);
            holdingObject = null;
            float closestDistance = float.MaxValue;
            foreach(Collider collider in colliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    holdingObject = collider.GetComponentInParent<Rigidbody>();
                }
            }
            if (holdingObject)
            {
                grabOffset = Quaternion.Inverse(transform.rotation) * holdingObject.transform.rotation;
                grabPositionOffset = holdingObject.transform.position - transform.position;
            }
        }
        else if(handClosed && holdingObject != null)
        {
            if(!holdingObject.isKinematic)
            {
                Vector3 targetPosition = transform.position + grabPositionOffset;
                holdingObject.linearVelocity = (targetPosition - holdingObject.transform.position) / Time.fixedDeltaTime;
                
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
}
