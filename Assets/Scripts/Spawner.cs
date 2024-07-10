using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject myPrefab;
    public int numberOfInstances; // Cantidad de instancias deseadas

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < numberOfInstances; i++)
        {
            Instantiate(myPrefab, new Vector3(i * 0.5f, transform.position.y, 0), Quaternion.identity);
        }
    }
}
