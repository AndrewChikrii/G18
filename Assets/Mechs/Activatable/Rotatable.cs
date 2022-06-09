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

    AudioSource audio;

    void Start() 
    {
        audio = GetComponent<AudioSource>();
    }

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
            audio.volume =  Mathf.Lerp(audio.volume, 0.6f, 2f * Time.deltaTime);
            if (rotationCounter < maxRotation)
            {
                transform.Rotate(0f, 0f, -rotationAngle * Time.deltaTime);
                rotationCounter += rotationAngle * Time.deltaTime;
                return;
            }
            audio.volume = 0f;
            isRotating = false;
            solved = true;
            this.enabled = false;
        }
        else
        {
            audio.volume = Mathf.Lerp(audio.volume, 0f, 1f * Time.deltaTime);
            if (rotationCounter > 0 && !solved)
            {
                transform.Rotate(0f, 0f, rotationAngle * 3f * Time.deltaTime);
                rotationCounter -= rotationAngle * 3f * Time.deltaTime;
            }
        }
    }

    public float ReturnRot()
    {
        return rotationCounter / 720f;
    }
}
