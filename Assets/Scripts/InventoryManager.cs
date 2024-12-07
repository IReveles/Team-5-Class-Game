using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public Transform ItemContent;
    public GameObject InventoryItem;

    public InventoryItemController[] InventoryItems;

    private void Awake(){
        if (Instance == null)
    {
        Instance = this;
    }
    else
    {
        Destroy(gameObject);
    }
    }

    public bool HasItem(Item item)
    {
        return Items.Contains(item);
    }

    public void Add(Item item){
        Items.Add(item);
    }

    public void Remove(Item item){
        Items.Remove(item);
    }

    public void ListItems(){
        foreach (var item in Items) {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
        
            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }

        SetInventoryItems();
    }


    public void SetInventoryItems(){

        InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();

        for(int i = 0; i < Items.Count; i++){
            if (InventoryItems.Length > i)
        {
            // Ensure AddItem is only called for uninitialized items
            if (InventoryItems[i].item == null)  // Check if the item is already assigned
            {
                InventoryItems[i].AddItem(Items[i]);
            }
        }
        else
        {
            Debug.LogWarning("InventoryItemController not available for item at index " + i);
        }
        }

    }

     public void ClearContent()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
    }
}
