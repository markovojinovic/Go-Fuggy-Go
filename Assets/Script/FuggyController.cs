using UnityEngine;
using TMPro;

public class FuggyController : MonoBehaviour
{
    public enum FuggyState { DEPUMPED, PUMPED, IN_BUBBLE }

    public Vector2 fuggyVelocity;
    public Camera ourCamera;
    public float moveSpeed = 0;
    public float idleVerticalSpeed = -1.3f;
    public int score = 0;
    private int scoreIncrement = 1;
    private double timeScoreUpdate = 1f;
    private int changeBound = 10;
    private Animator animator;
    public RuntimeAnimatorController pumpAnimationController, depumpAnimationController;
    public GameObject gameOverText;

    public TextMeshProUGUI tmpText;

    private Rigidbody2D rb;
    private bool spaceEnabled = true;
    private float boundaryLeft;
    private float boundaryRight;
    private Vector3 cameraVelocity;
    private CircleCollider2D circleCollider;
    private FuggyState fuggyState = FuggyState.DEPUMPED;
    private GameObject bubbleGameObject;
    private const float MOVE_RIGHT = 5f;
    private const float MOVE_LEFT = -5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        pumpAnimationController = Resources.Load<RuntimeAnimatorController>("Assets/Animations/FuggyAnimationDepumpSprite_0.controller");
        depumpAnimationController = Resources.Load<RuntimeAnimatorController>("Assets/Animations/FuggyAnimationPumpSprite_0.controller");
        // animator.runtimeAnimatorController = pumpAnimationController;
        fuggyVelocity = new Vector2(0f, idleVerticalSpeed);
        cameraVelocity = new Vector3(0f, idleVerticalSpeed);
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        Debug.Log(circleCollider.radius);

        setBoundaries();

        Invoke("IncrementScore", (float)timeScoreUpdate);
    }

    private void Update(){

        tmpText.text = score.ToString();

        if(Input.GetKeyDown(KeyCode.Space) && spaceEnabled) {
            togglePumpState();
        }

        // if(Input.GetKeyDown(KeyCode.Space) && spaceEnabled){
        //     // animator.enabled = true;
        //     // animator.Play("FuggyPumpAnimation");
        //     fuggyState = FuggyState.PUMPED;
        //     Invoke("DisableSpace", 7f);
        // } else if(Input.GetKeyUp(KeyCode.Space) && spaceEnabled){
        //     // animator.runtimeAnimatorController = depumpAnimationController;
        //     // animator.Play("FuggyDepumpAnimation");
        //     CancelInvoke("DisableSpace");
        //     spaceEnabled = false;
        //     fuggyState = FuggyState.DEPUMPED;
        //     Invoke("EnableSpace", 5f);
        // }

        constructVelocities();
        Vector2 newFuggyPosition = rb.position + fuggyVelocity * Time.deltaTime;
        newFuggyPosition.x = Mathf.Clamp(newFuggyPosition.x, boundaryLeft, boundaryRight);
        rb.position = newFuggyPosition;
        if (bubbleGameObject)
            bubbleGameObject.transform.position = newFuggyPosition;

        ourCamera.transform.position += cameraVelocity * Time.deltaTime;
    }

    private void togglePumpState() {
        if (fuggyState == FuggyState.DEPUMPED) {
            fuggyState = FuggyState.PUMPED;
            //play animaciju
            spaceEnabled = false;
            Invoke("EnableSpace", 1.5f);
        } else 
        if (fuggyState == FuggyState.PUMPED) {
            fuggyState = FuggyState.DEPUMPED;
            //play animaciju
            spaceEnabled = false;
            Invoke("EnableSpace", 1.5f);
        }
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
                idleVerticalSpeed = -1.3f;
                break;
            case FuggyState.PUMPED:
                idleVerticalSpeed = -0.7f;
                break;
            case FuggyState.IN_BUBBLE:
                idleVerticalSpeed = 1f;
                break;
        }
        fuggyVelocity = new Vector2(moveSpeed, idleVerticalSpeed);
        cameraVelocity = new Vector3(0, idleVerticalSpeed);
    }

    private void EnableSpace() {
        spaceEnabled = true;
    }

    private void DisableSpace() {
        spaceEnabled = false;
        // idleVerticalSpeed = -1f;
        // fuggyVelocity = new Vector2(0f, idleVerticalSpeed);
        // cameraVelocity = new Vector3(0f, idleVerticalSpeed);
        Invoke("EnableSpace", 5f);
    }

    private void setBoundaries() {
        Vector3 upperLeftCorner = ourCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, ourCamera.nearClipPlane));
        boundaryLeft = upperLeftCorner.x + 0.5f;

        Vector3 bottomRightCorner = ourCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, ourCamera.nearClipPlane));
        boundaryRight = bottomRightCorner.x - 0.5f;
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

    public void StopRendering()
    {
        Invoke("quitGame", 2f);
        Instantiate(gameOverText, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    private void quitGame(){
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}