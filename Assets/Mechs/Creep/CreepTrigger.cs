using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepTrigger : MonoBehaviour
{
    Transform playerT;
    CreepController creep;
    [SerializeField] float dist;
    [SerializeField] float triggerDist = 4f;

    void Start()
    {
        playerT = GameObject.Find("Player").transform;
        creep = GameObject.Find("Creep").GetComponent<CreepController>();
        creep.freezed = true;
    }

    
    void FixedUpdate()
    {
        dist = Vector3.Distance(playerT.position, transform.position);
        if(dist <= triggerDist)
        {
            creep.freezed = false;
        }
    }
}
