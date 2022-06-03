using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFresnel : MonoBehaviour
{
    GameObject player;
    Renderer rend;
    float dist;

    void Start()
    {
        player = GameObject.Find("Player");
        rend = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist < 5f)
        {
            rend.sharedMaterial.SetFloat("_Fresnel_mult", (5f - dist) / 5f);
        }
    }
}
