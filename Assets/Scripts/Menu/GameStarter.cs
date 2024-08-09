using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    public Animator animator;
    public void StartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.Debug.Log("inside start game");
        
        // SceneManager.LoadScene("IntroScreen");
        
        animator.SetTrigger("FadeOut");
        Invoke("onFadeComplete", 1);
        // SceneManager.LoadScene("MainSceneSnow");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        // animator.SetTrigger("FadeOut");
        UnityEngine.Debug.Log("inside restart game");
        
        SceneManager.LoadScene("MainSceneSnow");
        
        
        // Invoke("onFadeComplete", 1);
        // SceneManager.LoadScene("MainSceneSnow");
    }

    public void QuitGame()
    {      

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    public void onFadeComplete()
    {
        SceneManager.LoadScene("IntroScreen");
    }


}

