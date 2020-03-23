using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public PlayerCamera playerCamera;

    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;

    public float maxSlowDown;

    private float sens, oldsens;
    void Awake()
    {
        oldsens = playerCamera.mouseSen;
        sens = (playerCamera.mouseSen * 2);
    }

    void Update()
    {
        Time.timeScale += (1f / slowDownLength) * Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        if(Time.timeScale == 1)
        {
            playerCamera.mouseSen = oldsens;
        }
    }

    public void SlowDown()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;

        playerCamera.mouseSen = sens;

        print("Slowing Down Now");
    }
}
