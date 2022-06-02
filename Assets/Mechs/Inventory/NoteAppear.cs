using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour, IActivatable
{

    [SerializeField] private Image note;

    public void ActPrimary()
    {
        note.enabled = true;
    }

    public void ActSecondary() { }

    public void Update()
    {
        if (Input.GetKey("escape") || Input.GetKey("mouse 1") || Input.GetKey("space"))
        {
            note.enabled = false;
        }
    }
    public void Deact() { }
}