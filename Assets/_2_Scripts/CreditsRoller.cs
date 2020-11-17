using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CreditsRoller : MonoBehaviour
{
    public GameObject blackoutPanel;
    private GameObject[] animators;
    public float fadeSpeed = 1f;
    public float fadeDelay = 0f;
    private Task fader_t;
    public GameObject credits;
    private bool creditsHasPlayed;
    // Start is called before the first frame update
    void Start()
    {
        animators = GameObject.FindGameObjectsWithTag("Animator");
        creditsHasPlayed = false;
    }


    // Update is called once per frame
    void OnTriggerEnter()
    {

        bool playCredits = true;
        MediaController media;
        if (!creditsHasPlayed) {
            foreach (GameObject animator in animators)
            {
                media = animator.GetComponent<MediaController>();
                if (media.hasBeenPlayed() != true)
                {
                    playCredits = false;
                    break;
                }
            }
            print("play credits:" + playCredits);
            if (playCredits)
            {
                
                fader_t = new Task(FadeToBlack(fadeSpeed, fadeDelay));
            }

        }
        
    }

    void Update()
    {
        if (fader_t!=null && !fader_t.Running && !creditsHasPlayed)
        {
            credits.SetActive(true);
            
            Cursor.lockState = CursorLockMode.None;

            Cursor.visible = true;
            creditsHasPlayed = true;
        }
    
    }

    public void stopCredits()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        new Task(FadeToBlack(fadeSpeed, fadeDelay, false));

    }
    public IEnumerator FadeToBlack(float fadespeed, float fadeDelay, bool fade = true)
    {
        Color objColor = blackoutPanel.GetComponent<Image>().color;
        float fadeAmount;
        yield return new WaitForSeconds(fadeDelay);
        if (fade)
        {
            while (objColor.a < 1)
            {
                fadeAmount = objColor.a + (fadespeed * Time.deltaTime);
                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                blackoutPanel.GetComponent<Image>().color = objColor;
                yield return null;
            }
        }
        else
        {
            while (objColor.a > 0)
            {
                fadeAmount = objColor.a - (fadespeed * Time.deltaTime);
                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                blackoutPanel.GetComponent<Image>().color = objColor;
                yield return null;
            }
        }
    }

    
}
