using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDoor : MonoBehaviour
{
    [SerializeField] GameObject keyWheel;
    Rotatable rot;
    void Start()
    {
        rot = keyWheel.GetComponent<Rotatable>();
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, 3f * rot.ReturnRot() + 1.6f, transform.position.z);
    }
}
