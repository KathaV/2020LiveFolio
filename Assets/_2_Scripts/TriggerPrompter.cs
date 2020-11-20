using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerPrompter : MonoBehaviour
{
    public GameObject msgBox;
    public AudioSource soundEffect;
    private GameObject msgPrompt;
    public string triggerMessage;
    private bool tutorialPaused;
    public float messageDuration;
    private Tutorial tutorial;
    private Task msgShower;
    // Start is called before the first frame update
    void Start()
    {
        tutorialPaused = false;
        msgPrompt = msgBox.transform.GetChild(0).gameObject;
        tutorial = msgBox.GetComponent<Tutorial>();
    }
    void Update()
    {
        if (tutorialPaused && msgShower != null && !msgShower.Running)
        {
            tutorial.Play();
            msgShower = null;
            tutorialPaused = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        print("locked collision");
        if (soundEffect != null)
        {
            soundEffect.Play();
        }
        if (other.gameObject.tag == "Player")
        {
            tutorial.Pause();
            tutorialPaused = true;
            msgShower = new Task(ShowText(msgBox, msgPrompt, triggerMessage));

        }
    }

    public IEnumerator ShowText(GameObject msgBox, GameObject msgPrompter, string msg)
    {
        print("showing locked message");
        msgBox.SetActive(true);
        msgPrompter.SetActive(true);
        msgPrompt.GetComponent<TextMeshProUGUI>().text = msg;
        yield return new WaitForSeconds(messageDuration);
        msgBox.SetActive(false);
        msgPrompter.SetActive(false);
    }
}
