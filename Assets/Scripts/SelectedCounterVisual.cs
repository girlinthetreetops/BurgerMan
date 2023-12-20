using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private Player player;
    private ClearCounter counterParent;

    [SerializeField] private GameObject selectedVisuals;

    private void Start()
    {
        //get a reference to the parent
        counterParent = GetComponentInParent<ClearCounter>();

        //Find the player
        player = FindObjectOfType<Player>();

        //Subscribe to their selection event 
        player.OnSelectedCounterChanged.AddListener(activateSelected);
    }

    private void activateSelected()
    {
        Debug.Log("I (the counter) has heard you, is it me tho?");

        if (player.selectedCounter == counterParent)
        {
            Debug.Log("Its me thats found, not the other counters!");
            ActivateChildren();
        } else
        {
            DeactivateChildren();
        }
    }

    // Function to activate child GameObjects
    public void ActivateChildren()
    {
        // Loop through each child of the current GameObject
        foreach (Transform child in transform)
        {
            // Set the child GameObject as active
            child.gameObject.SetActive(true);
        }
    }

    public void DeactivateChildren()
    {
        // Loop through each child of the current GameObject
        foreach (Transform child in transform)
        {
            // Set the child GameObject as inactive
            child.gameObject.SetActive(false);
        }
    }


    

}
 