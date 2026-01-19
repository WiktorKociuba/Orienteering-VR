using UnityEngine;

public class checkIfPickedUp : MonoBehaviour
{
    public PickUpWithHand leftHand;
    public PickUpWithHand rightHand;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        bool isHeldByLeftHand = leftHand.handClosed && leftHand.holdingObject != null && leftHand.holdingObject.name == gameObject.name;
        bool isHeldByRightHand = rightHand.handClosed && rightHand.holdingObject != null && rightHand.holdingObject.name == gameObject.name;
        if(!isHeldByLeftHand && !isHeldByRightHand)
        {
            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;
        }
    }
}