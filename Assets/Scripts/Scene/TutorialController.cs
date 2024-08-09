using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public bool step1 = true;
    public bool step2 = false;
    public bool step3 = false;
    public bool step4 = false;
    public bool step5 = false;
    public bool step6 = false;
    public bool step7 = false;

    public GameObject enemies;
    public GameObject backgroundImage;
    public GameObject leftBarrier;
    public GameObject rightBarrier;
    public GameObject healthBar;
    //public GameObject rocks;

    public TMP_Text messageText;

    //public InputField input = tutorialScript.GetComponent<InputField>();

    // Start is called before the first frame update
    void Start()
    {

        backgroundImage.SetActive(true);
        enemies.SetActive(false);
        leftBarrier.SetActive(true);
        rightBarrier.SetActive(true);
        //rocks.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("w") && step1)
        {
            messageText.SetText("Great Job! \nNow <u><b>press Z</u></b> to try switching characters in the case there is a " +
                "fire enemy!\n" +
                "Be careful, fire enemies could start fire that hurts!");
            step1 = false;
            step2 = true;
        }
        else if (Input.GetKeyUp("z") && step2)
        {
            messageText.SetText("Great Job! \nNow <u><b>press Z</u></b> again to switch back to kill the ice enemies that will be present in level 1 (they will pop up once the tutorial ends)!");

            step2 = false;
            step3 = true;
        }
        else if (Input.GetKeyUp("z") && step3)
        {
            messageText.SetText("Great Job! \nNow <u><b>press the Space Bar</u></b> to jump! ");
            step3 = false;
            step4 = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && step4)
        {
            messageText.SetText("Great Job! \nNow <u><b>press the Left Mouse Button</u></b> to attack!");
            step4 = false;
            step5 = true;
        }

        else if (Input.GetMouseButtonDown(0) && step5)
        {
            messageText.SetText("Great! Those cover the basic moves! \nRemember some other advanced moves \n"
                + "\nHeavy Attack: Right Mouse Button"
                + "\nDash: Left Shift Key \n"
                + "\nFeel free to practice these moves and <u><b>press the Enter key</u></b> when you are ready!");

            step5 = false;
            step6 = true;
        }

        else if (Input.GetKeyUp(KeyCode.Return) && step6)
        {
            messageText.SetText("Congrats on finishing the tutorial! \n\nOnce you are ready to start, <u><b>press the Enter key</u></b> to start the game! \n\nGood Luck!");

            step6 = false;
            step7 = true;
        }

        else if (Input.GetKeyDown(KeyCode.Return) && step7)
        {
            messageText.SetText("");
            enemies.SetActive(true);
            backgroundImage.SetActive(false);
            leftBarrier.SetActive(false);
            rightBarrier.SetActive(false);
            healthBar.SetActive(true);
            //rocks.SetActive(false);
        }

    }
}
