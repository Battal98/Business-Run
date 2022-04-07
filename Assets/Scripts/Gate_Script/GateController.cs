using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GateController : MonoBehaviour
{
    public enum GateType
    {
        None, SkateGate, BikeGate, MotorGate
    }
    public GateType type = GateType.None;
    [SerializeField] GameObject VehiclesParent;
    public List<GameObject> Vehicles = new List<GameObject>();
    public int GatePrice;
    [SerializeField] TextMeshProUGUI PriceText;
    public Image PriceTextBackground;
    void Start()
    {
        if (type == GateType.None)
        {
            Debug.LogWarning("Pls Assign a Gate Type");
            return;
        }
        for (int i = 0; i < VehiclesParent.transform.childCount; i++)
        {
            Vehicles.Add(VehiclesParent.transform.GetChild(i).gameObject);
        }
        PriceText.text = GatePrice.ToString();
        PriceTextBackground.color = Color.red;
        if (type == GateType.BikeGate)
        {
            CheckStandVehicle(Vehicle.VehicleType.Bike);
        }
        if (type == GateType.SkateGate)
        {
            CheckStandVehicle(Vehicle.VehicleType.Skate);
        }
        if (type == GateType.MotorGate)
        {
            CheckStandVehicle(Vehicle.VehicleType.Motor);
        }
    }

    /// <summary>
    /// The Vehicle List has been checked and 
    /// the correct Vehicles have been activated in the stand.
    /// </summary>
    /// <param name="type"> Vehicle.VehicleType Vtype </param>
    void CheckStandVehicle(Vehicle.VehicleType type)
    {
        for (int i = 0; i < Vehicles.Count; i++)
        {
            Vehicle _vehicle = Vehicles[i].gameObject.GetComponent<Vehicle>();
            if (_vehicle.Vtype == type)
            {
                Vehicles[i].gameObject.SetActive(true);
            }
            else
            {
                Vehicles[i].gameObject.SetActive(false);
            }
        }
    }
}
