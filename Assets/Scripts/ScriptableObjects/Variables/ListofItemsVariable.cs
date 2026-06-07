using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/List of Items")]
public class ListOfItemsVaiable: ScriptableObject
{
    public List<Item> items;
}