using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoopable : MonoBehaviour, IActivatable
{
    [SerializeField] Item item;
    AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void ActPrimary()
    {
        SC_FPSController.inventory.AddItem(item);
        audio.Play();
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.transform.GetChild(0).gameObject);
        Destroy(gameObject, 2f);
    }

    public void ActSecondary() { }
    public void Deact() { }
}
