using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformController : MonoBehaviour
{
    public static PlayerTransformController instance;
    public enum MyTranform
    {
        Run, Walking, Skating, Cycling, Driving
    }
    public MyTranform currTransform = MyTranform.Run;
    public List<GameObject> VehiclesListInPlayer;
    [System.Serializable]
    public class MotorProps
    {
        [Header("** Motor Properties **")]
        public Transform BodyTarget;
        public Transform LeftHandTarget;
        public Transform RightHandTarget;
        public Transform LeftFootTarget;
        public Transform RightFootTarget;
        public float MotorSpeed;
    }
    [System.Serializable]
    public class BikeProps
    {
        [Header("** Bike Properties **")]
        public Transform BodyTarget;
        public Transform LeftHandTarget;
        public Transform RightHandTarget;
        public Transform LeftFootTarget;
        public Transform RightFootTarget;
        public float BikeSpeed;
    }
    [System.Serializable]
    public class SkateProps
    {
        [Header("** Skate Properties **")]
        public float SkateSpeed;
    }

    public BikeProps bikeProps;
    public MotorProps motorProps;
    public SkateProps skateProps;
}
