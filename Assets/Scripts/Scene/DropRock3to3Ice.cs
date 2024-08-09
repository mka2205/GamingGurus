using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class DropRock3to3Ice : MonoBehaviour
{
    GameObject rock;
    GameObject oldenemies;
    GameObject[] L3enemies;
    GameObject rockToHide;
    GameObject Player;
    PlayerHealthController health;    
    // Start is called before the first frame update
    void Start()
    {
        rock = GameObject.Find("RockSceneTrans3choosePath/Rock1AToIce");
        rockToHide = GameObject.Find("RockSceneTrans3choosePath/Rock1AIceOut");
        // oldenemies = GameObject.Find("enemyL2");
        L3enemies = GameObject.FindGameObjectsWithTag("IceEnemy3rdScene");
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
        rockToHide.SetActive(false);
        // oldenemies.SetActive(false);
        foreach (GameObject curr in L3enemies) {
            EnemyFlameAI aiScript = curr.GetComponent<EnemyFlameAI>();
            aiScript.state = EnemyState.Wander;
        }
        health.setSceneNumber(4);
    }
}
