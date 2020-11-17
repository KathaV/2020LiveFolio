using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject Panel;
    // Start is called before the first frame update
    
    public void TogglePanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            print("state: "+isActive);
            Panel.SetActive(!isActive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
