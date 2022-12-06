using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public int quantity;
    public enum ConsumableType {Null, Apple, Pear, Orange};
    public ConsumableType type;

    public int Quantity { get { return quantity; } set { quantity = value; } }

    public Consumable(int quantity, string nameItem, string itemDescription, string spriteName, ConsumableType consumableType)
    {
        // Herdados de Item
        this.nameItem = nameItem;
        this.itemDescription = itemDescription;
        this.spriteName = spriteName;

        this.quantity = quantity;
        this.type = consumableType;
    }
}

public class Equipment : Item
{
    public int durability;

    public enum EquipmentType {Null, Sword, Armor};
    public EquipmentType type;

    public Equipment(int durability, string nameItem, string itemDescription, string spriteName, Equipment.EquipmentType type)
    {
        //Herdados de Item
        this.nameItem = nameItem;
        this.itemDescription = itemDescription;
        this.spriteName = spriteName;

        this.durability = durability;
        this.type = type;
        
    }
}
