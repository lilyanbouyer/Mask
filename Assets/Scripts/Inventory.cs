using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    private bool FirstKey = false;

    public void unlockFirstKey(){
        Debug.Log("Got Key");
        FirstKey = true;
    }

    public bool hasFirstKey(){
        return FirstKey;
    }
}
