using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour {

    [SerializeField]
    Transform Origin;

    [SerializeField]
    Vector3 Offset;

    void FixedUpdate () {
        transform.position = Origin.position + Offset;
	}
}
