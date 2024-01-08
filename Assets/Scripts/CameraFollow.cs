using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followObject;

    // Start is called before the first frame update
    void Start()
    {
        if (followObject != null)
            transform.position = new Vector3(followObject.position.x, followObject.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {   if (followObject != null)
            transform.position = new Vector3(followObject.position.x, followObject.position.y, transform.position.z);
    }
}
