using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class Tutorial : MonoBehaviour
{
    private GameObject msgBox;
    private GameObject msgPrompt;
    public float timeBetweenMsgs = 5;
    public List<string> messages;
    private float countdown;
    private int curr;
    private bool isActive;
    private bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        msgBox = this.gameObject;
        msgPrompt = msgBox.transform.GetChild(0).gameObject;
        countdown = timeBetweenMsgs;
        curr = 0;
        isActive = false;
        isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        print("count" + messages.Count + " curr: " + curr);
        if (isPlaying&& !isActive)
        {

            msgBox.SetActive(false);
            print("not active");
        }
            

        if (isPlaying && messages.Count > 0 && curr < messages.Count)
        {
            print("setting boxes...");
            msgBox.SetActive(true);
            msgPrompt.SetActive(true);
            isActive = true;
            string msg = messages[curr];
            msgPrompt.GetComponent<TextMeshProUGUI>().text = msg;
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                countdown = timeBetweenMsgs;
                curr++;
                if (curr >= messages.Count)
                {
                    print("curr" + curr);
                    print("removing boxes");
                    msgBox.SetActive(false);
                    msgPrompt.SetActive(false);
                    isActive = false;
                }
            }
        }
    }

    public void Pause()
    {
        isPlaying = false;
    }
    public void Play()
    {
        isPlaying = true;
    }

    public bool isBroadcasting()
    {
        return isActive;
    }

    public void Toggle(bool isOn)
    {
        isActive = isOn;
    }
    
    
}
