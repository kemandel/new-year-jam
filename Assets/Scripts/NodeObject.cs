using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeObject : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = gameObject;
    }
}
