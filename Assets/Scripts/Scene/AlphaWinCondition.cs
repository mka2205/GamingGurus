using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWinCondition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Player") {
            // GO TO WIN SCENE
        }
    }
}
