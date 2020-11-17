using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private bool menuOn;
    public GameObject Menu;
    public GameObject Instructions;
    private FirstPersonController movement;
    void Start()
    {
        menuOn = false;
        player = GameObject.FindWithTag("Player");
        movement = player.GetComponent<FirstPersonController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuOn = !menuOn;
            
            movement.enabled = !menuOn;
            Cursor.visible = menuOn;
            Menu.SetActive(menuOn);
            Instructions.SetActive(false);
            
        }
    }
}
