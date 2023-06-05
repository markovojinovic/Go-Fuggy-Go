using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkController : MonoBehaviour
{

    private float directionRight = 1f;
    private float velocity = 2f;
    private const float RIGHT_ORIENTED = -90f;
    private const float LEFT_ORIENTED = 90f;
    private GameObject shark;

    // private SpriteRenderer spriteRenderer;
    // private PolygonCollider2D sharkCollider;

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

    public void setInitialDirection(float dir) { 
        this.directionRight = dir;
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // sharkCollider = GetComponent<PolygonCollider2D>();

        if (dir < 0)
            flipShark();
        
    }

    public void changeDirection(float dir) { 
        this.directionRight = dir;
        flipShark();
    }

    private void flipShark() {
        // spriteRenderer.flipX = !spriteRenderer.flipX;
        // Vector2[] points = sharkCollider.GetPath(0);
        // System.Array.Reverse(points);
        // sharkCollider.SetPath(0, points);
        transform.Rotate(0, 180f, 0);
    }

    public void setShark(GameObject shark) { this.shark = shark; }

    public void setVelocity(bool slow) {
        if (slow) 
            velocity = Random.Range(1.2f, 2f);
        else
            velocity = Random.Range(2f, 2.7f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.gameObject.name == "TriggerUpper")
        {
            Destroy(this.shark);
        }
        else if (collision.collider.gameObject.name == "ColliderLeft")
        {
            this.changeDirection(1f);
        }
        else if (collision.collider.gameObject.name == "ColliderRight")
        {
            this.changeDirection(-1f);
        }
        else if (collision.collider.gameObject.name == "Fuggy")
        {
            FuggyController fuggyController = collision.collider.gameObject.GetComponent<FuggyController>();
            if (fuggyController.poisonAvailable) {
                Destroy(this.shark);
                fuggyController.stopPoison();
                fuggyController.poisonAvailable = false;
                fuggyController.startPoisonCountdown();
            } else {
                fuggyController.StopGame();
            }
        }
    }
}
