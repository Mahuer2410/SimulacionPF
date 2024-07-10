using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependienteController : MonoBehaviour
{
    private List<Transform> targetPoints = new List<Transform>(); // Lista de puntos de destino (mesas)
    private Transform originalPosition; // Posición original del NPC
    private bool movingToTarget = true; // Indica si se mueve hacia el destino o la posición original

    public float movementSpeed = 2f; // Velocidad de movimiento
    private float waitTime; // Tiempo de espera aleatorio
    private float currentTime; // Tiempo actual
    private int currentTargetIndex = 0; // Índice del punto de destino actual

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform;

        // Establecemos un tiempo de espera aleatorio entre 1 y 5 segundos
        waitTime = Random.Range(1f, 5f);
        currentTime = waitTime;

        
    }

    // Update is called once per frame
    void Update()
    {
        // Buscamos todas las instancias del objeto "MesaTecnica"
        GameObject[] mesaTecnicaInstances = GameObject.FindGameObjectsWithTag("MesaTecnica");
        foreach (var instance in mesaTecnicaInstances)
        {
            targetPoints.Add(instance.transform);
        }
        targetPoints.Add(originalPosition);

        // Esperamos antes de comenzar a mover al NPC
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            return; // Salimos del Update sin hacer nada más
        }

        // Movimiento hacia el punto de destino actual
        Vector3 directionToTarget = (targetPoints[currentTargetIndex].position - transform.position).normalized;
        transform.Translate(directionToTarget * movementSpeed * Time.deltaTime);

        // Verificamos si hemos llegado al punto de destino
        if (Vector3.Distance(transform.position, targetPoints[currentTargetIndex].position) <= 0.1f)
        {
            movingToTarget = !movingToTarget; // Cambiamos la dirección
            currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Count; // Cambiamos al siguiente punto de destino
            // Reiniciamos el tiempo de espera aleatorio
            waitTime = Random.Range(1f, 5f);
            currentTime = waitTime;
        }
    }
}
