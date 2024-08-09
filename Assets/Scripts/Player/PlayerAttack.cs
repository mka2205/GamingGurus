using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyEnum;

public class PlayerAttack : MonoBehaviour
{
    public int lightDamage = 15;
    public int heavyDamage = 20;

    private Animator anim;
    private EnemyType type;
    private Collider weaponCollider;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        weaponCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponCollider.enabled && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            weaponCollider.enabled = false;
        }
    }

    public void setAttackType(bool isStanceFire)
    {
        if (isStanceFire)
        {
            type = EnemyType.Fire;
        }
        else
        {
            type = EnemyType.Ice;
        }
    }

    public void lightAttack()
    {
        StartCoroutine(enableCollider(lightDamage));
    }

    public void heavyAttack()
    {
        StartCoroutine(enableCollider(heavyDamage));
    }

    IEnumerator enableCollider(int damage)
    {
        yield return new WaitForSeconds(0.4f);
        weaponCollider.enabled = true;
        this.damage = damage;
    }

    private void OnTriggerStay(Collider other)
    {
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage, type);
        }
    }
}
