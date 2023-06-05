using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject backGorund; 
    private float size;

    // Start is called before the first frame update
    void Start()
    {
        size = backGorund.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(transform.position.y - mainCamera.transform.position.y) >= 15f)
            transform.position = new Vector3(transform.position.x, backGorund.transform.position.y - size, transform.position.z);
    }
}
