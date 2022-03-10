using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTrailPath : MonoBehaviour
{
    private PlayerController player;
    private LineRenderer lineRenderer;

    // Number of points on the line
    public int numPoints = 50;

    // distance between those points on the line
    public float timeBetweenPoints = 0.1f;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        lineRenderer.positionCount = (int)numPoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPos = player.shootPoint.position;
        Vector3 startingVel = player.shootPoint.forward * player.arrowForce;
        for (float q = 0; q < numPoints; q += timeBetweenPoints)
        {
            Vector3 newPoint = startingPos + q * startingVel;
            newPoint.y = startingPos.y + startingVel.y * q + Physics.gravity.y / 2f * q * q;
            points.Add(newPoint);

/*            if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
*/        }

        lineRenderer.SetPositions(points.ToArray());
    }
}
