using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependentController : MonoBehaviour
{
    private List<Transform> targetPoints = new List<Transform>(); // Lista de puntos de destino (mesas)
    private GameManager gameManager;//referencia al game manager
    private Vector3 originalPosition; // Posición original del NPC

    private bool movingToTarget = true; // Indica si se mueve hacia el destino o la posición original
    private bool visitedAllTargets = false; //Indica si se han visitado todos los puntos de destino

    public float movementSpeed = 2f; // Velocidad de movimiento
    private float currentTime; // Tiempo actual
    private float waitTime; // Tiempo de espera

    private int currentTargetIndex = 0; // Índice del punto de destino actual
    private List<GameObject> laptops = new List<GameObject>(); // Lista para almacenar las laptops
    private Transform currentTarget; // Referencia a la laptop actual a la que se dirige

    void Start()
    {
        originalPosition = transform.position;
        // Suscribirse al evento de instanciación de laptops
        NpcMove2D.OnLaptopInstantiated += OnLaptopInstantiated;
        gameManager = GameManager.Instance; // Obtener la instancia del GameManager
        gameManager.CalculateWithoutLimits(); // Calcular los valores (si aún no se ha hecho)
        
        // Buscamos todas las instancias del objeto "MesaTecnica(Clone)"
        GameObject[] mesaTecnicaInstances = GameObject.FindGameObjectsWithTag("Technician");
        foreach (var instance in mesaTecnicaInstances)
        {
            targetPoints.Add(instance.transform);
        }
    }

    void Update()
    {
        // Esperamos antes de comenzar a mover al NPC
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            return; // Salimos del Update sin hacer nada más
        }
        if (currentTarget != null)
        { Move();}
        else
        { FindNextLaptop();}
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
                waitTime = Random.Range(1f, (float)gameManager.ws*10);// Reiniciamos el tiempo de espera aleatorio
                
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
                
                // Si hemos visitado todos los puntos de destino, activamos la variable
                if (currentTargetIndex == 0)
                {
                    visitedAllTargets = true;
                }
            }
        }
    }
    void FindNextLaptop()
    {
        // Buscar la siguiente laptop en la lista de laptops instanciadas
        if (laptops.Count > 0)
        {
            currentTarget = laptops[0].transform;
            laptops.RemoveAt(0); // Eliminar la laptop de la lista
        }
        else
        {
            currentTarget = null; // No hay más laptops que alcanzar
        }
    }

    void OnLaptopInstantiated(GameObject laptop)
    {
        // Agregar la nueva laptop a la lista
        laptops.Add(laptop);
    }

    void OnDestroy()
    {
        // Dejar de escuchar el evento de instanciación de laptops
        NpcMove2D.OnLaptopInstantiated -= OnLaptopInstantiated;
    }
}