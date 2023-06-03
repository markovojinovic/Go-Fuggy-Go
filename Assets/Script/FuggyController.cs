using UnityEngine;

public class FuggyController : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(0f, 2f);   // The initial velocity to apply
    public Camera camera;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;
        camera.transform.position = new Vector3(0f, rb.transform.position.y, -10);
    }

    private void Update(){
        camera.transform.position = new Vector3(0f, rb.transform.position.y, -10);
    }

}