using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScreen : MonoBehaviour
{
    public Animator animator;
    public void StartGame()
    {   
        SceneManager.LoadScene("MainSceneSnow");
    }


}

