using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    private float last_attack_time;
    public Transform respawnPoint1; 
    public Transform respawnPoint2; 
    public Transform respawnPoint3; 
    public Transform respawnPoint4; 
    public Transform respawnPoint5; 
    public CharacterController characterController;
    private GameObject checkpoint; 
    Collider collider;
    public int sceneNumber;
    Transform pos;

    private int damageFactor = 1;

    void Start()
    {
        currentHealth = maxHealth;
        last_attack_time = Time.time;
        healthBar.SetMaxHealth(maxHealth);
        characterController = GetComponent<CharacterController>();

        collider = gameObject.AddComponent<BoxCollider>(); 
        collider.enabled = false;
        collider.isTrigger = false;
        sceneNumber = 1;
    }

    public void setSceneNumber(int scene) {
        sceneNumber = scene;
        UnityEngine.Debug.Log(sceneNumber);
    }

    public void takeDamage(int damage)
    {
        if (Time.time - last_attack_time < 2.0f) return;
        currentHealth -= damage * damageFactor;
        healthBar.SetHealth(currentHealth);
        last_attack_time = Time.time;

        if (currentHealth <= 0)
        {
            collider.enabled = true;
            collider.isTrigger = true;
            die();
        }
    }

    public void healDamage(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("FireCheckpoint"))
        {
            checkpoint = other.gameObject;
        }
    }

    protected virtual void die()
    {
        characterController.enabled = false;

        switch (sceneNumber)
        {
            case 1:
                transform.position = respawnPoint1.position;
                transform.rotation = respawnPoint1.rotation;
                break;
            case 2:
                transform.position = respawnPoint2.position;
                transform.rotation = respawnPoint2.rotation;
                break;
            case 3:
                // pos = GameObject.Find("respawnPoint3").transform;
                // transform.position = pos.position;
                // transform.rotation = pos.rotation;
                transform.position = respawnPoint3.position;
                transform.rotation = respawnPoint3.rotation;
                break;
            case 4:
                // pos = GameObject.Find("respawnPoint4").transform;
                // transform.position = pos.position;
                // transform.rotation = pos.rotation;
                transform.position = respawnPoint4.position;
                transform.rotation = respawnPoint4.rotation;
                break;
            case 5:
                // pos = GameObject.Find("respawnPoint5").transform;
                // transform.position = pos.position;
                // transform.rotation = pos.rotation;
                transform.position = respawnPoint5.position;
                transform.rotation = respawnPoint5.rotation;
                break;
            default:
                break;
        }
        // Debug.Log("KillerObject: " +  checkpoint);

        // transform.position = respawnPoint1.position;
        // transform.rotation = respawnPoint1.rotation;

        // if (checkpoint != null)
        // {
        //     string checkpointTag = checkpoint.tag;
        //     Debug.Log("KillerObject: " + checkpoint);
        //     if (checkpointTag == "FireCheckpoint")
        //     {
        //         transform.position = respawnPoint2.position;
        //         transform.rotation = respawnPoint2.rotation;
        //     }

        // }

        collider.enabled = false;
        collider.isTrigger = false;

        characterController.enabled = true;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
}
