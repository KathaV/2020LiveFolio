using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [Header("Door Object")]
    [SerializeField] private Animator myDoor = null;

    
    [Header("Animation Conditions")]
    [SerializeField] private string isOpen = "isOpen";
   


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag =="Player")
        {
            print("enter:" + isOpen);
           myDoor.SetBool(isOpen, true);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("exit:" +isOpen); 
           myDoor.SetBool(isOpen, false);
            
        }
    }
}
