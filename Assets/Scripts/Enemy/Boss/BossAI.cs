using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UnityEngine;
using static EnemyEnum;

public class BossAI : Enemy
{

    public GameObject flameStreamPrefab;
    public GameObject frostStreamPrefab;
    public GameObject flameEnemyPrefab;
    public GameObject frostEnemyPrefab;

    public GameObject leftEye;
    public GameObject rightEye;
    public GameObject leftEnemyParent;
    public GameObject rightEnemyParent;
    public GameObject flameBarrier;


    private GameObject target;
    private int attackNum;
    private bool attacking = false;
    private ParticleSystem leftEyeFlame;
    private ParticleSystem rightEyeFlame;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];

        leftEyeFlame = leftEye.GetComponentInChildren<ParticleSystem>();
        rightEyeFlame = rightEye.GetComponentInChildren<ParticleSystem>();
        leftEyeFlame.Play();
        rightEyeFlame.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        auto();
    }

    protected override void fight()
    {
        if (attackNum != 0 && attackNum % 4 == 0)
        {
            flameBarrier.SetActive(false);
            state = EnemyState.Idle;
            attackNum += 1;
            switchStateToFight(15);
        }
        else
        {
            flameBarrier.SetActive(true);
            state = EnemyState.Idle;

            if (!attacking)
            {
                attacking = true;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    StartCoroutine(streamAttack());
                }
                else
                {
                    StartCoroutine(minionAttack());
                }

                attackNum += 1;
            }
        }
        
    }

    IEnumerator streamAttack()
    {
        Vector3 eyePos;
        if (type == EnemyType.Fire)
        {
        eyePos = leftEye.GetComponent<Transform>().position;
        }
        else
        {
        eyePos = rightEye.GetComponent<Transform>().position;
        }

        Vector3 targetPos = target.GetComponent<Transform>().position;
        Vector3 targetDirection = targetPos - eyePos;
        targetDirection.y = 0;
        targetDirection = targetDirection.normalized;

        float delay = 0.5f;
        int startDistance = 5;
        int offset = 1;
        for (int i = 0; i < 12; i++)
        {
            int distanceFactor = startDistance + (offset * i);
            Vector3 streamPos = eyePos + (targetDirection * distanceFactor);
            streamPos.y = 0;
            if (type == EnemyType.Fire)
            {
                Instantiate(flameStreamPrefab, streamPos, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(frostStreamPrefab, streamPos, Quaternion.identity, transform);
            }
            
            yield return new WaitForSeconds(delay);
        }

        state = EnemyState.Fight;
        switchType();
        attacking = false;
    }

   IEnumerator minionAttack()
    {
        Transform leftTransform = leftEnemyParent.GetComponent<Transform>();
        Transform rightTransform = rightEnemyParent.GetComponent<Transform>();

        if (type == EnemyType.Fire)
        {
            Instantiate(flameEnemyPrefab, leftTransform);
            Instantiate(flameEnemyPrefab, rightTransform);
        }
        else
        {
            Instantiate(frostEnemyPrefab, leftTransform);
            Instantiate(frostEnemyPrefab, rightTransform);
        }

        yield return new WaitForSeconds(15);
        state = EnemyState.Fight;
        switchType();
        attacking = false;
    }

    private void switchStateToFight(int seconds)
    {
        Invoke("switchStateToFightHelper", seconds);
    }

    private void switchStateToFightHelper()
    {
        state = EnemyState.Fight;
    }

    private void switchType()
    {
        switch(type)
        {
            case EnemyType.Fire:
                {
                    type = EnemyType.Ice;
                    leftEyeFlame.Stop();
                    rightEyeFlame.Play();
                    break;
                }
            case EnemyType.Ice:
                {
                    type = EnemyType.Fire;
                    leftEyeFlame.Play();
                    rightEyeFlame.Stop();
                    break;
                }
        }
    }
}
