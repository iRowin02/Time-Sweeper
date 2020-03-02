using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeField : MonoBehaviour
{
    [SerializeField]
    public Dictionary<int, GameObject> enemies = new Dictionary<int, GameObject>();

    void OnTriggerStay(Collider other)
    {
        int id = other.gameObject.GetInstanceID();
        if (!enemies.ContainsKey(id)) { enemies.Add(id, other.gameObject); }
        Freeze(other);
    }

    private void OnTriggerExit(Collider other)
    {
        int id = other.gameObject.GetInstanceID();
        if (enemies.ContainsKey(id)) { enemies.Remove(id); }
    }


    void Freeze(Collider coll)
    {
        // enemies will contain all gameobjects currently in the collider
        foreach (KeyValuePair<int,GameObject> kvp in enemies)
        {
            GameObject enemy = kvp.Value;
            
            var _enemy = coll.GetComponent<FakePlayer>();

            if(_enemy.enemyStates != FakePlayer.states.TimeFrozen)
            {
                _enemy.enemyStates = FakePlayer.states.TimeFrozen;
            }
            else
            {
                _enemy.enemyStates = FakePlayer.states.Walking;
            }
        }
    }
}