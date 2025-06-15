using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory
{
    int gold;
    public int Gold { 
        get => gold;

        set { 
            gold += value;
        } 
    }
    //List<IItem> items;
}
