using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotator : MonoBehaviour
{
    [SerializeField] float yrot;
    [SerializeField] float rotClosed = 0f;
    [SerializeField] float rotOpened = 110f;
    [SerializeField] float speed;
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, yrot, transform.rotation.z), Time.deltaTime * speed);
    }

    public void SetRot(bool rot) {
        if(rot) {
            yrot = rotOpened;
        } else {
            yrot = rotClosed;
        }
    }

}
