using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float velocity = 5;
    public bool useGravity = true;
    public bool moveController = true;
    public float gravity = -9.81f;

    private Vector3 _input;
    private Vector3 _move;
    private Vector3 _gravity;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (moveController)
        {
            _input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"), Input.GetAxis("Vertical"));

            _move = transform.forward * _input.z + transform.right * _input.x;
            _move *= velocity;
        }

        if (useGravity)
        {
            _gravity.y += Time.deltaTime * gravity;
            _gravity.y = (_gravity.y < 0 && _controller.isGrounded) ? 0 : _gravity.y;
        }

        if (moveController || useGravity)
        {
            _controller.Move((_move + _gravity) * Time.deltaTime);
        }
    }

}
