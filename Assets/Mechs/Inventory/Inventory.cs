using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    [SerializeField] public int matchesCount;

    public void AddToInv(int count) {
        matchesCount += count;
    }

    public void RemoveFromInv(int count) {
        if(matchesCount > 0) {
            matchesCount -= count;
        }
    }

}
