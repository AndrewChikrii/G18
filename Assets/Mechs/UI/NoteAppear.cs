using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteAppear : MonoBehaviour, IActivatable
{
    [SerializeField] private GameObject note;

    AudioSource audio;

    void Start() 
    {
        audio = GetComponent<AudioSource>();
    }

    public void ActPrimary()
    {
        note.SetActive(true);
        audio.Play();
        PauseMenu.Pause();
    }

    public void ActSecondary() { }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            note.SetActive(false);
            audio.Play();
        }
    }
    public void Deact() { }
}