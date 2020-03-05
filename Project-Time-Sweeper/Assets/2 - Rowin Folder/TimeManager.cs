using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public PlayerCamera playerCamera;

    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;

    public float maxSlowDown;

    void Update()
    {
        Time.timeScale += (1f / slowDownLength) * Time.deltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        playerCamera.mouseSen += (playerCamera.mouseSen / slowDownLength ) * Time.deltaTime;
        playerCamera.mouseSen = Mathf.Clamp(playerCamera.mouseSen, 0, playerCamera.maxMouseSen);
    }

    public void SlowDown()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;

        playerCamera.mouseSen = playerCamera.mouseSen * 2;
        print("Slowing Down Now");
    }
    void Test() 
    {
        
    }
}
