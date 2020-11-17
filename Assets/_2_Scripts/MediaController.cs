using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Assertions;
using System;

public class MediaController : MonoBehaviour
{
    public GameObject blackoutPanel;
    public float fadeSpeed=0.5f;
    public float fadeAwaySpeed = 0.8f;
    public float fadeDelay = 1.0f;
    public VideoClip videoClip;
    public GameObject spawnPoint;
    public List<GameObject> otherUI;
    public GameObject instructions;
    private bool isFading;
    private Task fader_t;
    private Task unfader_t;
    private VideoPlayer vidPlyr;
    private bool isStreaming = false;
    private RawImage img;
    private GameObject canvas;
    private FirstPersonController movement;
    private bool hasPlayed = false;
    private GameObject player;
    private long pauseFrame;
    private GameObject pausePanel;
    private GameObject restartPanel;
    public float InstructionsDuration = 5f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindWithTag("UI");
        player = GameObject.FindWithTag("Player");
        print("PLayer:" + player.name);
        pausePanel = canvas.transform.Find("Pause Panel").gameObject;
        restartPanel = canvas.transform.Find("Restart Panel").gameObject;
        instructions.SetActive(false);
        //PrepareVideo();
        timer = InstructionsDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPlayed && fader_t != null && !fader_t.Running && !isStreaming )
        {
            print("fading stopped!");

            /* vidPlyr = blackoutPanel.GetComponentInChildren<VideoPlayer>();
             vidPlyr.Play();*/
            //vidPlyr.Pause();

            //unfader_t = new Task(FadeFromBlack(fadeAwaySpeed));
            

            ToggleOtherUI(false);
            instructions.SetActive(true);
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                print("videoPlaying!");
                instructions.SetActive(false);
                isStreaming = true;
                PlayVideo(canvas);
                Assert.IsNotNull(movement);
                movement.enabled = false;
            }
            


        }
        if (vidPlyr != null && isStreaming)
        {
            if (Input.GetKey("e"))
            {
                if (spawnPoint != null)
                {
                    CharacterController controller = player.GetComponent<CharacterController>();
                    controller.enabled = false;
                    player.transform.position = spawnPoint.transform.position;
                    controller.enabled = true;
                }

                StopVideo();
            }

            long playerCurrentFrame = vidPlyr.GetComponent<VideoPlayer>().frame;
            long playerFrameCount = Convert.ToInt64(vidPlyr.GetComponent<VideoPlayer>().frameCount);
            if (Input.GetKeyDown("space") && playerCurrentFrame>pauseFrame)
            {
                print("pausing");
                pausePanel.SetActive(true);
                vidPlyr.Pause();
                pauseFrame = playerCurrentFrame;
            }
            else if (Input.GetKeyDown("space") && playerCurrentFrame == pauseFrame)
            {
                pausePanel.SetActive(false);
                print("playing");
                vidPlyr.Play();
            }
            else if (Input.GetKeyDown("r"))
            {
                StartCoroutine(ShowPanel(1f, restartPanel));
                RestartVideo();
            }
            if (playerCurrentFrame ==playerFrameCount)
            {
                if (spawnPoint != null)
                {
                    CharacterController controller = player.GetComponent<CharacterController>();
                    controller.enabled = false;
                    player.transform.position = spawnPoint.transform.position;
                    controller.enabled = true;
                    player.transform.Rotate(0.0f, -90.0f, 0.0f, Space.World);
                }
                
                StopVideo();
            }
            
        }

    }

    void RestartVideo()
    {
        vidPlyr.loopPointReached += EndReached;
        vidPlyr.Stop();
        vidPlyr.Prepare();
        vidPlyr.loopPointReached += EndReached;
        pauseFrame = 0;
        vidPlyr.frame = 0;
        vidPlyr.Play();
    }


    void ToggleOtherUI(bool isActive)
    {
        foreach (GameObject ui in otherUI)
        {
            ui.SetActive(isActive);
        }
    }

    void StopVideo()
    {

        vidPlyr.loopPointReached += EndReached;
        vidPlyr.Stop();

        //vidPlyr.enabled = false;
        Task fader_t = new Task(FadeInRawImage(fadeSpeed, img, false));

        movement.enabled = true;

        Task fader2_t = new Task(FadeToBlack(fadeSpeed, fadeDelay + 1 / fadeAwaySpeed, false));
        isStreaming = false;

        ToggleOtherUI(true);
        hasPlayed = true;
        vidPlyr.enabled = false;
        timer = InstructionsDuration;
    }
    void PrepareVideo()
    {

        vidPlyr = canvas.GetComponentInChildren<VideoPlayer>();

        vidPlyr.clip = videoClip;
        vidPlyr.Prepare();
        vidPlyr.loopPointReached += EndReached;
        pauseFrame = 0;
    }
    void PlayVideo(GameObject canvas)
    {
        vidPlyr = canvas.GetComponentInChildren<VideoPlayer>();

        vidPlyr.loopPointReached += EndReached;
        vidPlyr.enabled = true;
        vidPlyr.clip = videoClip;
        vidPlyr.Prepare();
        vidPlyr.frame = 0;
        vidPlyr.Play();
        img = canvas.GetComponentInChildren<RawImage>();
        img.enabled = false;
        //img = new RawImage();
        img.enabled = true;
       // img.texture = 
        
        var fader_t = new Task( FadeInRawImage(fadeAwaySpeed, img));
        //Cursor.visible = true;
    }
    public bool hasBeenPlayed()
    {
        return hasPlayed;
    }
    void EndReached(VideoPlayer vp)
    {
        // Reset video to first frame
        vp.frame = 0;
    }
    //https://turbofuture.com/graphic-design-video/How-to-Fade-to-Black-in-Unity#:~:text=Method%201%3A%20Attach%20To%20Camera&text=In%20Unity%2C%20go%20to%20Assets,Voila.
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isFading)
        {
            // StartCoroutine(FadeToBlack(fadeSpeed, fadeDelay));
            fader_t = new Task(FadeToBlack(fadeSpeed, fadeDelay));
            isFading = true;
            movement = other.gameObject.GetComponent<FirstPersonController>();
            //player = other.gameObject;
            
        }

        
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

    public IEnumerator ShowPanel(float duration, GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }
    public IEnumerator FadeInRawImage(float fadespeed, RawImage img, bool fade = true)
    {
        Color objColor = img.color;
        float fadeAmount;
        yield return new WaitForSeconds(fadeDelay*0.3f);
        if (fade)
        {
            while (objColor.a < 1)
            {
                fadeAmount = objColor.a + (fadespeed * Time.deltaTime);
                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                img.color = objColor;
                //img.color.a = img.color.a + (fadespeed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            while (objColor.a > 0)
            {
                fadeAmount = objColor.a - (fadespeed * Time.deltaTime);
                objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
                img.color = objColor;
                yield return null;
            }
            img.enabled = false;
        }
        
    }
}
