using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        var itemInstance = GameObject.Instantiate(item);
        if (!itemInstance.isStackable || itemList.Count == 0)
        {
            itemList.Add(itemInstance);
            return;
        }

        foreach (Item invItem in itemList)
        {
            if (invItem.name == itemInstance.name)
            {
                invItem.amount += itemInstance.amount;
            }
            else
            {
                itemList.Add(itemInstance);
            }
        }
    }

    public int? MatchesCount()
    {
        return itemList.Find(item => item.name == "Matches")?.amount;
    }

    public void RemoveItem(Item item)
    {
        Item searchedItem = itemList.Find(i => i.name == item.name);
        searchedItem.amount -= 1;
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}