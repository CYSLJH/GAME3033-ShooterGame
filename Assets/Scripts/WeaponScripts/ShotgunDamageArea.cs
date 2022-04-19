using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunDamageArea : MonoBehaviour
{
    private List<GameObject> Targets = new List<GameObject>();
    public List<GameObject> GetTargets => Targets;
    private void OnDisable()
    {
        Targets.Clear();   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            Targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Targets.Remove(other.gameObject);
    }
}
