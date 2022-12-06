using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trabalho de Lorenzo Grando e Paloma
[System.Serializable]
public class Inventory
{
    public Item[] slots;
    public int size;
    public string invName;

    public Inventory(int size, string invName)
    {
        this.size = size;
        this.invName = invName;
        slots = new Item[size];
    }
}
