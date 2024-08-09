using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class DropRock : MonoBehaviour
{
    GameObject rock;
    GameObject oldenemies;
    GameObject[] L2enemies;
    GameObject Player;
    PlayerHealthController health;
    // Start is called before the first frame update
    void Start()
    {
        rock = GameObject.Find("RocksToTransition/Rock1A");
        oldenemies = GameObject.Find("IceEnemyFirstScene");
        L2enemies = GameObject.FindGameObjectsWithTag("enemyL2");
        Player = GameObject.Find("Player2");
        health = Player.GetComponent<PlayerHealthController>();

        // UnityEngine.Debug.Log(rock);
        // UnityEngine.Debug.Log(oldenemies);
        // UnityEngine.Debug.Log(L2enemies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.useGravity = true;
            oldenemies.SetActive(false);
        }
        foreach (GameObject curr in L2enemies) {
            EnemyFlameAI aiScript = curr.GetComponent<EnemyFlameAI>();
            aiScript.state = EnemyState.Wander;
        }
        health.setSceneNumber(2);
    }
}
