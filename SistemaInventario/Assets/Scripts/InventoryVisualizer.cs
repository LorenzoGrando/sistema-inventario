using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryVisualizer : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public GameObject[] individualSlots;
    Canvas canvasRef;
    public GameObject heldItemIcon;
    public bool holdingItem = false;
    public Sprite[] allSprites;
    public GameObject[] tooltipTexts;

    GameObject tooltipBox;


    public void Start()
    {
        allSprites = Resources.LoadAll<Sprite>("Sprites");
    }
    public void Update()
    {
        if (inventoryManager.createdInventory == true)
        {
            UpdateInventory(inventoryManager.currentInventory, individualSlots);
        }
        if (holdingItem == true)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 actualMousePosition = new Vector3(mousePosition.x, mousePosition.y, -5.1f);
            heldItemIcon.transform.position = actualMousePosition;
        }

        if(inventoryManager.isHoveringSlot == true && tooltipBox != null && holdingItem == false)
        {
            tooltipBox.SetActive(true);
            Vector3 mousePosition2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 actualMousePosition2 = new Vector3(mousePosition2.x, mousePosition2.y, -5.1f);
            tooltipBox.transform.position = new Vector3(actualMousePosition2.x + tooltipBox.transform.localScale.x / 2, actualMousePosition2.y + tooltipBox.transform.localScale.y / 2, actualMousePosition2.z);
            UpdateTooltipBoxTextPositions();
            for(int i = 0; i < tooltipTexts.Length; i++)
            {
                tooltipTexts[i].SetActive(true);
            }
        }
        else if (inventoryManager.isHoveringSlot == false && tooltipBox != null && holdingItem == false)
        {
            tooltipBox.SetActive(false);
            for (int i = 0; i < tooltipTexts.Length; i++)
            {
                tooltipTexts[i].SetActive(false);
            }
        }

        else if (holdingItem == true)
        {
            tooltipBox.SetActive(false);
            for (int i = 0; i < tooltipTexts.Length; i++)
            {
                tooltipTexts[i].SetActive(false);
            }
        }

    }

    public void SetTextContents(int slotIndex)
    {
        tooltipTexts[0].GetComponent<Text>().text = inventoryManager.currentInventory.slots[slotIndex].nameItem;
        tooltipTexts[1].GetComponent<Text>().text = inventoryManager.currentInventory.slots[slotIndex].itemDescription;

        var reference = inventoryManager.currentInventory.slots[slotIndex];
        if (reference.GetType() == typeof(Consumable))
        {
            Consumable referenceC = (Consumable)reference;
            tooltipTexts[2].GetComponent<Text>().text = "Quantity: " + referenceC.quantity.ToString();
        }

        else
        {
            tooltipTexts[2].GetComponent<Text>().text = "Quantity: 1";
        }

    }
    void UpdateTooltipBoxTextPositions()
    {
        tooltipTexts[0].transform.position = Camera.main.WorldToScreenPoint(new Vector3(tooltipBox.transform.position.x - 1f, tooltipBox.transform.position.y + .5f));
        tooltipTexts[1].transform.position = Camera.main.WorldToScreenPoint(new Vector3(tooltipBox.transform.position.x - 1f, tooltipBox.transform.position.y + .2f));
        tooltipTexts[2].transform.position = Camera.main.WorldToScreenPoint(new Vector3(tooltipBox.transform.position.x - 1f, tooltipBox.transform.position.y - .9f));
    }
    public void CreateBoxes(Inventory createdInventory, int collumnAmount)
    {
        Font Arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        individualSlots = new GameObject[createdInventory.slots.Length];
        int rowAmount = 0;
        int xPos;
        for (int i = 0; i < createdInventory.slots.Length; i++)
        {
            GameObject slot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var rendererSlot = slot.GetComponent<MeshRenderer>();
            rendererSlot.material = (Material)Resources.Load("inventMaterial");
            slot.name = i.ToString();
            slot.transform.parent = transform;
            slot.layer = 6;
            individualSlots[i] = slot;

            //Cria o texto dos slots
            GameObject slotText = new GameObject(slot.name + " text.");

            Text currentSlotText = slotText.AddComponent<Text>();
            slotText.GetComponent<CanvasRenderer>();
            currentSlotText.font = Arial;
            currentSlotText.material = Arial.material;
            currentSlotText.fontSize = 20;
            //Seta o objeto para o layer UI;
            slotText.layer = 5;
            string currentText = "text";
            slotText.GetComponent<Text>().text = currentText;
            canvasRef = (Canvas)FindObjectOfType<Canvas>();
            GameObject objectCanvas = canvasRef.gameObject;
            slotText.transform.SetParent(objectCanvas.transform);
            RectTransform slotTextTransform = slotText.GetComponent<RectTransform>();



            if (i != 0)
            {
                xPos = i;
                if (i % collumnAmount == 0)
                {
                    rowAmount++;
                }

                if (xPos >= collumnAmount)
                {
                    xPos = xPos % collumnAmount;
                }

                Vector2 newPos = new Vector2(transform.position.x + xPos + (0.5f * xPos), transform.position.y - rowAmount * transform.localScale.y * 1.5f);
                slot.transform.position = newPos;

            }
            Vector3 slotPositionFactor = new Vector3(0.7f, -0.7f, 0);
            slotTextTransform.position = Camera.main.WorldToScreenPoint(slot.transform.position + slotPositionFactor);
            slotText.transform.SetParent(objectCanvas.transform);
            slotText.SetActive(false);

        }

        tooltipBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var tooltipRenderer = tooltipBox.GetComponent<MeshRenderer>();
        tooltipRenderer.material = (Material)Resources.Load("inventMaterial");
        tooltipBox.transform.localScale = new Vector3(3, 2, 0.3f);
        GameObject tooltipTitle = new GameObject("ItemName");
        GameObject tooltipDescription = new GameObject("ItemDesc");
        GameObject tooltipQuantity = new GameObject("ItemQTD");
        tooltipTexts = new GameObject[] { tooltipTitle, tooltipDescription, tooltipQuantity };

        for (int i = 0; i < 3; i++)
        {
            GameObject reference = tooltipTexts[i];

            Text tooltipObjectName = reference.AddComponent<Text>();
            RectTransform tooltipObjectTransform = tooltipObjectName.GetComponent<RectTransform>();
            tooltipObjectName.GetComponent<CanvasRenderer>();
            tooltipObjectName.font = Arial;
            tooltipObjectName.material = Arial.material;
            if (i > 0)
            {
                tooltipObjectName.fontSize = 20;
                tooltipObjectName.horizontalOverflow = HorizontalWrapMode.Overflow;
            }

            else
            {
                tooltipObjectName.fontSize = 25;
                tooltipObjectName.color = new Vector4(255, 0, 0, 1);
            }
            canvasRef = (Canvas)FindObjectOfType<Canvas>();
            Vector3 toolboxPositionFactor = new Vector3(tooltipBox.transform.localScale.x + 0.40f, -tooltipBox.transform.localScale.y - 0.5f, 0);
            tooltipObjectTransform.position = Camera.main.WorldToScreenPoint(tooltipBox.transform.position + toolboxPositionFactor);
            GameObject canvasObject = canvasRef.gameObject;
            reference.transform.SetParent(canvasObject.transform);
            tooltipTexts[i] = reference;
            tooltipTexts[i].SetActive(false);
        }

    }

    public void HoldItem(int slotIndex, bool itemIsHeld)
    {
        if (itemIsHeld)
        {
            heldItemIcon = individualSlots[slotIndex].transform.GetChild(0).gameObject;
            heldItemIcon =  Instantiate(heldItemIcon);
            holdingItem = true;
        }

        else if (!itemIsHeld)
        {
            Destroy(heldItemIcon);
            heldItemIcon = null;
            holdingItem = false;
        }
    }

    public void UpdateInventory(Inventory inventory, GameObject[] slots)
    {
        for(int i = 0; i < inventory.slots.Length; i++)
        {
            if(inventory.slots[i] == null)
            {
                if (slots[i].transform.childCount > 0)
                {
                    canvasRef.transform.GetChild(i).gameObject.SetActive(false);
                    RemoveItem(i);
                }
            }

            else
            {
                if (slots[i].transform.childCount > 0)
                {
                    if (inventory.slots[i].GetType() == typeof(Consumable))
                    {
                        Consumable consumableRef = (Consumable)inventory.slots[i];
                        string itemQuantity = "x" + consumableRef.quantity.ToString();
                        canvasRef.transform.GetChild(i).GetComponent<Text>().text = itemQuantity;
                        canvasRef.transform.GetChild(i).gameObject.SetActive(true);      
                    }
                    continue;
                }
                AddItem(i, inventory);
            }
        }
    }
    public void AddItem(int slotIndex, Inventory inventory)
    {
        var reference = inventory.slots[slotIndex];

            GameObject objectRef = Instantiate((GameObject)Resources.Load("ItemDefault"));
            objectRef.transform.parent = individualSlots[slotIndex].transform;
            objectRef.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            var renderer = objectRef.gameObject.GetComponent<SpriteRenderer>();

            for (int index = 0; index < allSprites.Length; index++)
            {
                if (allSprites[index].name == reference.spriteName)
                {
                    renderer.sprite = allSprites[index];
                    break;
                }
            }

            objectRef.transform.position = new Vector3(individualSlots[slotIndex].transform.position.x, individualSlots[slotIndex].transform.position.y, -5);
    }

    public void RemoveItem(int slotIndex)
    {
        Destroy(individualSlots[slotIndex].transform.GetChild(0).gameObject);
    }
}
