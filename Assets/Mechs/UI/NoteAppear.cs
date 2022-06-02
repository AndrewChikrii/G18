using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour, IActivatable
{

    [SerializeField] private GameObject note;

    public void ActPrimary()
    {
        note.SetActive(true);
        PauseMenu.Pause();
    }

    public void ActSecondary() { }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            note.SetActive(false);
        }
    }
    public void Deact() { }
}