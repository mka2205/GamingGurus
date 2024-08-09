using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Ensure you have this namespace

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenuToggle : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    // Reference to the main camera's CinemachineBrain
    public CinemachineBrain cinemachineBrain;
    public GameObject healthBar;

    void Start() {
        // healthBar = GameObject.Find("Canvas/Health bar");
    } 

    private void Awake()
    {
        // healthBar = GameObject.Find("Health bar");
        Cursor.visible = false;
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup Not Found");
        }

        if (cinemachineBrain == null)
        {
            Debug.LogError("CinemachineBrain Not Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
                healthBar.SetActive(true);

                // Enable CinemachineBrain (camera movement)
                if (cinemachineBrain != null)
                    cinemachineBrain.enabled = true;
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
                healthBar.SetActive(false);

                // Disable CinemachineBrain (camera movement)
                if (cinemachineBrain != null)
                    cinemachineBrain.enabled = false;
            }

            Cursor.visible = canvasGroup.interactable;
        }
    }
}
