using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeBehaviour : MonoBehaviour
{
    private Animator doorAnim;
    public bool isByFridge;

    [SerializeField] public UIManager uiManager;

    private void Start()
    {
        doorAnim = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isByFridge && !doorAnim.GetBool("isDoorOpen"))
        {
            openDoor();

        } else if (Input.GetKeyDown(KeyCode.Space) && isByFridge && doorAnim.GetBool("isDoorOpen"))
        {
            closeDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("PlayerTrigger"))
        {
            isByFridge = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isByFridge = false;
    }

    private void openDoor()
    {
        doorAnim.SetBool("isDoorOpen", true);
    }

    private void closeDoor()
    {
        doorAnim.SetBool("isDoorOpen", false);
    }

    public void updateConsoleText()
    {
        uiManager.SetConsoleText("Congratulations");
    }
}
