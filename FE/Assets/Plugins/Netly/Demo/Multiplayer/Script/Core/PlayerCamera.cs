using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sens = 1;
    public float smoothTime = 5;
    public bool hidenCursorOnStart = true;
    public Transform myTarget;
    public Camera myCamera;
    public Vector3 myPosition = new Vector3(0, 1, -3);
    public LayerMask cameraLayerMask;
    
    public List<KeyCode> changeCursorState = new List<KeyCode>
    { 
        KeyCode.F1,
        KeyCode.Escape,
        KeyCode.Mouse1,
        KeyCode.LeftControl,
    };

    [Header("Camera angle limiter")]
    public float minAngleRotation = -89;
    public float maxAngleRotation = 89;


    [Header("Rotate player")]
    public bool rotatePlayer = true;
    public bool rotateVertical;
    public bool rotateHorizontal = true;
    public Transform playerTransform;

    private Transform _target;
    private Transform _pointer;
    private Quaternion _rotation;
    private Vector2 _input;
    private RaycastHit _hit;
    private Vector3 _currentSmooth;
    private CursorLockMode _cursorLock;

    private void Awake()
    {
        _target = new GameObject("Camera Target").transform;
        _target.parent = myTarget;

        _pointer = new GameObject("Camera Pointer").transform;
        _pointer.parent = _target;
        _pointer.localRotation = Quaternion.identity;

        myCamera.transform.parent = null;
    }

    private void Start()
    {
        if (hidenCursorOnStart)
        {
            Cursor.visible = false;
        }   
    }

    private void FixedUpdate()
    {
        UpdateCursorState();
    }

    private void Update()
    {       
        if (!Cursor.visible)
        {
            _input.x += Input.GetAxis("Mouse X") * sens;
            _input.y -= Input.GetAxis("Mouse Y") * sens;
        }        

        _input.y = Mathf.Clamp(_input.y, minAngleRotation, maxAngleRotation);
        _rotation = Quaternion.Euler(_input.y, _input.x, 0);

        _target.localPosition = Vector3.up * myPosition.y;
        _pointer.localPosition = Vector3.forward * myPosition.z;

        _target.rotation = _rotation;

        //rotate player
        if((rotatePlayer && playerTransform))
        {
            float _h = (rotateHorizontal) ? _input.x : playerTransform.eulerAngles.y;
            float _v = (rotateVertical) ? _input.y : playerTransform.eulerAngles.x;

            playerTransform.rotation = Quaternion.Euler(_v, _h, playerTransform.eulerAngles.z);
        }
    }

    private void LateUpdate()
    {
        Vector3 _position = _pointer.position;

        if (Physics.Linecast(_target.position, _pointer.position, out _hit, cameraLayerMask))
        {
            _position = _hit.point;
        }

        Debug.DrawLine(_target.position, _position, Color.green);

        myCamera.transform.position = Vector3.SmoothDamp(myCamera.transform.position, _position, ref _currentSmooth, smoothTime * Time.deltaTime);
        myCamera.transform.LookAt(_target);
    }

    private void UpdateCursorState()
    {
        foreach (KeyCode key in changeCursorState)
        {
            if (Input.GetKeyDown(key))
            {
                Cursor.visible = !Cursor.visible;
            }
        }

        _cursorLock = (Cursor.visible) ? CursorLockMode.None : CursorLockMode.Locked;

        Cursor.lockState = _cursorLock;
    }
}
