using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class InventoryItemController : MonoBehaviour
{
    public Item item;
    private PlayerMovement playerMovement; // Reference to the player's movement script
    private float originalJumpHeight; // To store the original jump height
    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure your player GameObject is tagged as 'Player'.");
        }
        // Find the player's movement script
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
    }

    public void RemoveItem()
    {
        InventoryManager.Instance.Remove(item);
        Destroy(gameObject);
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        Debug.Log("Item added: " + item.itemName);
    }

    public void UseItem()
    {
        switch (item.itemType)
        {
            case Item.ItemType.JumpPotion:
                Debug.Log("Potion used");
                if (playerMovement != null)
                {
                    ApplyJumpBoost(); // Apply the jump boost
                }
                RemoveItem(); // Remove the item from inventory
                break;

            case Item.ItemType.SightPotion:
                Debug.Log("Potion used"); 
                UseSightPotion();
                RemoveItem();
                break;   

            case Item.ItemType.TeleportationPotion:
                TeleportPlayer();
                RemoveItem();
                break;

            case Item.ItemType.Ingredient:
                Debug.Log("Ingredient");
                break;

            default:
                break;
        }
    }

    private async void ApplyJumpBoost()
    {
        if (playerMovement == null) return;

        // Store the original jump height and apply the boost
        originalJumpHeight = playerMovement.jumpPower;
        playerMovement.jumpPower *= 2f;
        Debug.Log($"Jump height increased to {playerMovement.jumpPower}");

        await Task.Delay(30000);
        ResetJumpHeight();
    }

    private void ResetJumpHeight()
    {
        if (playerMovement != null)
        {
            // Reset the jump height to its original value
            playerMovement.jumpPower = originalJumpHeight;
            Debug.Log("Jump height reset to normal.");
        }
    }


private void UseSightPotion()
{
    Debug.Log("Sight Potion activated!");

    // Find all objects with the "Wall" tag
    GameObject[] walls = GameObject.FindGameObjectsWithTag("PotionWall");

    foreach (var wall in walls)
    {
        Renderer renderer = wall.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Enable transparency in material
            Material material = renderer.material;
            if (material.HasProperty("_Color"))
            {
                Color color = material.color;
                color.a = 0.3f; // Semi-transparent
                material.color = color;

                // Set rendering mode to transparent
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
            }
        }

        Collider collider = wall.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false; // Disable collision
        }
    }

    Debug.Log($"Processed {walls.Length} walls.");
}

private void TeleportPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player is not assigned!");
            return;
        }

        // Find the teleport destination by tag
        GameObject destination = GameObject.FindGameObjectWithTag("TeleportPoint");

        if (destination == null)
        {
            Debug.LogError("Teleport destination with tag 'TeleportPoint' not found!");
            return;
        }

        // Teleport the player to the destination's position
        player.position = destination.transform.position;

        Debug.Log($"Player teleported to {destination.transform.position}");
    }

}
