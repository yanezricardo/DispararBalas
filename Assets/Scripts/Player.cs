using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera = null;
    [SerializeField] private bool _lookCursor = true;
    [SerializeField, Range(1, 10)] private float _mouseSensitivity = 3.5f;
    [SerializeField, Range(10, 90)] private float _upViewAngle = 45.0f;
    [SerializeField, Range(10, 90)] private float _downViewAngle = 45.0f;
    [SerializeField, Range(0, 0.5f)] private float _moveSmoothTime = 0.3f;
    [SerializeField, Range(0, 0.5f)] private float _mouseSmoothTime = 0.03f;
    [SerializeField] private float _walkSpeed = 6.0f;
    [SerializeField] private float _gravity = -13.0f;
    [SerializeField] private float _jumpHeight = 5.0f;

    private float _cameraPitch = 0.0f;
    private float _velocityY = 0.0f;
    private CharacterController _characterController;
    private Vector2 _currentMovementDirection = Vector2.zero;
    private Vector2 _currentMovementVelocity = Vector2.zero;
    private Vector2 _currentMouseDelta = Vector2.zero;
    private Vector2 _currentMouseDeltaVelocity = Vector2.zero;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (_playerCamera == null)
            _playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Start()
    {
        if (_lookCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDirection.Normalize();

        _currentMovementDirection = Vector2.SmoothDamp(_currentMovementDirection, targetDirection, ref _currentMovementVelocity, _moveSmoothTime);

        if (_characterController.isGrounded)
        {
            _velocityY = 0.0f;
            if (Input.GetButtonDown("Jump"))
            {
                _velocityY += _jumpHeight;
            }
        }

        _velocityY += _gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * _currentMovementDirection.y + transform.right * _currentMovementDirection.x) * _walkSpeed + Vector3.up * _velocityY;
        _characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _currentMouseDelta = Vector2.SmoothDamp(_currentMouseDelta, targetMouseDelta, ref _currentMouseDeltaVelocity, _mouseSmoothTime);

        _cameraPitch -= _currentMouseDelta.y * _mouseSensitivity;
        _cameraPitch = Mathf.Clamp(_cameraPitch, _upViewAngle * -1, _downViewAngle);
        _playerCamera.localEulerAngles = Vector3.right * _cameraPitch;
        transform.Rotate(Vector3.up * _currentMouseDelta.x * _mouseSensitivity);
    }
}
