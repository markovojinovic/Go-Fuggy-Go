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

    private SpriteRenderer spriteRenderer;

    public GameObject gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (dir < 0)
            spriteRenderer.flipX = !spriteRenderer.flipX;
        
    }

    public void changeDirection(float dir) { 
        this.directionRight = dir;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        // if (dir > 0f)
        //     transform.Rotate(0f, 0f, 2*RIGHT_ORIENTED);
        // else 
        //     transform.Rotate(0f, 0f, 2*LEFT_ORIENTED);
    }

    public void setShark(GameObject shark) { this.shark = shark; }

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
            StopRendering();
        }
    }

    private void StopRendering()
    {
        Invoke("quitGame", 5f);
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
