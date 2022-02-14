using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Openable : MonoBehaviour, IActivatable
{
    [SerializeField] GameObject rotator;
    [SerializeField] bool opened;
    [SerializeField] bool freezed;
    [SerializeField] bool locked;
    
    public void ActPrimary() {
        if(!locked && !freezed) {
            if(!opened) {
                Open();
            } else {
                Close();
            }
        } 
        else if(locked) {
            Debug.Log("Door is locked");
        }
    }
    public void ActSecondary() {}
    public void Deact() {
        
    }

    void Open() {
        StartCoroutine(Freeze());
        opened = !opened;
        rotator.GetComponent<DoorRotator>().SetRot(true);
    }
    void Close() {
        StartCoroutine(Freeze());
        opened = !opened;
        rotator.GetComponent<DoorRotator>().SetRot(false);
    }

    IEnumerator Freeze() {
        freezed = true;
        yield return new WaitForSeconds(1f);
        freezed = false;
    }

}
/*
    assign to obj with collider! door panel etc
    inside of rotator
*/