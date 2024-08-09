using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healAmount = 5;

    void OnCollisionEnter(Collision other)
    {
        healPlayer(other.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        healPlayer(other.gameObject);
    }

    private void healPlayer(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            PlayerHealthController phc = obj.GetComponent<PlayerHealthController>();
            
            if (phc != null)
            {
                phc.healDamage(healAmount);
                Destroy(gameObject);
            }
        }
    }

}
