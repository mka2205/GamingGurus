using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using static EnemyEnum;

public class EnemySquidAI : Enemy
{
    public enum Direction { Left, Right };

    public float speed = 2;
    public int hopRange = 2;
    public GameObject target;
    public float targetAttackRange;
    public Direction dir = Direction.Left;

    private const float frameLength = 1 / 24f;
    private const int bounceFrames = 10;
    private const float animationSpeed = 0.5f;

    private Animator anim;
    private bool attacked = false;
    private Transform transfom;
    private float hopTime;
    private int hopCount = 0;
    private float targetDistance;
    private Transform targetTransform;
    private SquidProjectileLauncher launcher;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectsWithTag("Player")[0];
        }

        anim = GetComponent<Animator>();
        targetTransform = target.GetComponent<Transform>();
        launcher = GetComponentInChildren<SquidProjectileLauncher>();

        hopTime = frameLength * bounceFrames * (1 / animationSpeed);
        Invoke("changeDirection", 1.5f * hopTime);
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = (targetTransform.position - transform.position).magnitude;
        facePlayer();
        auto();
    }

    protected override void fight()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetTrigger("Wander");

            if (attacked == false)
            {
                launcher.launch();
                attacked = true;
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Bounce"))
        {
            state = EnemyState.Wander;
            Invoke("changeDirection", 1.5f * hopTime);
            attacked = false;
        }
    }

    protected override void wander()
    {
        Vector3 worldDir = new Vector3();

        switch (dir)
        {
            case Direction.Left:
                {
                    worldDir = transform.TransformDirection(-1, 0, 0);
                    break;
                }
            case Direction.Right:
                {
                    worldDir = transform.TransformDirection(1, 0, 0);
                    break;
                }
        }

        transform.position = transform.position + worldDir * speed * Time.deltaTime;

        if (targetDistance < targetAttackRange)
        {
            anim.SetTrigger("Fight");
            state = EnemyState.Fight;
            flipDirection();
        }
    }

    void changeDirection()
    {
        if (state == EnemyState.Wander)
        {
            Invoke("changeDirection", hopTime);
            hopCount += 1;
            if (hopCount == hopRange)
            {
                hopCount *= -1;
                flipDirection();
            }
        }
    }

    void flipDirection()
    {
        switch (dir)
        {
            case Direction.Left:
                {
                    dir = Direction.Right;
                    break;
                }
            case Direction.Right:
                {
                    dir = Direction.Left;
                    break;
                }
        }
    }

    void facePlayer()
    {
        Vector3 targetDirection = targetTransform.position - transform.position;
        float maxRadianDelta = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDirection, maxRadianDelta, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
