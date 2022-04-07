vusing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    #region Swerve Movement Values
    [SerializeField] Vector2 MinMaxPlayerPos;
    [SerializeField] Vector2 MinMaxPlayerSensivity;
    [SerializeField] float CalculatedXSens = 0.75f;
    GameObject MoneyCanvas;

    private GameObject OffSetObj;
    private Vector3 oldPosition = Vector3.zero;
    private Vector3 temp, temp2;
    float distanceFixer = 0;
    float lastX = 0;

    public Animator playerAnim;

    enum PlayerDirection
    {
        none,
        left,
        right
    }
    PlayerDirection playerDirection = PlayerDirection.none;
    PlayerDirection detectingDirection = PlayerDirection.none;
    #endregion

    PlayerTransformController myTransform;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        playerAnim = this.gameObject.GetComponentInChildren<Animator>();
        myTransform = this.gameObject.GetComponent<PlayerTransformController>();
    }

    void Start()
    {
        OffSetObj = new GameObject();
        OffSetObj.name = "OffSetObje";
        OffSetObj.transform.parent = this.transform.parent;
    }

    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            playerAnim.SetBool("Running", false);
            playerAnim.SetBool("Skating", false);
            return;
        }
        SwerveMovements();
        //CalculateCanvasPos();

    }
    #region Swerve Jobs
    private void SwerveMovements()
    {
        #region Movement 
        if (Input.GetMouseButtonDown(0))
        {
            OffSetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
            temp = this.transform.localPosition - OffSetObj.transform.localPosition;
            distanceFixer = Vector3.Distance(this.transform.position, OffSetObj.transform.localPosition);
            oldPosition = OffSetObj.transform.localPosition;
        }
        if (Input.GetMouseButton(0))
        {
            OffSetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
            DetectPlayerDirection();
            temp2 = OffSetObj.transform.localPosition + temp;
            temp2.y = this.transform.localPosition.y;
            temp2.z = 0;
            temp2.x = Mathf.Clamp(temp2.x, MinMaxPlayerPos.x, MinMaxPlayerPos.y);

            if (detectingDirection == PlayerDirection.none || detectingDirection != playerDirection)
                this.transform.localPosition = temp2;

            if (distanceFixer - 0.1f > Vector3.Distance(this.transform.position, OffSetObj.transform.position))
            {
                OffSetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
                temp = this.transform.localPosition - OffSetObj.transform.localPosition;
                distanceFixer = Vector3.Distance(this.transform.position, OffSetObj.transform.localPosition);
            }
            RotationCalculator();

        }
        if (!Input.GetMouseButton(0))
        {

            OffSetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
            temp = this.transform.localPosition - OffSetObj.transform.localPosition;
            distanceFixer = Vector3.Distance(this.transform.position, OffSetObj.transform.localPosition);
            playerDirection = PlayerDirection.none;
        }
        // this.transform.localEulerAngles = Vector3.zero;
        #endregion
        if (myTransform.currTransform == PlayerTransformController.MyTranform.Run)
        {
            playerAnim.SetBool("Running", true);
            playerAnim.SetBool("Skating", false);
        }
        else
        {
            playerAnim.SetBool("Running", false);
        }

    }
    private void LateUpdate()
    {
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * 5);
        //RotationCalculator();
    }
    float CalculateX()
    {
        Vector3 location = Input.mousePosition;
        return (location.x / (Screen.width / (MinMaxPlayerSensivity.y + Mathf.Abs(MinMaxPlayerSensivity.x)))) - CalculatedXSens;

    }
    void DetectPlayerDirection()
    {
        if (OffSetObj.transform.localPosition.x > oldPosition.x)
        {
            playerDirection = PlayerDirection.right;
        }
        else if (OffSetObj.transform.localPosition.x < oldPosition.x)
        {
            playerDirection = PlayerDirection.left;
        }
        oldPosition = OffSetObj.transform.localPosition;
    }
    void RotationCalculator()
    {

        float xRot = CalculateX();
        float currentY = this.transform.localEulerAngles.y;

        if (currentY > 180 && currentY < 360)
        {
            currentY = currentY - 360;
        }


        if (xRot > lastX)// Right Move
        {
            currentY += Mathf.Abs(xRot - lastX) * 200;

        }
        else if (xRot < lastX)//Left Move
        {
            currentY -= Mathf.Abs(lastX - xRot) * 200;
        }

        currentY = Mathf.Clamp(currentY, -60.0f, 60.0f);
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(Vector3.up * currentY), Time.deltaTime * 10);

        lastX = xRot;

        //Its is using for rotating to zero rotation everytime
    }
    #endregion

    public void CalculatedMouseDown()
    {
        OffSetObj.transform.localPosition = new Vector3(CalculateX() * 3, this.transform.position.y, 0);
        temp = this.transform.localPosition - OffSetObj.transform.localPosition;
        distanceFixer = Vector3.Distance(this.transform.position, OffSetObj.transform.localPosition);
        oldPosition = OffSetObj.transform.localPosition;
    }

    //World Up Canvas Controller if its out of bounds
    void CalculateCanvasPos()
    {
        if (this.transform.localPosition.x <= -0.5f)
        {
            MoneyCanvas.transform.localPosition = new Vector3(0.35f,MoneyCanvas.transform.localPosition.y, MoneyCanvas.transform.localPosition.z); 
        }
        else
        {
            MoneyCanvas.transform.localPosition = new Vector3(-0.25f, MoneyCanvas.transform.localPosition.y, MoneyCanvas.transform.localPosition.z);
        }

    }

}
