using UnityEngine;

public class FuggyController : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(0f, 2f);   // The initial velocity to apply
    public Camera ourCamera;

    public float moveSpeed = 5f;  // Movement speed

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
    }

    private void Update(){

        if (Input.GetKey(KeyCode.LeftArrow))
            initialVelocity.x = -moveSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            initialVelocity.x = 0;
        else if (Input.GetKey(KeyCode.RightArrow))
            initialVelocity.x = moveSpeed;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            initialVelocity.x = 0;
        

        rb.velocity = initialVelocity;

        Debug.Log(initialVelocity);

        ourCamera.transform.position = new Vector3(0f, rb.transform.position.y, ourCamera.transform.position.z);
    }



    // private void OnTriggerEnter2D(Collider2D collision)
    // {

        
    //     // Check if the collision involves a specific tag
    //     if (collision.CompareTag("OutCollider"))
    //     {
    //         // Handle collision with the obstacle
    //         Debug.Log("Collision with obstacle!");
    //         // Add your custom code here to respond to the collision
    //     }
    // }


}