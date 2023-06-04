using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorGenerator : MonoBehaviour
{

    public GameObject anchorPrefab;
    public Camera mainCamera;

    private Vector3 upperLeftCorner;
    private Vector3 upperRightCorner;
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjectsPeriodically());
    }

    // Update is called once per frame
    void Update()
    {
        upperLeftCorner = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, mainCamera.nearClipPlane));
        upperLeftCorner.y += 3f;

        // Calculate the bottom right corner in screen coordinates
        Vector3 bottomRightScreenPoint = new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane);
        // Convert the screen point to world coordinates
        upperRightCorner = mainCamera.ScreenToWorldPoint(bottomRightScreenPoint);
        upperRightCorner.y += 3f;
    }

    private IEnumerator SpawnObjectsPeriodically()
    {   
        Vector3 objectPosition;
        while (true)
        {   
            if (i == 0) { // To skip first cycle of generation
                i++;
            }
            else {
                float x = Random.Range(upperLeftCorner.x + (upperRightCorner.x - upperLeftCorner.x) * 0.15f, upperRightCorner.x - (upperRightCorner.x - upperLeftCorner.x) * 0.15f);
                objectPosition = new Vector3(x, upperLeftCorner.y);
                // Instantiate the object
                GameObject instantiatedAnchor;
                instantiatedAnchor = Instantiate(anchorPrefab, objectPosition, Quaternion.identity);
            }

            // Wait for the specified interval before spawning the next object
            yield return new WaitForSeconds(Random.Range(25f, 30f));
        }
    }
}
