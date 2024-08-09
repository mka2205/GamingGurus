using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSceneDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c)
    {
        
        // UnityEngine.Debug.Log(c.attachedRigidbody);
        // GameObject gphc = GetComponent<Player1>();
        // PlayerHealthController phc = gphc.GetComponent<PlayerHealthController>();
        // phc.takeDamage(20);
        
        if (c.gameObject != null) {
            PlayerHealthController phc = c.gameObject.GetComponent<PlayerHealthController>();
            if (phc != null) {
                phc.takeDamage(1);
                UnityEngine.Debug.Log("Fire hit 1 point");
            }
            
            // EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
            // Destroy(this.gameObject);
        }
    }
}
