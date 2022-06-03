using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuse : MonoBehaviour
{

    [SerializeField] GameObject fuse;

    [SerializeField] float dist;

    private bool questDone;

    void FixedUpdate()
    {
        dist = Vector3.Distance(fuse.transform.position, transform.position);

        if (dist < 0.5f && !questDone)
        {
            questDone = true;

            fuse.transform.SetParent(transform);

            GameObject.Find("PlayerCamera").GetComponent<PRaycast>().CancelAction();
            fuse.transform.position = new Vector3(transform.position.x + 0.20f, transform.position.y, transform.position.z);
            Destroy(fuse.GetComponent<Grabbable>());
            Destroy(fuse.GetComponent<Rigidbody>());

            fuse.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            this.enabled = false;
        }
    }
}
