using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public float offsetZ = 5f;
    public float smoothing = 2f;

    //player transform component
    Transform playerPos;
   

    private void Awake()
    {
        playerPos = FindObjectOfType<PlayerMovement2>().transform;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        //Position the camera should be in
        Vector3 targetPos = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z - offsetZ);

        //Set the position accordingly
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }


}
