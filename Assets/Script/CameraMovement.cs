using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed = 10.0f;
    public Vector3 dist;
    public Transform lookTarget;

    private void Awake()
    {
        //_camera = this.transform;

        //_offset = _camera.position - _player.position;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        //_camera.DOMoveX(_player.position.x + _offset.x, _speed * Time.deltaTime);
        //_camera.DOMoveZ(_player.position.z + _offset.z, _speed * Time.deltaTime);

        Vector3 dPos = cameraTarget.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, sSpeed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }


}
