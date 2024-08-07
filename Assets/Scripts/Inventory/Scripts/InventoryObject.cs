using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Object", menuName = "ScriptableObject / inventoryObject", order = 2)]
public class InventoryObject : ScriptableObject
{
    public Sprite Sprite;
    public string name;
    public string type;
    public string description;
}
