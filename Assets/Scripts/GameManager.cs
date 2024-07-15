using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class QueueData
{
    public int N { get; set; }
    public string Pn { get; set; }
    public string Fn { get; set; }

    public QueueData(int n, string pn, string fn)
    {
        N = n;
        Pn = pn;
        Fn = fn;
    }
}

public class GameManager : MonoBehaviour
{
    private List<QueueData> _queueData = new List<QueueData>();
    public List<QueueData> QueueData
    {
        get => _queueData;
        set
        {
            _queueData = value;
            OnPropertyChanged();
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    //Variables
    public double lambda;
    public double mu;
    //Variables a calcular.
    public double rho = 0.0;
    public double po = 0;
    public double ls = 0;
    public double lq = 0;
    public double ws = 0;
    public double wq = 0;
    //private double lastPn = 0;

    //instancia del singletown
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Hay mas de un Game Manager en la scena!!");
        }

    }
    void Start()
    {
        CalculateWithoutLimits();
        Debug.Log($"rho: {rho}, po: {po}, ls: {ls}, lq: {lq}, ws: {ws}, wq: {wq}");
        PrintQueueData();
    }

    //-->CALCULOS DE LOS VALORES
    public void CalculateWithoutLimits()
    {
        rho = lambda / mu; // Asegúrate de usar valores de punto flotante para la división
        po = 1.0 - rho;
        ls = rho / (1.0 - rho);
        lq = Math.Pow(rho, 2.0) / (1.0 - rho);
        ws = ls / 5.0;
        wq = lq / 5.0;

        CalculatesTableWithoutLimits(po, rho);//Calculo de la tabla de valores.

        // Redondea los valores antes de mostrarlos en la consola
        rho = Math.Round(rho, 4);
        po = Math.Round(po, 4);
        ls = Math.Round(ls, 4);
        lq = Math.Round(lq, 4);
        ws = Math.Round(ws, 4);
        wq = Math.Round(wq, 4);
    }

    //CALCULOS DE UN SOLO SERVIDOR: SIN LIMITE en la cola
    private double CalculatePN(int iteration, double po, double rho)
    {
        return po * Math.Pow(rho, iteration);
    }
    private double CalculateFN(double previousResult, double pn)
    {
        return previousResult + pn;
    }

    //CALCULOS DE LA TABLA DE ITERACIONES
    private void CalculatesTableWithoutLimits(double po, double rho)
    {
        const double limit = 0.9999;

        int n = 0;
        double previousResult = 0.0;

        var results = new List<QueueData>();
        double fn = 0.0000;

        while (fn < limit)
        {
            double pn = CalculatePN(n, po, rho);
            pn = Math.Round(pn, 4); // Redondear pn a 4 decimales

            if (n == 0)
            {
                fn = pn;
            }
            else
            {
                fn = CalculateFN(previousResult, pn);
                fn = Math.Round(fn, 4); // Redondear fn a 4 decimales
            }

            previousResult = fn;

            if (fn > limit)
            {
                break;
            }

            results.Add(new QueueData(
                n, pn.ToString(), fn.ToString()
            ));
            n++;
        }
        _queueData = results;
    }
    private void PrintQueueData()
    {
        foreach (var data in _queueData)
        {
            Debug.Log($"N: {data.N}, Pn: {data.Pn}, Fn: {data.Fn}");
        }
    }
}