using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorController : MonoBehaviour
{
    private Vector3 velocity = new Vector3(0f, -4f);
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.name == "TriggerLower")
        {
            Destroy(this.gameObject);
        }
        else if (collision.collider.CompareTag("Shark"))
        {
            Destroy(collision.collider.gameObject);
        }
        else if (collision.collider.gameObject.name == "Fuggy")
        {
            FuggyController fuggyController = collision.collider.gameObject.GetComponent<FuggyController>();
            // fuggyController.StopRendering();
        }
    }
}
