using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    float x, y, vx, vy;

    Animator am;

    [SerializeField]
    Camera cam;

    [SerializeField]
    float Sens = 10;


    private void Start()
    {
        am = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        vx += Input.GetAxis("Mouse X") * Sens;
        vy += Input.GetAxis("Mouse Y") * Sens;

        if (Input.GetKey(KeyCode.LeftShift))
            y += .5f;
        if (Input.GetKey(KeyCode.LeftAlt))
            y -= .5f;

        cam.transform.localEulerAngles = new Vector3(-vy, vx, 0);

        am.SetFloat("X", x);
        am.SetFloat("Y", y);
    }
}
