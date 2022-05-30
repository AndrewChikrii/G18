using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour, IActivatable
{
    [SerializeField] bool isRotating = false;
    [SerializeField] bool solved = false;
    [SerializeField] float rotationAngle = 50f;
    [SerializeField] float rotationCounter = 0f;
    [SerializeField] float maxRotation = 720f;
    public void ActPrimary()
    {
        isRotating = true;
        //Debug.Log("Rotating a " + gameObject.name);
    }
    public void ActSecondary() {}
    public void Deact()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            if (rotationCounter >= maxRotation)
            {
                isRotating = false;
                solved = true;
                //Debug.Log("Rot is max");
            }
            else
            {
                transform.Rotate(0f, 0f, -rotationAngle * Time.deltaTime);
                rotationCounter += rotationAngle * Time.deltaTime;
            }
        }
        else {
            if(rotationCounter > 0 && !solved) {
                transform.Rotate(0f, 0f, rotationAngle * Time.deltaTime);
                rotationCounter -= rotationAngle * 3f * Time.deltaTime;
            }
        }
    }

    public float ReturnRot() {
        return rotationCounter / 720f;
    }
}
