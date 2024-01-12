using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = LevelManager.activePlayer.transform;
        if (playerTransform != null)
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        playerTransform = LevelManager.activePlayer.transform;
        if (playerTransform != null)
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
    }
}
