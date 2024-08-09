using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstAttack : MonoBehaviour
{
    private ParticleSystem pSystem;
    private SphereCollider col;
    
    // Start is called before the first frame update
    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
        col = GetComponent<SphereCollider>();
        col.enabled = false;
    }

    public void attack()
    {
        pSystem.Play();
        col.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController phc = other.gameObject.GetComponent<PlayerHealthController>();
            if (phc != null) {
                phc.takeDamage(5);
            }
        }
        col.enabled = false;
    }
}
