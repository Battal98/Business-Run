using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment_Script : MonoBehaviour
{
    public GameObject EnvironmentPlazasParent;
    [SerializeField] List<Color> Colors;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < EnvironmentPlazasParent.transform.childCount; i++)
        {
            for (int k = 0; k < EnvironmentPlazasParent.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().materials.Length; k++)
            {
                int RandomValue = Random.Range(0,Colors.Count);
                EnvironmentPlazasParent.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().materials[k].color = Colors[RandomValue];

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
