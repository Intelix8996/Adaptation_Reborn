using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float x, y, vx, vy;

    Animator am;

    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject NeckBone;

    [Header("Camera")]
    [SerializeField]
    float Sens = 10;
    [SerializeField]
    float ClampAngleX = 90, ClampAngleY = 90;


    private void Start()
    {
        am = GetComponent<Animator>();
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

            am.SetFloat("X", x);
            am.SetFloat("Y", y);
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
        NeckBone.transform.localEulerAngles = cam.transform.localEulerAngles;
    }
}
