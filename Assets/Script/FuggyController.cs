using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FuggyController : MonoBehaviour
{
    public enum FuggyState { DEPUMPED, PUMPED, IN_BUBBLE, DEAD }

    private const float MOVE_RIGHT = 5f;
    private const float MOVE_LEFT = -5f;
    private const float DEPUMPED_VELOCITY = -1.5f;
    private const float PUMPED_VELOCITY = -0.9f;
    private const float IN_BUBBLE_VELOCITY = 1.1f;
    private const float DEAD_VELOCITY = 0;
    private const float DEPUMPED_COLLIDER_RADIUS = 1.367377f;
    private const float PUMPED_COLLIDER_RADIUS = 3.248377f;

    public Vector2 fuggyVelocity;
    public Camera ourCamera;
    public float moveSpeed = 0;
    public float verticalVelocity = -1.5f;
    public int score = 0;
    private int scoreIncrement = 1;
    private double timeScoreUpdate = 1f;
    private int changeBound = 10;
    private Animator animator;
    public GameObject gameOverText;
    private int jellyCount = 0;
    public TextMeshProUGUI jellyCountText;

    public TextMeshProUGUI tmpText;
    public TextMeshProUGUI poisonCooldownTimeText;
    public TextMeshProUGUI pumpTimeText;

    private int poisonCooldownTime = 0;
    public bool poisonAvailable = false;
    private int pumpTime = 0;

    private Rigidbody2D rb;
    private bool spaceEnabled = true;
    private float boundaryLeft;
    private float boundaryRight;
    private Vector3 cameraVelocity;
    private CircleCollider2D fuggyCircleCollider;
    private FuggyState fuggyState = FuggyState.DEPUMPED;
    private GameObject bubbleGameObject;
    private ParticleSystem poison;

    private void Start() {
        animator = GetComponent<Animator>();
        fuggyCircleCollider = GetComponent<CircleCollider2D>();
        poison = GetComponentInChildren<ParticleSystem>();
        poison.Stop();
        animator.enabled = true;
        rb = GetComponent<Rigidbody2D>();
        setBoundaries();
        Invoke("IncrementScore", (float)timeScoreUpdate);
    }

    private void Update() {

        tmpText.text = score.ToString();
        poisonCooldownTimeText.text = (poisonCooldownTime.ToString() + "s");
        pumpTimeText.text = (pumpTime.ToString() + "s");
        jellyCountText.text = jellyCount.ToString() + "x";

        if(Input.GetKeyDown(KeyCode.Space) && spaceEnabled) {
            togglePumpState();
        }

        constructVelocities();
        Vector2 newFuggyPosition = rb.position + fuggyVelocity * Time.deltaTime;
        newFuggyPosition.x = Mathf.Clamp(newFuggyPosition.x, boundaryLeft, boundaryRight);
        rb.position = newFuggyPosition;
        if (bubbleGameObject)
            bubbleGameObject.transform.position = newFuggyPosition;

        ourCamera.transform.position += cameraVelocity * Time.deltaTime;
    }

    private IEnumerator pumpCountdown() {   
        pumpTime = 10;
        while (fuggyState == FuggyState.PUMPED && pumpTime > 0)
        {   
            pumpTime--;
            // Wait for the specified interval
            yield return new WaitForSeconds(1f);
        }
        if (pumpTime == 0) {
            togglePumpState();
        }
        pumpTime = 0;
    }

    public void startPoisonCountdown() { StartCoroutine(poisonCountdown()); }
    private IEnumerator poisonCountdown() {   
        poisonCooldownTime = 25;
        while (poisonCooldownTime > 0)
        {   
            poisonCooldownTime--;
            // Wait for the specified interval
            yield return new WaitForSeconds(1f);
        }
        poisonAvailable = true;
    }

    public void stopPoison() {
        if (poisonAvailable)
            poison.Stop();
    }

    private void togglePumpState() {
        if (fuggyState == FuggyState.DEPUMPED) {
            fuggyState = FuggyState.PUMPED;

            // Play pump animation
            animator.SetBool("pumpItUp", true);
            animator.SetBool("dePump", false);

            if (!poisonAvailable && poisonCooldownTime == 0)
                poisonAvailable = true; 
            if (poisonAvailable)
                poison.Play();

            StartCoroutine(pumpCountdown());
            spaceEnabled = false;
            Invoke("UpdateFuggyCollider", 0.5f);
            Invoke("EnableSpace", 1.5f);
        } else 
        if (fuggyState == FuggyState.PUMPED) {
            fuggyState = FuggyState.DEPUMPED;
            
            // Play depump animation
            animator.SetBool("pumpItUp", false);
            animator.SetBool("dePump", true);

            if (poisonAvailable)
                poison.Stop();
            poisonAvailable = false;

            spaceEnabled = false;
            Invoke("UpdateFuggyCollider", 0.5f);
            Invoke("EnableSpace", 1.5f);
        }
    }

    private void EnableSpace() {
        spaceEnabled = true;
    }

    private void UpdateFuggyCollider() {
        if (fuggyState == FuggyState.PUMPED)
            fuggyCircleCollider.radius = PUMPED_COLLIDER_RADIUS;
        else if (fuggyState == FuggyState.DEPUMPED)
            fuggyCircleCollider.radius = DEPUMPED_COLLIDER_RADIUS;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Jelly"))
           jellyCount++;
    }

    private void constructVelocities() {
        if (Input.GetKey(KeyCode.LeftArrow))
            moveSpeed = MOVE_LEFT;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            moveSpeed = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            moveSpeed = MOVE_RIGHT;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            moveSpeed = 0;

        switch (fuggyState) {
            case FuggyState.DEPUMPED:
                verticalVelocity = DEPUMPED_VELOCITY;
                break;
            case FuggyState.PUMPED:
                verticalVelocity = PUMPED_VELOCITY;
                break;
            case FuggyState.IN_BUBBLE:
                verticalVelocity = IN_BUBBLE_VELOCITY;
                break;
            case FuggyState.DEAD:
                verticalVelocity = DEAD_VELOCITY;
                break;
        }
        fuggyVelocity = new Vector2(moveSpeed, verticalVelocity);
        cameraVelocity = new Vector3(0, verticalVelocity);
    }

    private void setBoundaries() {
        Vector3 upperLeftCorner = ourCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, ourCamera.nearClipPlane));
        boundaryLeft = upperLeftCorner.x + 0.5f;

        Vector3 bottomRightCorner = ourCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, ourCamera.nearClipPlane));
        boundaryRight = bottomRightCorner.x - 0.5f;
    }
    
    private void IncrementScore() {
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

    private void resetBubbleState() { 
        this.fuggyState = FuggyState.DEPUMPED;
        Destroy(this.bubbleGameObject);
        this.bubbleGameObject = null; 
    }

    public FuggyState getFuggyState() { return this.fuggyState; }

    public void setFuggyStateInBubble(GameObject bubble) {
        fuggyState = FuggyState.IN_BUBBLE;
        bubbleGameObject = bubble;
        bubbleGameObject.transform.position = rb.position;
        Invoke("resetBubbleState", Random.Range(3f, 4f));
    }

    public void StopGame() {
        Invoke("quitGame", 2f);
        fuggyState = FuggyState.DEAD;
        Instantiate(gameOverText, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    private void quitGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}