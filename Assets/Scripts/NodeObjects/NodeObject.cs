using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Start()
    {
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = gameObject;
    }
}
