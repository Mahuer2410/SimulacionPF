using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject myPrefab;
    public int numberOfInstances=1; // Cantidad de instancias deseadas

    void Awake()
    {
        for (int i = -1; i < (numberOfInstances-1); i++)
        {
            Instantiate(myPrefab, new Vector3(i * 1.5f, transform.position.y, 0), Quaternion.identity);
        }
    }
}
