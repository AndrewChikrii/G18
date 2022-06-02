using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        item.isStackable = !item.isStackable;
        if (!item.isStackable)
        {
            foreach (Item invItem in itemList)
            {
                if (invItem.name == item.name)
                {
                    invItem.amount += item.amount;
                }
                else
                {
                    itemList.Add(item);
                }
            }
        }
        else
        {
            itemList.Add(item);
        }
    }

    public int? MatchesCount()
    {
        Item matches = itemList.Find(item => item.name == "Matches");
        return matches?.amount;
    }

    public void RemoveItem(Item item)
    {
        Item searchedItem = itemList.Find(i => i.name == item.name);
        searchedItem.amount -= 1;
        if (searchedItem.amount == 0)
        {
            itemList.Remove(searchedItem);
        }
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
