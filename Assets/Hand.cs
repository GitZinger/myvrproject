using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class Hand : MonoBehaviour
{
    public enum HAND_SIDE { LEFT, RIGHT }

    public HAND_SIDE myside;

    public float snapRotateAmountDegrees = 25;
    OVRInput.Controller myHand;
    public GameObject handcube;
    public Avatar myavatar;
    bool canActivateSnap = true;

    void Awake()
    {
      
      
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
    public float getSpeedControl()
    {
        return OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, myHand).y;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, myHand);


        if (canActivateSnap && Mathf.Abs(thumbstick.x) > .5f)
        {
            myavatar.rotate(snapRotateAmountDegrees * Mathf.Sign(thumbstick.x));
            canActivateSnap = false;
        }
        else if (!canActivateSnap && Mathf.Abs(thumbstick.x) <= .5f)
        {
            canActivateSnap = true;
        }
    }
}
