using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoopable : MonoBehaviour, IActivatable
{
    [SerializeField] int count = 6;

    Inventory inv;

    void Start() {
        inv = GameObject.Find("Player").GetComponent<Inventory>();
    }

    public void ActPrimary() {
        inv.AddToInv(count);
        Destroy(gameObject);
    }

    public void ActSecondary() {}
    public void Deact() {}
}
