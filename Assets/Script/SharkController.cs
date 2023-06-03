using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{

    private float directionRight = 1f;
    private float velocity = 2f;
    private const float RIGHT_ORIENTED = -90f;
    private const float LEFT_ORIENTED = 90f;
    private GameObject shark;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = new Vector3(directionRight, 0f);

        Vector3 newPosition = transform.position + movementDirection * velocity * Time.deltaTime;

        transform.position = newPosition;
    }

    public void setInitialDirection(float dir) { 
        this.directionRight = dir;
        if (dir > 0f)
            transform.Rotate(0f, 0f, -RIGHT_ORIENTED);
        else 
            transform.Rotate(0f, 0f, -LEFT_ORIENTED);
    }

    public void changeDirection(float dir) { 
        this.directionRight = dir;
        if (dir > 0f)
            transform.Rotate(0f, 0f, 2*RIGHT_ORIENTED);
        else 
            transform.Rotate(0f, 0f, 2*LEFT_ORIENTED);
    }

    public void setShark(GameObject shark) { this.shark = shark; }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "TriggerUpper")
        {
            Destroy(this.shark);
        }
        else if (collider.gameObject.name == "ColliderLeft")
        {
            this.changeDirection(1f);
        }
        else if (collider.gameObject.name == "ColliderRight")
        {
            this.changeDirection(-1f);
        }
    }
}
