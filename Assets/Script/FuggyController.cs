using UnityEngine;

public class FuggyController : MonoBehaviour
{
    public Vector2 initialVelocity;
    public Camera ourCamera;
    public float moveSpeed = 5f;
    public float idleVerticalSpeed = -1f;

    private Rigidbody2D rb;
    private bool spaceEnabled = true;
    private float boundaryLeft;
    private float boundaryRight;
    private Vector3 cameraVelocity;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        initialVelocity = new Vector2(0f, idleVerticalSpeed);
        cameraVelocity = new Vector3(0f, idleVerticalSpeed);
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        Debug.Log(circleCollider.radius);

        setBoundaries();
    }

    private void Update(){

        if(Input.GetKeyDown(KeyCode.Space) && spaceEnabled){
            Debug.Log("Aimacija za napumpavanje");
        } else if(Input.GetKeyUp(KeyCode.Space) && spaceEnabled){
            Debug.Log("Aimacija za ispumpavanje");
            spaceEnabled = false;
            Invoke("EnableSpace", 5f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            initialVelocity.x = -moveSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            initialVelocity.x = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            initialVelocity.x = moveSpeed;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            initialVelocity.x = 0;


        Vector2 newFuggyPosition = rb.position + initialVelocity * Time.deltaTime;
        newFuggyPosition.x = Mathf.Clamp(newFuggyPosition.x, boundaryLeft, boundaryRight);
        rb.position = newFuggyPosition;

        ourCamera.transform.position += cameraVelocity * Time.deltaTime;
    }

    private void EnableSpace() {
        spaceEnabled = true;
    }

    private void setBoundaries() {
        Vector3 upperLeftCorner = ourCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, ourCamera.nearClipPlane));
        boundaryLeft = upperLeftCorner.x + 0.5f;

        Vector3 bottomRightCorner = ourCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, ourCamera.nearClipPlane));
        boundaryRight = bottomRightCorner.x - 0.5f;
    }
}