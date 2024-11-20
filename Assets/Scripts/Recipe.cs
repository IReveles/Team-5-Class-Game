using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Brewing/Recipe")]
public class Recipe : ScriptableObject
{
    public List<Item> requiredItems; // List of items needed for this recipe
    public Item result;              // The resulting item after brewing
}
