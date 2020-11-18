using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private bool menuOn;
    public GameObject Menu;
    public GameObject Instructions;
    public GameObject ResumeText;
    private FirstPersonController movement;
    private bool switchText;
    void Start()
    {
        menuOn = false;
        switchText = false;
        player = GameObject.FindWithTag("Player");
        movement = player.GetComponent<FirstPersonController>();
        ToggleMenu();
        switchText = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
            
        }
    }

    public void ToggleMenu()
    {
        menuOn = !menuOn;

        movement.enabled = !menuOn;
        Cursor.visible = menuOn;
        Menu.SetActive(menuOn);
        Instructions.SetActive(false);
        if (switchText)
        {
            ResumeText.GetComponent<TextMeshProUGUI>().SetText("Resume Game");
            switchText = false;
        }
        if (!menuOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Cursor.visible = menuOn;
    }
    public bool isOn()
    {
        return menuOn;
    }
}
