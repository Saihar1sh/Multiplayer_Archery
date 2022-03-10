using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField]
    private RPC rPC;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ArrowController>())
        {
            collision.gameObject.GetComponent<ArrowController>().SetKinematicMode(true);
            rPC.LocalPlayerHitTarget(5);
        }
    }
}
