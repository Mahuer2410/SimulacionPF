using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependentController : MonoBehaviour
{
    private List<Transform> targetPoints = new List<Transform>(); // Lista de puntos de destino (mesas)
    private Vector3 originalPosition; // Posición original del NPC
    private bool movingToTarget = true; // Indica si se mueve hacia el destino o la posición original
    private bool visitedAllTargets = false; //Comprueba si se han visitado todos los puntos de destino

    public float movementSpeed = 2f; // Velocidad de movimiento
    private float waitTime; // Tiempo de espera aleatorio
    private float currentTime; // Tiempo actual
    private int currentTargetIndex = 0; // Índice del punto de destino actual

    private GameManager gameManager;

    void Start()
    {
        originalPosition = transform.position;
        gameManager = GameManager.Instance; // Obtener la instancia del GameManager
        gameManager.CalculateWithoutLimits(); // Calcular los valores (si aún no se ha hecho)
        
        GameObject laptopObject = GameObject.FindGameObjectWithTag("Laptop");
        if (laptopObject != null)
        {
            targetPoints.Add(laptopObject.transform);
        }
        // Buscamos todas las instancias del objeto "MesaTecnica(Clone)"
        GameObject[] mesaTecnicaInstances = GameObject.FindGameObjectsWithTag("Technician");
        foreach (var instance in mesaTecnicaInstances)
        {
            targetPoints.Add(instance.transform);
        }
        waitTime = Random.Range(1f, (float)gameManager.ws * 10);
        Debug.LogWarning($"waitTime: {waitTime}");
        currentTime = waitTime;
    }

    void Update()
    {
        // Esperamos antes de comenzar a mover al NPC
        if (currentTime > 0)
        {

            currentTime -= Time.deltaTime;
            return; // Salimos del Update sin hacer nada más
        }

        Move();
    }

    void Move()
    {
        // Si ya ha visitado todos los puntos de destino, mueve al NPC hacia la posición original
        if (visitedAllTargets)
        {
            Vector3 directionToOriginal = (originalPosition - transform.position).normalized;
            transform.Translate(directionToOriginal * movementSpeed * Time.deltaTime);

            // Verificamos si hemos llegado a la posición original
            if (Vector3.Distance(transform.position, originalPosition) <= 0.1f)
            {
                // Reiniciamos el tiempo de espera aleatorio
                waitTime = Random.Range(1f, (float)gameManager.ws);
                Debug.LogWarning($"waitTime origen: {waitTime}");
                currentTime = waitTime;
                visitedAllTargets = false; // Reiniciamos la variable
            }
        }
        else
        {
            // Movimiento hacia el punto de destino actual
            Vector3 directionToTarget = (targetPoints[currentTargetIndex].position - transform.position).normalized;
            transform.Translate(directionToTarget * movementSpeed * Time.deltaTime);

            // Verificamos si hemos llegado al punto de destino
            if (Vector3.Distance(transform.position, targetPoints[currentTargetIndex].position) <= 0.1f)
            {
                movingToTarget = !movingToTarget; // Cambiamos la dirección
                currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Count; // Cambiamos al siguiente punto de destino
                // Reiniciamos el tiempo de espera aleatorio
                waitTime = Random.Range(0.001f, (float)gameManager.ws);
                Debug.LogWarning($"waitTime origen: {waitTime}");
                currentTime = waitTime;

                // Si hemos visitado todos los puntos de destino, activamos la variable
                if (currentTargetIndex == 0)
                {
                    visitedAllTargets = true;
                }
            }
        }
    }
}