using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class FightOnTriggerEnter : MonoBehaviour
{
    private BossAI boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponentInParent<BossAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (boss != null && other.gameObject.tag == "Player")
        {
            boss.setState(EnemyState.Fight);
        }
    }
}
