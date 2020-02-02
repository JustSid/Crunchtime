using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertZone : MonoBehaviour
{
    public PickupInsert insert;

    void Awake()
    {
        if(insert)
            insert.insertZone = this;
    }
}
