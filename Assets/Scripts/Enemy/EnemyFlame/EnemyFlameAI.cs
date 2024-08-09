using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using static EnemyEnum;

public class EnemyFlameAI : Enemy
{
    public float speed;
    public GameObject target;
    public float targetAttackRange;
    public float targetChaseRange;

    private Animator anim;
    private BurstAttack burstAttack;
    private Transform targetTransform;
    private float targetDistance;
    private Vector3 wanderPoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        anim = GetComponent<Animator>();
        burstAttack = GetComponentInChildren<BurstAttack>();
        targetTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = (targetTransform.position - transform.position).magnitude;
        auto();
    }

    protected override void wander()
    {
        Vector2 unitCircle = UnityEngine.Random.onUnitSphere * 5;
        Vector3 wanderJitter = new Vector3(unitCircle.x, 0, unitCircle.y);

        if (wanderPoint == Vector3.zero)
        {
            wanderPoint = transform.position + wanderJitter;
        }
        
        float dist = (wanderPoint - transform.position).magnitude;

        if (dist < 0.1)
        {
            wanderPoint = transform.position + wanderJitter;
        }
        else
        {
            steer2d(wanderPoint, true);
        }

        if (targetDistance < targetChaseRange)
        {
            state = EnemyState.Chase;
        }
    }

    protected override void chase()
    {
        steer2d(target.GetComponent<Transform>().position, true);

        if (targetDistance < targetAttackRange)
        {
            anim.SetTrigger("Fight");
            state = EnemyState.Fight;
        }
    }

    protected override void flee()
    {
        steer2d(target.GetComponent<Transform>().position, false);
    }

    protected override void fight()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Explode"))
        {
            burstAttack.attack();
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            state = EnemyState.Chase;
        }
    }

    private void steer2d(Vector3 vec, bool isChasing)
    {
        // Positional Movement
        Vector3 pos = transform.position;
        Vector3 targetDirection = vec - transform.position;
        targetDirection = targetDirection.normalized * speed;
        targetDirection.y = 0;

        if (!isChasing)
        {
            targetDirection = -targetDirection;
        }

        transform.position = pos + targetDirection * Time.deltaTime;

        // Turn in movement direction
        float maxRadianDelta = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, maxRadianDelta, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
