using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : NodeObject
{
    // Start is called before the first frame update
    public override void Start()
    {
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position).currentObject = gameObject;
        FindObjectOfType<C_Grid>().NodeFromWorldPoint(transform.position + new Vector3(0,FindObjectOfType<C_Grid>().nodeRadius * 2)).currentObject = gameObject;
    }
}
