using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour, IActivatable
{
    [SerializeField] GameObject rotator;
    [SerializeField] bool opened;
    [SerializeField] bool freezed;
    [SerializeField] bool locked;
    [SerializeField] Item key;
    [SerializeField] float freezeTime = 1f;

    void Start()
    {
        if (opened)
        {
            Open();
        }
    }

    public void ActPrimary()
    {
        if (freezed) return;
        if (!locked)
        {
            OpenOrClose();
            return;
        }

        Item doorKey = SC_FPSController.inventory.GetItemList().Find(item => item.name == key?.name);
        if (key && doorKey)
        {
            OpenOrClose();
            return;
        }
        Debug.Log("Door is locked");
    }
    public void ActSecondary() { }
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