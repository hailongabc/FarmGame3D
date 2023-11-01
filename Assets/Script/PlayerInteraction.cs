using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerMovement2 playerMovement2;

    //The land the player is currently selecting
    Land selectedLand;

    //The interactable object the player is currently selecting
    InteractableObject selectedInteractable = null;

    void Start()
    {
        playerMovement2 = transform.parent.GetComponent<PlayerMovement2>();
    }

    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }
    }

    //Handles what happens when the interaction raycast hits something interactable
    private void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        //CHeck if the player is going to interact with land
        if(other.tag == "Land")
        {
            //Get the land component
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        //Check if the player is going to interact with an Item
        if(other.tag == "Item")
        {
            //Set the interactable to the currently selected interactable
            InteractableObject interactable = other.GetComponent<InteractableObject>();
            return;
        }

        //Deselect the interactable if the player is not standing on anything at the moment
        if(selectedInteractable != null)
        {
            selectedInteractable = null;
        }

        //Unselect the land if the player is not standing on any land at the moment
        if(selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null; 
        }
    }

    //Handles the selection process
    void SelectLand(Land land)
    {
        //Set the previously selected land to false (if any)
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }

        //Set the new selected land to the land we're selecting no w
        selectedLand = land;
        land.Select(true);
    }

    //Triggered when the player presses the tool button
    public void Interact()
    {
        //Check if the player is selecting any land
        if(selectedLand != null)
        {
            selectedLand.Interact();
            return;
        }

        Debug.Log("Not on any land");
    }

    //Triggered when the player presses the item interact button
    public void ItemInteract()
    {
        //If the player is holding something, keep it in his inventory
        if(InventoryManager.Instance.equippedItem != null)
        {
            InventoryManager.Instance.HandToInventory(InventorySlots.InventoryType.Item);
            return;
        }

        //If the player isn't holding anything, pick up an item
        
        //Check if there is an interactable selected
        if (selectedInteractable != null)
        {
            Debug.Log("dddd");
            //Pick it up
            selectedInteractable.Pickup();
        }
    }
}
