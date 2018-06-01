using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float x, y, vx, vy;

    Animator am;
    Rigidbody rb;

    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject NeckBone;

    [Header("Camera")]
    [SerializeField]
    float Sens = 10;
    [SerializeField]
    float ClampAngleX = 90, ClampAngleY = 90;

    [SerializeField]
    bool Airbone = false;

    [SerializeField]
    float JumpForce = 10;

    private void Start()
    {
        am = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");

            if ((vx + Input.GetAxis("Mouse X") * Sens < ClampAngleX) &&
               (vx + Input.GetAxis("Mouse X") * Sens > -ClampAngleX) &&
               (vy + Input.GetAxis("Mouse Y") * Sens < ClampAngleY) &&
               (vy + Input.GetAxis("Mouse Y") * Sens > -ClampAngleY))
            {
                vx += Input.GetAxis("Mouse X") * Sens;
                vy += Input.GetAxis("Mouse Y") * Sens;
            }

            if (Input.GetKey(KeyCode.LeftShift))
                y += .5f;

            if (Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * JumpForce);
            }

            if (Airbone)
                am.applyRootMotion = false;
            else
                am.applyRootMotion = true;

            if ((vx > ClampAngleX - 10 || vx < -ClampAngleX + 10) && !Input.GetKey(KeyCode.LeftAlt))
            {
                am.Play("Grounded_Turn");

                am.SetFloat("X", vx / ClampAngleX);
                am.SetFloat("Y", y);
            }
            else
            {
                am.Play("Grounded");


                if (!Input.GetKey(KeyCode.LeftAlt))
                    transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X"), 0);

                am.SetFloat("X", x);
                am.SetFloat("Y", y);
            }
        }
        else
        {
            /*foreach (GameObject GO in ComponentsToDisable)
            {
                GO.SetActive(false);
            }*/

            cam.enabled = false;
        }


        cam.transform.localEulerAngles = new Vector3(-vy, vx, 0);
    }

    private void LateUpdate()
    {
        if (isLocalPlayer && Input.GetKey(KeyCode.LeftAlt))
            CmdApplyHeadRotation(new Vector3(-vy, vx, 0));
    }

    [Command]
    void CmdApplyHeadRotation (Vector3 _transform)
    {
        NeckBone.transform.localEulerAngles = _transform;
    }
}
