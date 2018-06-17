using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{

    float moveX, moveY, moveX_Target, moveY_Target, viewX, viewY;

    Animator am;
    Rigidbody rb;

    [SerializeField]
    Camera cam;

    [SerializeField]
    GameObject NeckBone, JawBone;

    [SerializeField]
    bool isAirbone = false;

    [SerializeField]
    float JumpForce = 6f;

    [SerializeField]
    float AirboneRaycastDistance = 1f;

    [Header("Camera")]
    [SerializeField]
    float Sensitivity = 10;
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
        isAirbone = AirboneCheckRaycast(AirboneRaycastDistance);

        if (isLocalPlayer)
        {
            moveX_Target = Input.GetAxis("Horizontal");
            moveY_Target = Input.GetAxis("Vertical");

            if ((viewX + Input.GetAxis("Mouse X") * Sensitivity < ClampAngleX) &&
               (viewX + Input.GetAxis("Mouse X") * Sensitivity > -ClampAngleX) &&
               (viewY + Input.GetAxis("Mouse Y") * Sensitivity < ClampAngleY) &&
               (viewY + Input.GetAxis("Mouse Y") * Sensitivity > -ClampAngleY))
            {
                viewX += Input.GetAxis("Mouse X") * Sensitivity;
                viewY += Input.GetAxis("Mouse Y") * Sensitivity;
            }

            if (Input.GetKey(KeyCode.LeftShift))
                moveY_Target += .5f;

            if (Input.GetKeyDown(KeyCode.Space) && !isAirbone)
            {
                isAirbone = true;

                am.applyRootMotion = false;

                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            }

            if (isAirbone)
                am.applyRootMotion = false;
            else
                am.applyRootMotion = true;

            //if ((x == 0 && y == 0 && !Input.GetKey(KeyCode.LeftAlt)) && (vx > ClampAngleX - 15 || vx < -ClampAngleX + 15))
            //{
            //    am.Play("Grounded_Turn");
            //
            //    am.SetFloat("X", vx / ClampAngleX);
            //   am.SetFloat("Y", y);
            //}
            //else
            //{

            StartCoroutine("LerpAxles");

            if (!Input.GetKey(KeyCode.LeftAlt))
                transform.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * Sensitivity, 0);

            am.SetFloat("X", moveX);
            am.SetFloat("Y", moveY);
            am.SetBool("Airbone", isAirbone);
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

        JawBone.transform.localEulerAngles = new Vector3(0, 0, -48.694f);
    }

    [Command]
    void CmdApplyHeadRotation(Vector3 _transform)
    {
        NeckBone.transform.localEulerAngles = _transform;
    }

    IEnumerator LerpAxles()
    {
        if (moveY < moveY_Target)
        {
            for (float i = moveY; i <= moveY_Target; i += 0.05f)
            {
                moveY = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (moveY > moveY_Target)
        {
            for (float i = moveY; i >= moveY_Target; i -= 0.05f)
            {
                moveY = i;

                yield return new WaitForSeconds(0.01f);
            }
        }

        if (moveX < moveX_Target)
        {
            for (float i = moveX; i <= moveX_Target; i += 0.05f)
            {
                moveX = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (moveX > moveX_Target)
        {
            for (float i = moveX; i >= moveX_Target; i -= 0.05f)
            {
                moveX = i;

                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    bool AirboneCheckRaycast(float distance)
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, distance))
            return false;
        else
            return true;
    }
}