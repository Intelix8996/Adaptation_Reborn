using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    float moveX, moveY, moveX_Target, moveY_Target, viewX, viewY;

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

    private void Start()
    {
        am = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            moveX_Target = Input.GetAxis("Horizontal");
            moveY_Target = Input.GetAxis("Vertical");

            if ((viewX + Input.GetAxis("Mouse X") * Sens < ClampAngleX) &&
               (viewX + Input.GetAxis("Mouse X") * Sens > -ClampAngleX) &&
               (viewY + Input.GetAxis("Mouse Y") * Sens < ClampAngleY) &&
               (viewY + Input.GetAxis("Mouse Y") * Sens > -ClampAngleY))
            {
                viewX += Input.GetAxis("Mouse X") * Sens;
                viewY += Input.GetAxis("Mouse Y") * Sens;
            }

            if (Input.GetKey(KeyCode.LeftShift))
                moveY_Target += .5f;

            //if ((x == 0 && y == 0 && !Input.GetKey(KeyCode.LeftAlt)) && (vx > ClampAngleX - 15 || vx < -ClampAngleX + 15))
            //{
            //    am.Play("Grounded_Turn");
            //
            //    am.SetFloat("X", vx / ClampAngleX);
            //   am.SetFloat("Y", y);
            //}
            //else
            //{
            am.Play("Grounded");

            StartCoroutine("Lerp");

            if (!Input.GetKey(KeyCode.LeftAlt))
                transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * Sens, 0);

            am.SetFloat("X", moveX);
            am.SetFloat("Y", moveY);
            //}
        }
        else
        {
            /*foreach (GameObject GO in ComponentsToDisable)
            {
                GO.SetActive(false);
            }*/

            cam.enabled = false;
        }


        cam.transform.localEulerAngles = new Vector3(-viewY, 0, 0);
    }

    private void LateUpdate()
    {
        if (isLocalPlayer && Input.GetKey(KeyCode.LeftAlt))
            CmdApplyHeadRotation(new Vector3(-viewY, viewX, 0));
    }

    [Command]
    void CmdApplyHeadRotation (Vector3 _transform)
    {
        NeckBone.transform.localEulerAngles = _transform;
    }

    IEnumerator Lerp()
    {
        if (moveY < moveY_Target)
        {
            for (float i = moveY; i < moveY_Target; i += 0.05f)
            {
                moveY = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (moveY > moveY_Target)
        {
            for (float i = moveY; i > moveY_Target; i -= 0.05f)
            {
                moveY = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
