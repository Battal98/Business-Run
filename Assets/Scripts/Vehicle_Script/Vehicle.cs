using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public enum VehicleType
    {
        None, Skate, Bike, Motor
    }
    public VehicleType Vtype = VehicleType.None;
    private void Start()
    {
        if (Vtype == VehicleType.None)
        {
            Debug.LogWarning("Pls Assign a Vehicle Type");
            return;
        }
    }
}
