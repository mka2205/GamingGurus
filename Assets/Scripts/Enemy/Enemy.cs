using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnemyEnum;
using System.ComponentModel;
using System.CodeDom;
using System;

public class Enemy : MonoBehaviour
{
    public EnemyState state;
    public EnemyType type;
    public Renderer renderer;

    [SerializeField] int maxHealth = 20;
    [SerializeField] EnemyHealthBar healthBar;
    [SerializeField] GameObject itemDrop;

    private int health;
    private float lastAttackTime;
    private float damageTime = 0.25f;

    public void TakeDamage(int d, EnemyType attackType)
    {
        Debug.Log(attackType);
        if (attackType != type)
        {
            Debug.Log(attackType + "  " + type);
            if (Time.time - lastAttackTime >= 0.5)
            {
                health -= d;
                lastAttackTime = Time.time;
                StartCoroutine(showDamage());
            }

            if (health <= 0)
            {
                StartCoroutine(die());
            }
        }

    }

    IEnumerator showDamage()
    {
        Color damageColor = Color.red;
        Color normalColor = renderer.material.color;
        Color normalEmissionColor = renderer.material.GetColor("_EmissionColor");

        renderer.material.color = damageColor;
        renderer.material.SetColor("_EmissionColor", damageColor);
        yield return new WaitForSeconds(damageTime);
        renderer.material.color = normalColor;
        renderer.material.SetColor("_EmissionColor", normalEmissionColor);
    }

    public void setState(EnemyState newState)
    {
        state = newState;
    }

    private void Awake() {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        health = maxHealth;
        lastAttackTime = Time.time;
    }
    private void Start(){
        health  = maxHealth;
        healthBar.UpdateHealthBar(health,maxHealth);
    }

    protected void auto()
    {
        switch(state)
        {
            case EnemyState.Idle:
            {
                idle();
                break;
            }
            case EnemyState.Chase:
            {
                chase();
                break;
            }
            case EnemyState.Fight:
            {
                fight();
                break;
            }
            case EnemyState.Flee:
            {
                flee();
                break;
            }
            case EnemyState.Wander:
            {
                wander();
                break;
            }
        }
    }

    protected virtual void idle()
    {

    }

    protected virtual void chase()
    {
        
    }
    
    protected virtual void fight()
    {
    
    }
    
    protected virtual void flee()
    {
    
    }
    
    protected virtual void wander()
    {
    
    }

    protected virtual IEnumerator die()
    {
        yield return new WaitForSeconds(damageTime);
        gameObject.SetActive(false);
        Instantiate(itemDrop, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),Quaternion.identity);
    }
    
}
