using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    [SerializeField] float burnTime = 2f;

    void Start()
    {
        Destroy(gameObject, burnTime);
    }


}
