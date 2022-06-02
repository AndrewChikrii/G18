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

    public void ActSecondary()
    {
        // Input.GetKey(KeyCode)
        note.enabled = false;
    }

    public void Deact() { }
}