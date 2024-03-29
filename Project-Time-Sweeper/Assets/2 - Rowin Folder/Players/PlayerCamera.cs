﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;

    public Vector2 minMaxClampValue;

    public float mouseSen;
    public float maxMouseSen;
    public float clampValue = 90.0f;
    private float xAxisClamp = 90.0f;

    public bool lockedMouse;

    void Awake()
    {
        maxMouseSen = mouseSen;
        LockCursor();
        xAxisClamp = 0.0f;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && lockedMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            lockedMouse = false;
        }
        else if (Input.GetButtonDown("Cancel") && !lockedMouse)
        {
            Cursor.lockState = CursorLockMode.None;
            lockedMouse = true;
        }

        if (!lockedMouse)
        {
            CameraRotation();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * (mouseSen*10) * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSen*10) * Time.unscaledDeltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > clampValue)
        {
            xAxisClamp = clampValue;
            mouseY = 0.0f;
            ClampXAxisrotationToValue(minMaxClampValue.y);
        }

        else if (xAxisClamp < -clampValue)
        {
            xAxisClamp = -clampValue;
            mouseY = 0.0f;
            ClampXAxisrotationToValue(minMaxClampValue.x);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisrotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}
