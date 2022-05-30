using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    [SerializeField] float burnTime = 2f;

    RaycastHit surfaceHit;

    void Start()
    {
        Destroy(gameObject, burnTime);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Water")
        {
            Destroy(gameObject);
        }
    }
}