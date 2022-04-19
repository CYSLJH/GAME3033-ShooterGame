using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTable", menuName = "Items/ItemTable", order = 3)]
public class ItemTable : ScriptableObject
{
    public List<ItemScript> itemTable;

    public ItemScript getItemIndex(int index)
    {
        ItemScript itemToGet = itemTable[index];
        return itemToGet;
    }
}
