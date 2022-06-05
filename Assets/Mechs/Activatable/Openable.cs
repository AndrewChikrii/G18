using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Openable : MonoBehaviour, IActivatable
{
    [SerializeField] GameObject rotator;
    [SerializeField] bool opened;
    [SerializeField] bool freezed;
    [SerializeField] bool locked;
    [SerializeField] Item key;
    [SerializeField] float freezeTime = 1f;

    HintDisplay hintDisplay;

    void Start()
    {
        hintDisplay = GameObject.Find("Canvas").GetComponent<HintDisplay>();
        if (opened)
        {
            Open();
        }
    }

    public void ActPrimary()
    {
        if (freezed) return;
        rotator.GetComponent<DoorRotator>().speed = 1f;
        if (!locked)
        {
            OpenOrClose();
            return;
        }

        Item doorKey = SC_FPSController.inventory.GetItemList().Find(item => item.name == key?.name);
        if (key && doorKey)
        {
            locked = false;
            hintDisplay.interaction.text = "Door unlocked";
            StartCoroutine(hintDisplay.HintCoroutine());
            return;
        }
        hintDisplay.interaction.text = "Door is locked";
        StartCoroutine(hintDisplay.HintCoroutine());
    }
    public void ActSecondary()
    {
        if (freezed) return;
        rotator.GetComponent<DoorRotator>().speed = 3f;
        if (!locked)
        {
            OpenOrClose();
            return;
        }
        hintDisplay.interaction.text = "Door is locked";
        StartCoroutine(hintDisplay.HintCoroutine());
    }
    public void Deact() { }

    void Open()
    {
        StartCoroutine(Freeze());
        if (!opened)
        {
            opened = !opened;
        }
        rotator.GetComponent<DoorRotator>().SetRot(true);
    }
    void Close()
    {
        StartCoroutine(Freeze());
        opened = !opened;
        rotator.GetComponent<DoorRotator>().SetRot(false);
    }

    IEnumerator Freeze()
    {
        freezed = true;
        yield return new WaitForSeconds(freezeTime);
        freezed = false;
    }

    void OpenOrClose()
    {
        if (!opened)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
}