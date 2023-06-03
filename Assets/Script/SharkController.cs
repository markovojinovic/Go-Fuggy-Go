using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{

    private float directionRight = 1f;
    private float velocity = 2f;
    private const float RIGHT_ORIENTED = -90f;
    private const float LEFT_ORIENTED = 90f;

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

    public void setDirection(float dir) { 
        this.directionRight = dir;
        if (dir > 0f)
            transform.Rotate(0f, 0f, RIGHT_ORIENTED);
        else 
            transform.Rotate(0f, 0f, LEFT_ORIENTED);
    }
}
