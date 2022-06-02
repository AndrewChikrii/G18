using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Key,
        Consumable
    }
    public new string name;
    public ItemType itemType;
    public string description;
    public int amount = 1;
    public bool isStackable = false;
}