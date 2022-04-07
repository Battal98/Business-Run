using UnityEngine;
using DG.Tweening;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] ParticleSystem ChangingPS;
    public enum ObstacleMovement
    {
        None, Spinner, Saw, Pushing, Smasher, Shaker
    }
    public enum ObstacleType
    {
        None, Fire, Hand, Punch, Barrier, Hole, Worker
    }
    public ObstacleMovement ObsMov = ObstacleMovement.Pushing; // For Movement
    public ObstacleType ObsType = ObstacleType.None; // For Type of Machine
    [System.Serializable]
    public class PushOptions
    {
        public float Speed = 2f;//Move Side    
        public bool Status = false; //Status
        public Vector2 minMaxPushValueX;
    }
    [System.Serializable]
    public class SawOptions
    {
        public float spinSpeed = 700;
        public float direction = 1;
        [Tooltip("Give 1 whichever angle you want it to rotate.")]
        public Vector3 angle;
    }
    [System.Serializable]
    public class SmasherOptions
    {
        public float Speed = 2f;//Move Side    
        public bool Status = false; //Status
        public Vector2 minMaxPushValueX;
    }

    [System.Serializable]
    public class ShakerOptions
    {
        public GameObject ShakeObject;
        public float duration = 2f;
        public float strength = 2f;
        public int vibration = 2;
        public float randomness = 90f;
        public int loopValue = 150;

    }
    [System.Serializable]
    public class BarrierOptions
    {
        public float recoil = 0;
        public float recoilDuration = 0;
    }

    [System.Serializable]
    public class WorkerOptions
    {
        public float slowEffect = 0;
    }

    [Space]
    [Header("*_ Movements Properties _*")]
    public SmasherOptions smasher;
    public PushOptions push;
    public SawOptions saw;
    public ShakerOptions shaker;
    public BarrierOptions barrier;
    public WorkerOptions worker;

    private void Start()
    {
        ShakeMovement(shaker.ShakeObject, shaker.loopValue);
        push.Status = true;
    }
    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            return;
        }
        XAxisMovement();
        YAxisMovement();
        RotateMovement();
    }

    void XAxisMovement()
    {
        if (ObsMov == ObstacleMovement.Pushing)
        {
            if (transform.position.x > push.minMaxPushValueX.x)
            {
                push.Status = false;
            }
            if (transform.position.x < push.minMaxPushValueX.y)
            {
                push.Status = true;
            }
            if (push.Status == true)
            {
                transform.Translate(push.Speed * Time.deltaTime, 0, 0);
            }
            if (push.Status == false)
            {
                transform.Translate(-push.Speed * Time.deltaTime, 0, 0);
            }
        }
    }

    void YAxisMovement()
    {
        if (ObsMov == ObstacleMovement.Smasher)
        {
            if (transform.position.y > smasher.minMaxPushValueX.x)
            {
                smasher.Status = false;
            }
            if (transform.position.y < smasher.minMaxPushValueX.y)
            {
                smasher.Status = true;
            }
            if (smasher.Status == true)
            {
                transform.Translate(0, smasher.Speed * Time.deltaTime, 0);
            }
            if (smasher.Status == false)
            {
                transform.Translate(0, -smasher.Speed * Time.deltaTime, 0);
            }
        }
    }

    void RotateMovement()
    {
        if (ObsMov == ObstacleMovement.Saw)
        {
            transform.Rotate(new Vector3(saw.angle.x, saw.angle.y, saw.angle.z) * Time.deltaTime * saw.spinSpeed * saw.direction);
        }
    }

    void ShakeMovement(GameObject _shakeObj, int _levelTime)
    {
        if(ObsMov == ObstacleMovement.Shaker)
        {
            _shakeObj.transform.DOShakePosition(shaker.duration, shaker.strength, shaker.vibration, shaker.randomness, false, true).SetLoops(_levelTime,LoopType.Yoyo);
        }
    }
}
