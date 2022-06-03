using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoopable : MonoBehaviour, IActivatable
{
    [SerializeField] Item item;

    public void ActPrimary()
    {
        SC_FPSController.inventory.AddItem(item);
        Destroy(gameObject);
    }

    public void ActSecondary() { }
    public void Deact() { }
}
