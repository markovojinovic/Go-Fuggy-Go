using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JellyFishController : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fuggy") || collision.gameObject.CompareTag("Anchor"))
        {
            Destroy(gameObject);
        }
    }
}
