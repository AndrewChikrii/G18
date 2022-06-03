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
    }
    public void ActSecondary() { }
    public void Deact()
    {
        isRotating = false;
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            if (rotationCounter < maxRotation)
            {
                transform.Rotate(0f, 0f, -rotationAngle * Time.deltaTime);
                rotationCounter += rotationAngle * Time.deltaTime;
                return;
            }
            isRotating = false;
            solved = true;
            this.enabled = false;
        }
        else
        {
            if (rotationCounter > 0 && !solved)
            {
                transform.Rotate(0f, 0f, rotationAngle * Time.deltaTime);
                rotationCounter -= rotationAngle * 3f * Time.deltaTime;
            }
        }
    }

    public float ReturnRot()
    {
        return rotationCounter / 720f;
    }
}
