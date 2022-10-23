using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothInstance : MonoBehaviour
{
    public static readonly float speed = 10;
    private Vector3 _rotVelocity;
    private Vector3 _posVelocity;
    public Vector3 position;
    public Vector3 rotation;

    private void Start()
    {
        position = transform.position;
        rotation = transform.eulerAngles;
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, position, ref _posVelocity, speed * Time.deltaTime);
        transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, rotation, ref _rotVelocity, speed * Time.deltaTime);
    }
}
