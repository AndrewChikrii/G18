using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelNest : MonoBehaviour
{

    [SerializeField] GameObject wheel;

    [SerializeField] float dist;

    private bool questDone;

    void FixedUpdate()
    {
        dist = Vector3.Distance(wheel.transform.position, transform.position);

        if (dist < 0.5f && !questDone)
        {
            questDone = true;

            wheel.transform.SetParent(transform);

            GameObject.Find("PlayerCamera").GetComponent<PRaycast>().CancelAction();
            wheel.transform.position = new Vector3(transform.position.x + 0.15f, transform.position.y, transform.position.z);
            Destroy(wheel.GetComponent<Grabbable>());
            Destroy(wheel.GetComponent<Rigidbody>());
            wheel.GetComponent<MeshCollider>().enabled = false;
            wheel.GetComponent<BoxCollider>().enabled = true;
            wheel.GetComponent<Rotatable>().enabled = true;

            wheel.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }
}
