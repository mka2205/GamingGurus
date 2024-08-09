using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class DropRock2to3 : MonoBehaviour
{
    GameObject rock;
    GameObject oldenemies;
    GameObject[] L3enemies;
    GameObject Player;
    PlayerHealthController health;
    // Start is called before the first frame update
    void Start()
    {
        rock = GameObject.Find("RockSceneTrans2to3/Rock1A");
        oldenemies = GameObject.Find("FireEnemySecondScene");
        // L3enemies = GameObject.FindGameObjectsWithTag("enemyL3");
        Player = GameObject.Find("Player2");
        health = Player.GetComponent<PlayerHealthController>();

        UnityEngine.Debug.Log(rock);
        UnityEngine.Debug.Log(oldenemies);
        // UnityEngine.Debug.Log(L3enemies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        if (oldenemies != null) oldenemies.SetActive(false);
        // foreach (GameObject curr in L3enemies) {
        //     EnemyFlameAI aiScript = curr.GetComponent<EnemyFlameAI>();
        //     aiScript.state = EnemyState.Wander;
        // }
        health.setSceneNumber(3);
    }
}
