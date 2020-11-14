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
    

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindWithTag("UI");
        player = GameObject.FindWithTag("Player");
        print("PLayer:" + player.name);

        //PrepareVideo();

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

            isStreaming = true;
            print("videoPlaying!");
            
            PlayVideo(canvas);
            Assert.IsNotNull(movement);
            movement.enabled = false;


        }
        if (vidPlyr != null)
        {
            if (isStreaming && Input.GetKey("e"))
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
            if (isStreaming && playerCurrentFrame ==playerFrameCount)
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
            hasPlayed = true;
        }

    }

    void StopVideo()
    {
        vidPlyr.Stop();
        //vidPlyr.enabled = false;
        Task fader_t = new Task(FadeInRawImage(fadeSpeed, img, false));

        movement.enabled = true;

        Task fader2_t = new Task(FadeToBlack(fadeSpeed, fadeDelay + 1 / fadeAwaySpeed, false));
        isStreaming = false;
    }
    void PrepareVideo()
    {

        vidPlyr = canvas.GetComponentInChildren<VideoPlayer>();

        vidPlyr.clip = videoClip;
        vidPlyr.Prepare();
    }
    void PlayVideo(GameObject canvas)
    {
        vidPlyr = canvas.GetComponentInChildren<VideoPlayer>();

        vidPlyr.enabled = true;
        vidPlyr.clip = videoClip;
        vidPlyr.Prepare();

        vidPlyr.Play();
        img = canvas.GetComponentInChildren<RawImage>();
        img.enabled = true;
        
        var fader_t = new Task( FadeInRawImage(fadeAwaySpeed, img));
        //Cursor.visible = true;
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

/*    public IEnumerator FadeFromBlack(float fadespeed, float fadeDelay, bool fade = true)
    {
        Color objColor = blackoutPanel.GetComponent<Image>().color;
        float fadeAmount;
        yield return new WaitForSeconds(fadeDelay);
        
        while (objColor.a > 0)
        {
            fadeAmount = objColor.a - (fadespeed * Time.deltaTime);
            objColor = new Color(objColor.r, objColor.g, objColor.b, fadeAmount);
            blackoutPanel.GetComponent<Image>().color = objColor;
            yield return null;
        }
    }*/
    public IEnumerator FadeInRawImage(float fadespeed, RawImage img, bool fade = true)
    {
        Color objColor = img.color;
        float fadeAmount;
        //yield return new WaitForSeconds(fadeDelay);
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
