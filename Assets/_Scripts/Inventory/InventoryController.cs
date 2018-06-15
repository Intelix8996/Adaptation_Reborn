using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour {

    [SerializeField]
    GameObject Backpack;

    [SerializeField]
    Vector3 BackpackOffset;

    [SerializeField]
    Vector3 BackpackOffsetRot;

    GameObject BackpackObj;

    bool isInventoryOpened = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpened = !isInventoryOpened;

            if (isInventoryOpened)
                Close();
            else
                Open();

        }
    }

    void Open()
    {
        BackpackObj = Instantiate(Backpack, transform.position + BackpackOffset, Quaternion.identity) as GameObject;

        BackpackObj.transform.localEulerAngles = BackpackOffsetRot;

        BackpackObj.transform.parent = transform;

        GetComponent<PlayerController>().enabled = false;
    }

    void Close()
    {
        Destroy(BackpackObj);

        GetComponent<PlayerController>().enabled = true;
    }
}
