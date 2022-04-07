using UnityEngine;

public class PlayerForward : MonoBehaviour
{
    public static PlayerForward instance;
    public GameObject PlayerPrefab;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameEnded)
        {
            return;
        }
        this.transform.Translate(Vector3.forward * Time.deltaTime * GameManager.instance.PlayerForwardSpeed);
    }
}
