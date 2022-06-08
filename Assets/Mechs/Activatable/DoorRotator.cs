using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotator : MonoBehaviour
{
    float xrot;
    float yrot;
    float zrot;
    [SerializeField] float openX;
    [SerializeField] float openY;
    [SerializeField] float openZ;
    public float speed = 1f;

    void Start()
    {
        xrot = transform.localRotation.x;
        yrot = transform.localRotation.y;
        zrot = transform.localRotation.z;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(xrot, yrot, zrot), Time.deltaTime * speed);
    }

    public void SetRot(bool rot)
    {
        if (rot)
        {
            xrot += openX;
            yrot += openY;
            zrot += openZ;
        }
        else
        {
            xrot -= openX;
            yrot -= openY;
            zrot -= openZ;
        }
    }

}
