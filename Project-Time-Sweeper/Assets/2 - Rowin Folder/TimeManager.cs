using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;

    public float maxSlowDown;

    public void SlowDown()
    {
        StartCoroutine(Slow());
    }
    IEnumerator Slow()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        print("Slowing Down Now");
        yield return new WaitForSeconds(maxSlowDown);
        print("Back to normal speed");
        Time.timeScale = 1;
    }
}
