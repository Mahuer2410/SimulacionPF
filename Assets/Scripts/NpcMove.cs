using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMove2D : MonoBehaviour
{    
    public GameObject laptopPrefab; // Referencia al objeto de la laptop
    public GameObject ParentObject; // Referencia al objeto padre donde se intanciaran las laptops
    private bool laptopInstantiated = false;//chequea si ya se ha instanciado una laptop
    float newY = 0.05f;

    private Transform targetPoint; // Punto de destino (mesa)
    private Vector3 originalPosition; // Posici�n original del NPC
    private bool movingToTarget = true; // Indica si se mueve hacia el destino o la posici�n original

    public float movementSpeed = 2f; // Velocidad de movimiento
    private float waitTime; // Tiempo de espera aleatorio
    private float currentTime; // Tiempo actual

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        // Asignamos el objeto deseado a targetPoint
        targetPoint = GameObject.Find("Mesa").transform;
        // Establecemos un tiempo de espera aleatorio entre 1 y 5 segundos
        waitTime = Random.Range(1f, 5f);
        currentTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Esperamos antes de comenzar a mover al NPC
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            return; // Salimos del Update sin hacer nada m�s
        }

        if (movingToTarget)
        {
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
            transform.Translate(directionToTarget * movementSpeed * Time.deltaTime);

            // Verificamos si hemos llegado al punto de destino
            if (Vector3.Distance(transform.position, targetPoint.position) <= 0.1f)
            {
                // Deja la laptop en el punto de destino
                LeaveLaptop();

                movingToTarget = false;
                // Reiniciamos el tiempo de espera aleatorio
                waitTime = Random.Range(1f, 5f);
                currentTime = waitTime;
            }
        }
        else
        {
            Vector3 directionToOriginal = (originalPosition - transform.position).normalized;
            transform.Translate(directionToOriginal * movementSpeed * Time.deltaTime);

            // Verificamos si hemos vuelto a la posici�n original
            if (Vector3.Distance(transform.position, originalPosition) <= 0.1f)
            {
                movingToTarget = true;
                // Reiniciamos el tiempo de espera aleatorio
                waitTime = Random.Range(1f, 5f);
                currentTime = waitTime;
            }
        }
    }
    void LeaveLaptop()
    {
        if (!laptopInstantiated)
        {
            // Aumentamos la posici�n en y del punto de destino
            newY += targetPoint.position.y + 0.02f;
            Vector3 newPosition = new Vector3(-0.2f, newY, 0);

            // Crea una instancia de la laptop en el nuevo punto de destino
            Instantiate(laptopPrefab, newPosition, Quaternion.identity);

            laptopInstantiated = true; // Marcamos que la laptop ya se ha instanciado
        }
    }

}