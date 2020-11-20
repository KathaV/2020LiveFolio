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
    private GameObject player;
    public GameObject spawnPoint;
    private bool creditsPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        animators = GameObject.FindGameObjectsWithTag("Animator");
        creditsHasPlayed = false;
        creditsPlaying = false;
        player = GameObject.FindWithTag("Player");
    }




    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {

        MediaController media;
        if (other.gameObject.tag =="Player") {
            
            
          

            creditsPlaying = true;
            fader_t = new Task(FadeToBlack(fadeSpeed, fadeDelay));
            

        }
        
    }

    
    void Update()
    {
        if (creditsPlaying && fader_t!=null && !fader_t.Running)
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
        //respawn
        CharacterController controller = player.GetComponent<CharacterController>();
        
        controller.enabled = false; 
        player.transform.position = spawnPoint.transform.position;
        player.transform.eulerAngles = new Vector3(0, -90, 0);
        controller.enabled = true;
        print("player rot:" + player.transform.rotation);

        credits.SetActive(false);
        creditsPlaying = false;
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
    public bool creditsPlayed()
    {
        return creditsHasPlayed;
    }

    
}
