using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMove2D : MonoBehaviour
{
    private Transform targetPoint; // Punto de destino (mesa)
    private Vector3 originalPosition; // Posición original del NPC
    private bool movingToTarget = true; // Indica si se mueve hacia el destino o la posición original

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
            return; // Salimos del Update sin hacer nada más
        }

        if (movingToTarget)
        {
            Vector3 directionToTarget = (targetPoint.position - transform.position).normalized;
            transform.Translate(directionToTarget * movementSpeed * Time.deltaTime);

            // Verificamos si hemos llegado al punto de destino
            if (Vector3.Distance(transform.position, targetPoint.position) <= 0.1f)
            {
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

            // Verificamos si hemos vuelto a la posición original
            if (Vector3.Distance(transform.position, originalPosition) <= 0.1f)
            {
                movingToTarget = true;
                // Reiniciamos el tiempo de espera aleatorio
                waitTime = Random.Range(1f, 5f);
                currentTime = waitTime;
            }
        }
    }
}
