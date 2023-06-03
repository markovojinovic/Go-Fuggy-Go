using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkGenerator : MonoBehaviour
{

    public GameObject slowSharkPrefab;
    public GameObject fastSharkPrefab;
    public Camera mainCamera;

    private Vector3 bottomLeftCorner;
    private Vector3 bottomRightCorner;
    private const float FAST_SHARK_CHANCE = 0.06f;
    private int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObjectsPeriodically());
    }

    // Update is called once per frame
    void Update()
    {
        bottomLeftCorner = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        bottomLeftCorner.y -= 1f;

        // Calculate the bottom right corner in screen coordinates
        Vector3 bottomRightScreenPoint = new Vector3(Screen.width, 0f, mainCamera.nearClipPlane);
        // Convert the screen point to world coordinates
        bottomRightCorner = mainCamera.ScreenToWorldPoint(bottomRightScreenPoint);
        bottomRightCorner.y -= 1f;
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
                float x = Random.Range(bottomLeftCorner.x + (bottomRightCorner.x - bottomLeftCorner.x) * 0.2f, bottomRightCorner.x - (bottomRightCorner.x - bottomLeftCorner.x) * 0.2f);
                objectPosition = new Vector3(x, bottomLeftCorner.y);
                // Instantiate the object
                GameObject instantiatedShark;
                SharkController controller;
                if (Random.Range(0f, 1f) <= FAST_SHARK_CHANCE) { // Fast shark chance
                    instantiatedShark = Instantiate(fastSharkPrefab, objectPosition, Quaternion.identity);
                    controller = instantiatedShark.GetComponent<SharkController>();
                    controller.setVelocity(false);
                } else {
                    instantiatedShark = Instantiate(slowSharkPrefab, objectPosition, Quaternion.identity);
                    controller = instantiatedShark.GetComponent<SharkController>();
                    controller.setVelocity(true);
                }

                if (Random.Range(0f, 1f) > 0.5f) // orientation coinflip
                    controller.setInitialDirection(1f);
                else 
                    controller.setInitialDirection(-1f);

                controller.setShark(instantiatedShark);
            }

            // Wait for the specified interval before spawning the next object
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }
}
