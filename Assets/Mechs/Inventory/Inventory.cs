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

        }
        else
        {
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
    }

    public int? MatchesCount()
    {
        return itemList.Find(item => item.name == "Matches")?.amount;
    }

    public void RemoveItem(Item item)
    {
        Item searchedItem = itemList.Find(i => i.name == item.name);
        searchedItem.amount -= 1;
        if (searchedItem.amount == 0 && !searchedItem.isStackable)
        {
            itemList.Remove(searchedItem);
        }
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
