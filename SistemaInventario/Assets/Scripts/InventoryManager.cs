using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory currentInventory;
    public InventoryVisualizer inventoryVisualizer;
    public LayerMask slotsLayer;
    Item heldItem;
    bool isHoldingItem;
    public bool createdInventory = false;
    public bool isHoveringSlot = false;


    [Range(1, 15)]
    public int numSlots;

    [Range(3, 6)]
    public int amountPerRow;

    void Start()
    {
        slotsLayer = LayerMask.GetMask("Slots");
    }

    void Update()
    {
        if (createdInventory == false && Input.GetKeyDown(KeyCode.I))
        {
            CreateInventory();
            createdInventory = true;
        }

        if(createdInventory == true) {

            DetectMouseHover();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                DebugQuantity(currentInventory);
            }

            if (!inventoryVisualizer.holdingItem)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    AddItem(currentInventory, "Apple", "A red fruit", "apple", Consumable.ConsumableType.Apple, Equipment.EquipmentType.Null);
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    AddItem(currentInventory, "Pear", "A green fruit", "pear", Consumable.ConsumableType.Pear, Equipment.EquipmentType.Null);
                }

                if (Input.GetKeyDown(KeyCode.O))
                {
                    AddItem(currentInventory, "Orange", "An orange fruit", "orange", Consumable.ConsumableType.Orange, Equipment.EquipmentType.Null);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    AddItem(currentInventory, "Sword", "A sharp weapon", "sword", Consumable.ConsumableType.Null, Equipment.EquipmentType.Sword);
                }

                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    RemoveItem(currentInventory, 0, false);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (isHoldingItem == false)
                {
                    Vector3 mousePos = Input.mousePosition;

                    Ray ray = Camera.main.ScreenPointToRay(mousePos);
                    Debug.Log("Ray cast");
                    if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit rayHit, 200, slotsLayer))
                    {
                        GameObject slotReference = rayHit.transform.gameObject;
                        int slotIndex = Convert.ToInt32(slotReference.gameObject.name);
                        if (currentInventory.slots[slotIndex] != null)
                        {
                            heldItem = currentInventory.slots[slotIndex];
                            isHoldingItem = true;
                            inventoryVisualizer.HoldItem(slotIndex, isHoldingItem);
                            currentInventory.slots[slotIndex] = null;
                        }

                    }
                }

                else if (isHoldingItem == true)
                {
                    Vector3 mousePos = Input.mousePosition;

                    Ray ray = Camera.main.ScreenPointToRay(mousePos);
                    Debug.Log("Ray cast");
                    if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit rayHit, 200, slotsLayer))
                    {
                        GameObject slotReference = rayHit.transform.gameObject;
                        int slotIndex = Convert.ToInt32(slotReference.gameObject.name);
                        if (currentInventory.slots[slotIndex] == null)
                        {
                            currentInventory.slots[slotIndex] = heldItem;
                            isHoldingItem = false;
                            inventoryVisualizer.HoldItem(slotIndex, isHoldingItem);
                            heldItem = null;

                        }
                    }
                }

            }


            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Input.mousePosition;

                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                Debug.Log("Ray cast");
                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit rayHit, 200, slotsLayer))
                {
                    Debug.Log("Ray hit");
                    GameObject slotReference = rayHit.transform.gameObject;
                    int slotIndex = Convert.ToInt32(slotReference.gameObject.name);
                    RemoveItem(currentInventory, slotIndex, true);
                }
            }
        }
    }

        void CreateInventory()
        {
            currentInventory = new Inventory(numSlots, "Inventário1");
            inventoryVisualizer.CreateBoxes(currentInventory, amountPerRow);
            Debug.Log("Created Inventory of name: " + currentInventory.invName + " and of size: " + currentInventory.size);
        }

        void AddItem(Inventory currentInventory, string nameItem, string itemDescription, string sprite, Consumable.ConsumableType Ctype, Equipment.EquipmentType Etype)
        {
            bool itemWasAdded = false;
        if (Ctype != Consumable.ConsumableType.Null)
        {
            for (int i = 0; i < currentInventory.slots.Length; i++)
            {

                if (currentInventory.slots[i] != null && currentInventory.slots[i].GetType() == typeof(Consumable))
                {
                    Consumable referenceC = (Consumable)currentInventory.slots[i];
                    if (referenceC.type == Ctype)
                    {
                        referenceC.quantity++;
                        currentInventory.slots[i] = referenceC;
                        itemWasAdded = true;
                        break;
                    }
                }
            }
        }

        if (itemWasAdded == false)
        {
            for (int j = 0; j < currentInventory.slots.Length; j++)
            {
                if (currentInventory.slots[j] == null)
                {
                    if (Ctype != Consumable.ConsumableType.Null)
                    {
                        currentInventory.slots[j] = new Consumable(1, nameItem, itemDescription, sprite, Ctype);
                        break;
                    }

                    else if (Etype != Equipment.EquipmentType.Null)
                    {
                        currentInventory.slots[j] = new Equipment(10, nameItem, itemDescription, sprite, Etype);
                        break;
                    }
                }
            }
        }   

        }

        void RemoveItem(Inventory currentInventory, int slotIndex, bool useIndex)
        {
            bool indexWasChanged = false;
            for (int i = currentInventory.slots.Length - 1; i >= 0; i--)
            {
                if (useIndex == true)
                {
                    i = slotIndex + 1;
                    useIndex = false;
                    indexWasChanged = true;
                    Debug.Log("Index changed");
                    continue;
                }
                if (currentInventory.slots[i] != null)
                {
                    if (currentInventory.slots[i].GetType() == typeof(Consumable))
                    {
                        Consumable referenceC = (Consumable)currentInventory.slots[i];
                        if (referenceC.quantity <= 1)
                        {
                            currentInventory.slots[i] = null;
                            break;
                        }

                        else
                        {
                            referenceC.quantity--;
                            currentInventory.slots[i] = referenceC;
                            break;
                        }
                    }
                    currentInventory.slots[i] = null;
                    break;
                }

                if (indexWasChanged)
                {
                    break;
                }
            }
        }

        void DebugQuantity(Inventory currentInventory)
        {
            for (int i = 0; i < currentInventory.slots.Length; i++)
            {
                if (currentInventory.slots[i] != null)
                {
                    var reference = currentInventory.slots[i];
                    Consumable referenceC = (Consumable)reference;

                    Debug.Log("Item Name: " + referenceC.nameItem + " has " + referenceC.quantity + " items");
                }
            }
        }

    void DetectMouseHover()
    {
        if (isHoldingItem == false)
        {
            Vector3 mousePos = Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit rayHit, 200, slotsLayer))
            {
                GameObject slotReference = rayHit.transform.gameObject;
                int slotIndex = Convert.ToInt32(slotReference.gameObject.name);
                if (currentInventory.slots[slotIndex] != null)
                {
                    inventoryVisualizer.SetTextContents(slotIndex);
                    isHoveringSlot = true;
                }
            }

            else
            {
                isHoveringSlot = false;
            }
        }
    }
}
