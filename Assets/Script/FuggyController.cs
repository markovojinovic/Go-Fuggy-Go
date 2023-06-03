using UnityEngine;
using TMPro;

public class FuggyController : MonoBehaviour
{
    public Vector2 initialVelocity = new Vector2(0f, 2f);
    public Camera ourCamera;
    public float moveSpeed = 5f;
    public int score = 0;
    private int scoreIncrement = 1;
    private double timeScoreUpdate = 1f;
    private int changeBound = 10;

    public TextMeshProUGUI tmpText;

    private Rigidbody2D rb;
    private bool spaceEnabled = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialVelocity;

        Invoke("IncrementScore", (float)timeScoreUpdate);
    }

    private void Update(){

        tmpText.text = score.ToString();

        if(Input.GetKeyDown(KeyCode.Space) && spaceEnabled){
            Debug.Log("Aimacija za napumpavanje");
        }else if(Input.GetKeyUp(KeyCode.Space) && spaceEnabled){
            Debug.Log("Aimacija za ispumpavanje");
            spaceEnabled = false;
            Invoke("EnableSpace", 5f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
            initialVelocity.x = -moveSpeed;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            initialVelocity.x = 0;
        else if (Input.GetKey(KeyCode.RightArrow))
            initialVelocity.x = moveSpeed;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            initialVelocity.x = 0;
        

        rb.velocity = initialVelocity;

        ourCamera.transform.position = new Vector3(0f, rb.transform.position.y, ourCamera.transform.position.z);
    }

    private void EnableSpace(){
        spaceEnabled = true;
    }

    private void IncrementScore()
    {
        score += scoreIncrement;
        if(timeScoreUpdate < 0.4 && score % changeBound == 0){
            scoreIncrement += 1;
            timeScoreUpdate += 0.3;
            changeBound += 50;
        }
        else if(score % changeBound == 0)
            timeScoreUpdate -= 0.05;

        Invoke("IncrementScore", (float)timeScoreUpdate);
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