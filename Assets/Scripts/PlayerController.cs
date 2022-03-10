using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;

    private ArrowController arrow;

    private Camera cam;

    [SerializeField]
    private float verSensitivity, horSensitivity;

    [SerializeField]
    private ArrowController arrowPrefab;

    [SerializeField]
    private RPC rPC;

    public Transform shootPoint, bow;

    public float arrowForce;

    public static bool canInput = false;

    public AimTrailPath aimTrail;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        aimTrail.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        CheckForCameraMovement();

        Debug.Log("lucif can input: " + canInput);

        if (canInput)
        {
            ArrowInputs();

        }
    }

    private void ArrowInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            aimTrail.gameObject.SetActive(true);
            arrow = Instantiate(arrowPrefab, bow);
        }
        else if (Input.GetMouseButton(0))
        {
            arrowForce = Mathf.Clamp(arrowForce + Time.deltaTime * 10, 0, 30);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            arrow.SetInitVelocity(shootPoint.forward * arrowForce);
            arrow.transform.parent = null;
            rPC.MakeTurn(1);
            arrowForce = 0;
            aimTrail.gameObject.SetActive(false);

        }
    }

    private void CheckForCameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X"); //get x input
        float mouseY = Input.GetAxisRaw("Mouse Y"); //get y input

        Vector3 rotateX = new Vector3(mouseY * verSensitivity, 0, 0); //calculate the x rotation based on the y input
        Vector3 rotateY = new Vector3(0, mouseX * horSensitivity, 0); //calculate the y rotation based on the x input

        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateY)); //rotate rigid body
        cam.transform.Rotate(-rotateX); //rotate the camera negative to the x rotation (so the movement isn't inversed)
    }
}
