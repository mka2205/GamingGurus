using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class DropRock3to3Fire : MonoBehaviour
{

    GameObject rock;
    GameObject oldenemies;
    GameObject[] L3enemies;
    GameObject rockToHide;
    // Start is called before the first frame update
    void Start()
    {
        rock = GameObject.Find("RockSceneTrans3choosePath/Rock1AToFire");
        rockToHide = GameObject.Find("RockSceneTrans3choosePath/Rock1AFireOut");
        // oldenemies = GameObject.Find("enemyL2");
        L3enemies = GameObject.FindGameObjectsWithTag("FireEnemy3rdScene");

        // UnityEngine.Debug.Log(rock);
        // UnityEngine.Debug.Log(oldenemies);
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
    }
}
