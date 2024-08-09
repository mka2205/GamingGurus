using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damageAmount = 5;

    void OnCollisionEnter(Collision other)
    {
        damagePlayer(other.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        damagePlayer(other.gameObject);
    }

    private void damagePlayer(GameObject o)
    {
        if (o.tag == "Player")
        {
            PlayerHealthController phc = o.GetComponent<PlayerHealthController>();
            if (phc != null)
            {
                phc.takeDamage(damageAmount);
            }
        }
    }
}
