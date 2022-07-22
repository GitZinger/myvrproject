using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

[RequireComponent(typeof(Rigidbody))]

public class Grabbable : MonoBehaviour
{
    //the object needs to be grabbed must have this script and rigidbody
    protected Transform follow; //follow might be our hands
    protected Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = Mathf.Infinity;
        transform.GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
    public virtual void handleTrigger(float v)
    {
        GetComponent<AudioSource>().Play();
    }

    public virtual void grab(Transform by)
    {
        rb.useGravity = false;
        follow = by;
    }

    public virtual void release()
    {
        rb.useGravity = true;
        follow = null;
    }
    private void FixedUpdate()
    {
        if (follow != null)
        {
            Vector3 between = follow.position - rb.position;
            rb.velocity = between / Time.deltaTime;

            Quaternion betweenRot = follow.rotation * Quaternion.Inverse(rb.rotation);
            float angle;
            Vector3 axis;
            betweenRot.ToAngleAxis(out angle, out axis);
            Vector3 av = axis * Mathf.Deg2Rad * angle;
            rb.angularVelocity = av / Time.deltaTime;
        }
    }
 
}
