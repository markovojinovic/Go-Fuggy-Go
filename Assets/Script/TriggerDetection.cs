using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fuggy"))
        {
            FuggyController fuggyController = other.gameObject.GetComponent<FuggyController>();
            fuggyController.setFuggyStateInBubble(this.gameObject);
        } 
        else if (other.gameObject.name == "TriggerUpper")
        {
            Destroy(this.gameObject);
        }
    }
}
