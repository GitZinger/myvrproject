using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Hand : MonoBehaviour
{


    public Transform follow;
    Rigidbody rb;

    public Grabbable grabbed = null;

    public enum HAND_SIDE { LEFT, RIGHT }

    public HAND_SIDE myside;
    public float gripAtPercentage;
    public float releaseAtPercentage;
    public float snapRotateAmountDegrees = 25;
    OVRInput.Controller myHand;
    public GameObject handcube;
    public Avatar myavatar;
    bool canActivateSnap = true;

    void Awake()
    {

        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;

        if (myside == HAND_SIDE.LEFT)
        {
            handcube.GetComponent<Renderer>().material.color = Color.red;
            myHand = OVRInput.Controller.LTouch;
        }
        else
        {
            handcube.GetComponent<Renderer>().material.color = Color.blue;
            myHand = OVRInput.Controller.RTouch;
        }

    }
    public float getSpeedControl2()
    {
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, myHand).x;
    }
    public float getSpeedControl()
    {
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, myHand).y;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, myHand);

        Rigidbody otherRB = other.attachedRigidbody;
        if (otherRB == null)
        {
            return;
        }

        Grabbable g = otherRB.GetComponent<Grabbable>();

        if (g == null) { return; } //we know it's a grabbable

        if (handTrigger > gripAtPercentage && grabbed == null)
        {
            grabbed = g;
           
            g.grab(this.follow);

        }
    
    }

    private void FixedUpdate()
    {
        /*
        Vector3 between = follow.position - rb.position;
        rb.velocity = between / Time.deltaTime;

        Quaternion betweenRot = follow.rotation * Quaternion.Inverse(rb.rotation);
        float angle;
        Vector3 axis;
        betweenRot.ToAngleAxis(out angle, out axis);
        Vector3 av = axis * Mathf.Deg2Rad * angle;
        rb.angularVelocity = av / Time.deltaTime;
        */
        transform.position = Vector3.Lerp(transform.position, follow.position, 0.9f);
        transform.rotation = Quaternion.Slerp(transform.rotation, follow.rotation, 0.9f);


    }

    // Update is called once per frame
    void Update()
    {

        float handTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, myHand);
        if (handTrigger < releaseAtPercentage && grabbed != null)
        {
            grabbed.release();
            grabbed = null;
            
        }
        float indexTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, myHand);
         
        if (grabbed != null&&indexTrigger>0.5f)
        {
            grabbed.GetComponent<AudioSource>().Play();
            grabbed.handleTrigger(indexTrigger);
        }

        if (myside == HAND_SIDE.RIGHT)
        {

            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, myHand);

            if (Mathf.Abs(thumbstick.x) > .5f)
            {


                myavatar.transform.eulerAngles = Vector3.Lerp(myavatar.transform.eulerAngles,
                    new Vector3(myavatar.transform.eulerAngles.x, myavatar.transform.eulerAngles.y + snapRotateAmountDegrees * Mathf.Sign(thumbstick.x),
                    myavatar.transform.eulerAngles.z), 0.5f);
            }
            /*
                if (canActivateSnap && Mathf.Abs(thumbstick.x) > .5f)
            {
                myavatar.rotate(snapRotateAmountDegrees * Mathf.Sign(thumbstick.x));
                canActivateSnap = false;
            }
            else if (!canActivateSnap && Mathf.Abs(thumbstick.x) <= .5f)
            {
                canActivateSnap = true;
            }*/

        }
    }
}
