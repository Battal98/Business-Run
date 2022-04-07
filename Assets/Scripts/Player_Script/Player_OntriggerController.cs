using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RootMotion.FinalIK;
public class Player_OntriggerController : MonoBehaviour
{
    private PlayerTransformController transformController;
    private Rigidbody rb;
    private float humanSpeed;
    private void Start()
    {
        humanSpeed = GameManager.instance.PlayerForwardSpeed;
        transformController = this.gameObject.GetComponent<PlayerTransformController>();
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Obstacle":
                ObstacleCheck(other.gameObject);
                break;
            case "Collectable":
                CollectableCheck(other.gameObject);
                break;
            case "Gate":
                GateCheck(other.gameObject);
                break;
            case "Finish":
                FinishCheck(other.gameObject);
                break;
            default:
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Obstacle":
                OnTriggerExitFromWorker(other.gameObject);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// It tells me what will happen when I hit the obstacle.
    /// </summary>
    /// <param name="_triggerObj"> Holds the object I hit. </param>
    private void ObstacleCheck(GameObject _triggerObj)
    {
        ObstacleController obsController = _triggerObj.GetComponent<ObstacleController>();
        if (obsController == null)
        {
            return;
        }

        if (obsController.ObsType == ObstacleController.ObstacleType.Barrier)
        {
            PlayerForward.instance.transform.DOMoveZ(PlayerForward.instance.transform.position.z - obsController.barrier.recoil, obsController.barrier.recoilDuration);

            //player transformunda herhangi bir arac varsa yok edilecek. !!
            if (transformController.currTransform != PlayerTransformController.MyTranform.Run)
            {
                transformController.currTransform = PlayerTransformController.MyTranform.Run;
                for (int i = 0; i < transformController.VehiclesListInPlayer.Count; i++)
                {
                    transformController.VehiclesListInPlayer[i].gameObject.SetActive(false);
                }
                DisableFinalIK();
            }
        }
        if (obsController.ObsType == ObstacleController.ObstacleType.Worker)
        {
            //player transformunda herhangi bir arac varsa yok edilecek. !!
            if (transformController.currTransform != PlayerTransformController.MyTranform.Run)
            {
                transformController.currTransform = PlayerTransformController.MyTranform.Run;
                for (int i = 0; i < transformController.VehiclesListInPlayer.Count; i++)
                {
                    transformController.VehiclesListInPlayer[i].gameObject.SetActive(false);
                }
                DisableFinalIK();
            }
            GameManager.instance.PlayerForwardSpeed = obsController.worker.slowEffect;
        }
        if (obsController.ObsType == ObstacleController.ObstacleType.Hole)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            GameManager.instance.vCamGame.m_Follow = null;
            StartCoroutine(WaitForFinish());
        }
    }

    /// <summary>
    /// Operates according to the type of collectible objects.
    /// </summary>
    /// <param name="_triggerObj"> Holds the triggered collectible. </param>
    private void CollectableCheck (GameObject _triggerObj)
    {
        CollectableController CollController = _triggerObj.GetComponent<CollectableController>();
        if (CollController == null)
        {
            return;
        }
        if (CollController.type == CollectableController.CollectableType.Coin)
        {
            GameManager.instance.currCoin += CollController.CoinCost;
            UIManager.instance.currCoinText.text = GameManager.instance.currCoin.ToString();
            Destroy(_triggerObj);
            for (int i = 0; i < LevelManager.instance.GateLists.Count; i++)
            {
                if(GameManager.instance.currCoin >= LevelManager.instance.GateLists[i].GetComponent<GateController>().GatePrice)
                {
                    Debug.Log("Active Compare");
                    LevelManager.instance.GateLists[i].GetComponent<GateController>().PriceTextBackground.color = Color.green;
                }
            }
            return;
        }
        if (CollController.type == CollectableController.CollectableType.Time)
        {
            if (CollController.mode == CollectableController.CollectableMode.Bad)
            {
                LevelManager.instance.timer -= CollController.TimeValue;
                UIManager.instance.timerFill.fillAmount -= (1f/ LevelManager.instance.LevelTime) * CollController.TimeValue;
                Destroy(_triggerObj);
                return;
            }
            if (CollController.mode == CollectableController.CollectableMode.Good)
            {
                LevelManager.instance.timer += CollController.TimeValue;
                UIManager.instance.timerFill.fillAmount += (1f / LevelManager.instance.LevelTime) * CollController.TimeValue;
                Destroy(_triggerObj);
                return;
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_triggerObj"></param>
    private void GateCheck(GameObject _triggerObj)
    {
        GateController _gateController = _triggerObj.GetComponent<GateController>();
        if (GameManager.instance.currCoin >= _gateController.GatePrice)
        {

            if (_gateController.type == GateController.GateType.None)
            {
                return;
            }

            if (_gateController.type == GateController.GateType.SkateGate)
            {
                if (transformController.currTransform == PlayerTransformController.MyTranform.Skating)
                {
                    Debug.Log("U re already Skating..");
                    return;
                }

                #region Gate Canvas Jobs
                for (int i = 0; i < LevelManager.instance.GateLists.Count; i++)
                {
                    if (GameManager.instance.currCoin < LevelManager.instance.GateLists[i].GetComponent<GateController>().GatePrice)
                    {
                        LevelManager.instance.GateLists[i].GetComponent<GateController>().PriceTextBackground.color = Color.red;
                    }
                } //Checking Gate list Canvas 
                GameManager.instance.currCoin -= _gateController.GatePrice;
                UIManager.instance.currCoinText.text = GameManager.instance.currCoin.ToString();
                #endregion

                //Add Coin Particle

                GameManager.instance.PlayerForwardSpeed = transformController.skateProps.SkateSpeed;
                for (int i = 0; i < _gateController.Vehicles.Count; i++)
                {
                    if (_gateController.Vehicles[i].GetComponent<Vehicle>().Vtype == Vehicle.VehicleType.Skate)
                    {
                        _gateController.Vehicles[i].gameObject.SetActive(false);
                    }
                } // Disable Stand Stake
                for (int i = 0; i < transformController.VehiclesListInPlayer.Count; i++) 
                {
                    if (transformController.VehiclesListInPlayer[i].GetComponent<Vehicle>().Vtype == Vehicle.VehicleType.Skate)
                    {
                        transformController.VehiclesListInPlayer[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        transformController.VehiclesListInPlayer[i].gameObject.SetActive(false);
                    }
                } //Enable Player Skate
                transformController.currTransform = PlayerTransformController.MyTranform.Skating;
                DisableFinalIK();
                PlayerController.instance.playerAnim.SetBool("Running", false);
                PlayerController.instance.playerAnim.SetBool("Skating",true);
                return;
            }

            if (_gateController.type == GateController.GateType.BikeGate)
            {
                if (transformController.currTransform == PlayerTransformController.MyTranform.Cycling)
                {
                    Debug.Log("U re already Cycling..");
                    return;
                }

                #region Gate Canvas Jobs
                for (int i = 0; i < LevelManager.instance.GateLists.Count; i++)
                {
                    if (GameManager.instance.currCoin < LevelManager.instance.GateLists[i].GetComponent<GateController>().GatePrice)
                    {
                        LevelManager.instance.GateLists[i].GetComponent<GateController>().PriceTextBackground.color = Color.red;
                    }
                } //Checking Gate list Canvas 
                GameManager.instance.currCoin -= _gateController.GatePrice;
                UIManager.instance.currCoinText.text = GameManager.instance.currCoin.ToString();
                #endregion

                //Add Puf Particle

                PlayerController.instance.playerAnim.SetBool("Skating", false);
                GameManager.instance.PlayerForwardSpeed = transformController.bikeProps.BikeSpeed;
                for (int i = 0; i < _gateController.Vehicles.Count; i++)
                {
                    if (_gateController.Vehicles[i].GetComponent<Vehicle>().Vtype == Vehicle.VehicleType.Bike)
                    {
                        _gateController.Vehicles[i].gameObject.SetActive(false);
                    }
                }// Disable Stand Bike
                for (int i = 0; i < transformController.VehiclesListInPlayer.Count; i++)
                {
                    if (transformController.VehiclesListInPlayer[i].GetComponent<Vehicle>().Vtype == Vehicle.VehicleType.Bike)
                    {
                        transformController.VehiclesListInPlayer[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        transformController.VehiclesListInPlayer[i].gameObject.SetActive(false);
                    }
                } //Enable Player Bike
                transformController.currTransform = PlayerTransformController.MyTranform.Cycling;

                #region Final IK Jobs
                FullBodyBipedIK bipedIK = this.gameObject.GetComponentInChildren<FullBodyBipedIK>();
                bipedIK.solver.bodyEffector.target = transformController.bikeProps.BodyTarget;
                bipedIK.solver.leftHandEffector.target = transformController.bikeProps.LeftHandTarget;
                bipedIK.solver.rightHandEffector.target = transformController.bikeProps.RightHandTarget;
                bipedIK.solver.leftFootEffector.target = transformController.bikeProps.LeftFootTarget;
                bipedIK.solver.rightFootEffector.target = transformController.bikeProps.RightFootTarget;
                bipedIK.enabled = true;
                #endregion

                return;
            }

            if (_gateController.type == GateController.GateType.MotorGate)
            {
                Debug.Log("Active Gate");
                if (transformController.currTransform == PlayerTransformController.MyTranform.Driving)
                {
                    Debug.Log("U re already Driving..");
                    return;
                }

                #region Gate Canvas Jobs
                for (int i = 0; i < LevelManager.instance.GateLists.Count; i++)
                {
                    if (GameManager.instance.currCoin < LevelManager.instance.GateLists[i].GetComponent<GateController>().GatePrice)
                    {
                        LevelManager.instance.GateLists[i].GetComponent<GateController>().PriceTextBackground.color = Color.red;
                    }
                } //Checking Gate list Canvas 
                GameManager.instance.currCoin -= _gateController.GatePrice;
                UIManager.instance.currCoinText.text = GameManager.instance.currCoin.ToString();
                #endregion

                PlayerController.instance.playerAnim.SetBool("Skating", false);
                GameManager.instance.PlayerForwardSpeed = transformController.motorProps.MotorSpeed;
                transformController.currTransform = PlayerTransformController.MyTranform.Driving;
                for (int i = 0; i < _gateController.Vehicles.Count; i++)
                {
                    if (_gateController.Vehicles[i].GetComponent<Vehicle>().Vtype == Vehicle.VehicleType.Motor)
                    {
                        _gateController.Vehicles[i].gameObject.SetActive(false);
                    }
                }// Disable Stand Motor
                for (int i = 0; i < transformController.VehiclesListInPlayer.Count; i++)
                {
                    if (transformController.VehiclesListInPlayer[i].GetComponent<Vehicle>().Vtype == Vehicle.VehicleType.Motor)
                    {
                        transformController.VehiclesListInPlayer[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        transformController.VehiclesListInPlayer[i].gameObject.SetActive(false);
                    }
                } //Enable Player Motor

                //Add Puf Particle

                #region Final IK Jobs
                FullBodyBipedIK bipedIK = this.gameObject.GetComponentInChildren<FullBodyBipedIK>();
                bipedIK.solver.bodyEffector.target = transformController.motorProps.BodyTarget;
                bipedIK.solver.leftHandEffector.target = transformController.motorProps.LeftHandTarget;
                bipedIK.solver.rightHandEffector.target = transformController.motorProps.RightHandTarget;
                bipedIK.solver.leftFootEffector.target = transformController.motorProps.LeftFootTarget;
                bipedIK.solver.rightFootEffector.target = transformController.motorProps.RightFootTarget;
                bipedIK.enabled = true;
                #endregion

                return;
            }
            else
            {
                DisableFinalIK();
            }
        }
    }

    private void FinishCheck(GameObject _triggerObj)
    {
        GameManager.instance.OnLevelEnded();
        DisableFinalIK();
        this.gameObject.GetComponent<PlayerTransformController>().currTransform = PlayerTransformController.MyTranform.Walking;
        for (int i = 0; i < transformController.VehiclesListInPlayer.Count; i++)
        {
            if (transformController.VehiclesListInPlayer[i].gameObject.activeInHierarchy) 
            {
                transformController.VehiclesListInPlayer[i].gameObject.SetActive(false);
            }
        } //Disable All Vehicles in Player
        PlayerController.instance.playerAnim.SetBool("Running", false);
        PlayerController.instance.playerAnim.SetBool("Skating", false);
        PlayerController.instance.playerAnim.SetBool("Walking", true);
        PlayerForward.instance.transform.DOMoveZ(_triggerObj.transform.GetChild(0).transform.position.z, 3f).OnComplete(() =>
        GameManager.instance.OnLevelCompleted()
         
        );
    }

    void DisableFinalIK ()
    {
        FullBodyBipedIK bipedIK = this.gameObject.GetComponentInChildren<FullBodyBipedIK>();
        bipedIK.enabled = false;
    }

    private void OnTriggerExitFromWorker(GameObject _triggerObj)
    {
        ObstacleController obsController = _triggerObj.GetComponent<ObstacleController>();
        if (obsController.ObsType == ObstacleController.ObstacleType.Worker)
        {
            GameManager.instance.PlayerForwardSpeed = humanSpeed;
        }
    }

    private IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(1);
        GameManager.instance.OnLevelFailed();
    }
}
