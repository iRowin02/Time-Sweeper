using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(3);
    }
}
