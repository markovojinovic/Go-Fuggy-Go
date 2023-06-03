using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{

    private float directionRight = 1f;
    private int velocity = 2;

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
}
