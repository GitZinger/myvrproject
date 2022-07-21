using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour
{

    public Hand leftHand;
    public Hand rightHand;
    public Transform head;
    public Transform myavatar;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    Vector3 getWorldFootPosition()
    {
        Vector3 headPosWorld = head.position;
        Vector3 headPosPlay = myavatar.worldToLocalMatrix.MultiplyPoint(headPosWorld);
        Vector3 footPosPlay = headPosPlay;
        footPosPlay.y = 0;
        Vector3 footPosWorld = myavatar.localToWorldMatrix.MultiplyPoint(footPosPlay);

        return footPosWorld;
    }


    public void rotate(float angleDegrees)
    {
        Vector3 saveFootPos = getWorldFootPosition();
        myavatar.Rotate(0, angleDegrees, 0, Space.World);


    }


    // Update is called once per frame
    void Update()
    {
        float ls = leftHand.getSpeedControl();
        float rs = rightHand.getSpeedControl();



        float s = Mathf.Clamp(ls + rs, -1, 1);
        Vector3 intendedMotion = s * head.forward * Time.deltaTime;
        intendedMotion.y = 0;
        myavatar.Translate(intendedMotion, Space.World);//move the body


    }
}
