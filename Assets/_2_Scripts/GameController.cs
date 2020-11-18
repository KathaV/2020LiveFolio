using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject[] animators;
    private GameObject mainMenu;
    public GameObject msgPrompter;
    private bool instructionsShown;
    
    // Start is called before the first frame update
    void Start()
    {
        animators = GameObject.FindGameObjectsWithTag("Animator");
        mainMenu = GameObject.FindWithTag("Menu");
        instructionsShown = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //remove instructions for all media once it has been played once.
       /* if (!instructionsShown)
        {
            foreach(GameObject animator in animators)
            {
                MediaController media = animator.GetComponent<MediaController>();
                if (media.hasBeenPlayed())
                {
                    print("has been played");
                    RemoveInstructions();
                    instructionsShown = true ;
                    break;
                }
            }
        }*/

        //toggle message prompter off when menu is on screen.
        Tutorial msgScript = msgPrompter.GetComponent<Tutorial>();
        if (mainMenu.GetComponent<MenuController>().isOn() && msgScript.isBroadcasting()){
            msgPrompter.SetActive(false);
        }
        else if (!mainMenu.GetComponent<MenuController>().isOn() && msgScript.isBroadcasting() && !MediaController.mediaStreaming)
        {
            msgPrompter.SetActive(true);
        }

        

    }
    void RemoveInstructions()
    {
        print("removing instructions");
        foreach (GameObject animator in animators)
        {
            MediaController media = animator.GetComponent<MediaController>();
            media.InstructionsDuration = 0f;
        }
    }
}
