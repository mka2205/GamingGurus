using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class DropRock3to4 : MonoBehaviour
{
    GameObject rock;
    GameObject oldenemies;
    GameObject[] L4enemies;
    GameObject rockToHide;
    GameObject Player;
    PlayerHealthController health;    
    // Start is called before the first frame update
    void Start()
    {
        rock = GameObject.Find("RockSceneTrans3to4/Rock1A");
        // rockToHide = GameObject.Find("RockSceneTrans3choosePath/Rock1AIceOut");
        // oldenemies = GameObject.Find("enemyL2");
        L4enemies = GameObject.FindGameObjectsWithTag("enemyL4");
        Player = GameObject.Find("Player2");
        health = Player.GetComponent<PlayerHealthController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c) {
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        // rockToHide.SetActive(false);
        // oldenemies.SetActive(false);
        foreach (GameObject curr in L4enemies) {
            if (curr != null) {
                EnemyFlameAI aiScript = curr.GetComponent<EnemyFlameAI>();
                if (aiScript != null) {
                    aiScript.state = EnemyState.Wander;
                } else {
                    BossAI bossScript = curr.GetComponent<BossAI>();
                    if (bossScript != null) {
                        bossScript.state = EnemyState.Wander;
                    }
                }
            }  
        }        
        health.setSceneNumber(5);
    }
}
