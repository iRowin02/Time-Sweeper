using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public PlayerMove player;
    public PlayerCamera playerCam;

    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;

    public float maxSlowDown;



    public void SlowDown()
    {
        StartCoroutine(ResetTime());
    }
    IEnumerator ResetTime()
    {
        Time.timeScale = Time.timeScale / 2;
        Time.fixedDeltaTime = Time.timeScale;

        playerCam.mouseSen = playerCam.mouseSen * 2;
        player.movementSpeed = player.movementSpeed * 2;

        yield return new WaitForSeconds(4);

        playerCam.mouseSen = playerCam.mouseSen / 2;
        player.movementSpeed = player.movementSpeed / 2;

        Time.timeScale = Time.timeScale * 2;
        Time.fixedDeltaTime = Time.timeScale;
    }
}
