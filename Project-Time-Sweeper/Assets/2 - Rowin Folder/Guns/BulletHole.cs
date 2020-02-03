using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    public float despawn;
    void Start()
    {
        StartCoroutine(Despawner());
    }
    IEnumerator Despawner()
    {
        yield return new WaitForSeconds(despawn);

        Destroy(gameObject);
    }
}
