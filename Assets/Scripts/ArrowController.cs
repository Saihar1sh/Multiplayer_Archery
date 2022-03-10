using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private Rigidbody arrowRb;
    private float initVel, currentVel;

    // Start is called before the first frame update
    void Start()
    {
        arrowRb = GetComponent<Rigidbody>();
        arrowRb.useGravity = false;

    }

    public void SetKinematicMode(bool val)
    {
        arrowRb.isKinematic = val;
    }

    public void SetInitVelocity(Vector3 vel)
    {
        //initVel = vel;
        arrowRb.velocity =  vel;
        arrowRb.useGravity = true;
    }
}
