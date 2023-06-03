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
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 movementDirection = new Vector3(horizontalInput, 0f);

        // Calculate the object's new position
        Vector3 newPosition = transform.position + movementDirection * moveSpeed * Time.deltaTime;

        // Update the object's position
        transform.position = newPosition;


        ourCamera.transform.position = new Vector3(0f, rb.transform.position.y, ourCamera.transform.position.z);
    }

}