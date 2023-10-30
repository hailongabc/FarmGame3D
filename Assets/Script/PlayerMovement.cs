using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    [SerializeField]
    private Transform cameraTranform;

    private Vector3 _moveVector;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

        Vector3 movementDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        movementDirection = Quaternion.AngleAxis(cameraTranform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        if(_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            _animator.SetBool("isRunning", true);
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
            //_animator.Play("Run");
        }
        else
        {
            _animator.SetBool("isRunning", false);
            //_animator.Play("breathingIdle");
        }
    }

    private void Move2()
    {

    }

    private void OnApplicationFocus(bool focus)
    {
        //if (focus)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
        //else
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //}
    }
}
