using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    public Ability ability;

    public enum Ability
    {

        WalkRight, WalkLeft, Jump, Pickup
    }
}
