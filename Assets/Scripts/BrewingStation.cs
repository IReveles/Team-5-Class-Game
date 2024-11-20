using UnityEngine;
using UnityEngine.UI;

public class BrewingStation : MonoBehaviour
{
    public GameObject interactionPromptUI; // Assign in the inspector (UI Text or Canvas Group)
    public GameObject brewingUI; // The brewing UI Panel that we will enable/disable

    private bool isPlayerInRange = false;

    private void Start()
    {
        interactionPromptUI.SetActive(false);
        brewingUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionPromptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPromptUI.SetActive(false);
            brewingUI.SetActive(false); // Hide the brewing UI when player leaves
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            brewingUI.SetActive(!brewingUI.activeSelf); // Toggle UI
            interactionPromptUI.SetActive(!brewingUI.activeSelf); // Hide prompt when brewing UI is active
        }
    }
}
