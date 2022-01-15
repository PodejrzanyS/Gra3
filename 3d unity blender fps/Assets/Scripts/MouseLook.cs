using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{


    static public float CurrentMouseSensitivity;
    public float mouseSensitivity;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        wrocSensitivity();
    }

    public void wrocSensitivity()
    {
        CurrentMouseSensitivity = mouseSensitivity;
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * CurrentMouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * CurrentMouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
