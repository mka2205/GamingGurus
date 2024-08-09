using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFireEnemy : MonoBehaviour
{
    private float last_check_time;
    public GameObject firePrefab;
    // Start is called before the first frame update
    void Start()
    {
        last_check_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time - last_check_time > 2.0f) {
            if (Random.Range(0.0f, 1.0f)<0.01f) {
                Instantiate(firePrefab, transform.position, Quaternion.identity);
            }
            last_check_time = Time.time;
        }
        
    }
}
