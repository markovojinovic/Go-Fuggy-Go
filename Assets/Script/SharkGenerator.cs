using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkGenerator : MonoBehaviour
{

    public GameObject sharkPrefab;
    public Camera mainCamera;

    private Vector3 bottomLeftCorner;
    private Vector3 bottomRightCorner;
    private float spawnInterval = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjectsPeriodically());
    }

    // Update is called once per frame
    void Update()
    {
        bottomLeftCorner = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        
        // Debug.Log("Bottom Left Corner: " + bottomLeftCorner);

        // Calculate the bottom right corner in screen coordinates
        Vector3 bottomRightScreenPoint = new Vector3(Screen.width, 0f, mainCamera.nearClipPlane);
        // Convert the screen point to world coordinates
        bottomRightCorner = mainCamera.ScreenToWorldPoint(bottomRightScreenPoint);

        // Debug.Log("Bottom Right Corner: " + bottomRightCorner);
    }

    private IEnumerator SpawnObjectsPeriodically()
    {   
        Vector3 objectPosition;
        while (true)
        {   
            if (Random.Range(0f, 1f) > 0.5f) {
                objectPosition = new Vector3(bottomLeftCorner.x, bottomLeftCorner.y);
                // Instantiate the object
                GameObject instantiatedShark = Instantiate(sharkPrefab, objectPosition, Quaternion.identity);
                SharkController controller = instantiatedShark.GetComponent<SharkController>();
                controller.setDirection(1f);
            }
            else {
                objectPosition = new Vector3(bottomRightCorner.x, bottomRightCorner.y);
                // Instantiate the object
                GameObject instantiatedShark = Instantiate(sharkPrefab, objectPosition, Quaternion.identity);
                SharkController controller = instantiatedShark.GetComponent<SharkController>();
                controller.setDirection(-1f);
            }

            // Wait for the specified interval before spawning the next object
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
