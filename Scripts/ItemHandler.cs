using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour
{
    [Tooltip("The name of this Item")]
    public string itemName = "";
    [Tooltip("The item's inventory slot")]
    public GameObject itemSlot = null;
    [Tooltip("The item's sprite")]
    public Sprite itemSprite = null;

}
