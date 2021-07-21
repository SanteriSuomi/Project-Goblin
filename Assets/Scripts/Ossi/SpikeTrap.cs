using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField]
    float damage = 100f;

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Hit " + other.gameObject.name);
        if (other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth comp))
        {
            comp.ModifyHealth(-damage);
        }
    }
}
