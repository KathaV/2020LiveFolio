using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject[] rooms;
    private GameObject mainMenu;
    public GameObject msgPrompter;
    private GameObject credits;
    private bool roomsDone;
    // Start is called before the first frame update
    void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("Room");
        mainMenu = GameObject.FindWithTag("Menu");
        credits = GameObject.FindWithTag("Credits");
        roomsDone = false;
        foreach (GameObject room in rooms)
        {
            LockRoom(room, false);
        }
        LockRoom(credits, true);

    }

    // Update is called once per frame
    void Update()
    {
        //remove instructions for all media once it has been played once.

        roomsDone = true;
        foreach (GameObject room in rooms)
        {
            MediaController media = room.GetComponentInChildren<MediaController>();
            if (!media.hasBeenPlayed())
            {
                roomsDone = false;
            }
            else if (!isLocked(room))
            {
               
                LockRoom(room, true);
            }

        }
        if (roomsDone && isLocked(credits))
        {
            LockRoom(credits, false);
        }


        //toggle message prompter off when menu is on screen.
        Tutorial msgScript = msgPrompter.GetComponent<Tutorial>();
        if (mainMenu.GetComponent<MenuController>().isOn() && msgScript.isBroadcasting()) {
            //print("not broadcasting");
            msgPrompter.SetActive(false);
        }
        else if (!mainMenu.GetComponent<MenuController>().isOn() && msgScript.isBroadcasting() && !MediaController.mediaStreaming)
        {
            //print("broadcasting and not streaming");
            msgPrompter.SetActive(true);
        }/*
        else
        {
            print("menu: " + mainMenu.GetComponent<MenuController>().isOn());
            print("braodcasting: " + msgScript.isBroadcasting());
            print("streaming: " + MediaController.mediaStreaming);

        }*/
    }

    void LockRoom(GameObject room, bool isLocked)
    {
        print("Locking: " + room.name + " " + isLocked);
        GameObject openTrigger = room.gameObject.transform.Find("jj_door_1_red/TriggerHallway").gameObject;

        GameObject lockedTrigger = room.gameObject.transform.Find("jj_door_1_red/TriggerLocked").gameObject;

        openTrigger.gameObject.SetActive(!isLocked);
        //print("Success1");
        lockedTrigger.gameObject.SetActive(isLocked);
        //print("Success2");



    }
    bool isLocked(GameObject room)
    {
        GameObject lockedTrigger = room.gameObject.transform.Find("jj_door_1_red/TriggerLocked").gameObject;
        return lockedTrigger.activeSelf;
    }


    /*void RemoveInstructions()
    {
        print("removing instructions");
        foreach (GameObject animator in animators)
        {
            MediaController media = animator.GetComponent<MediaController>();
            media.InstructionsDuration = 0f;
        }
    }*/

}
