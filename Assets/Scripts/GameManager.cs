using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string _lambda;
    public string Lambda
    {
        get => _lambda;
        set
        {
            _lambda = value;
            OnPropertyChanged();
        }
    }

    private string _mu;
    public string Mu
    {
        get => _mu;
        set
        {
            _mu = value;
            OnPropertyChanged();
        }
    }

    private string _n;
    public string N
    {
        get => _n;
        set
        {
            _n = value;
            OnPropertyChanged();
        }
    }

    private string _k;
    public string K
    {
        get => _k;
        set
        {
            _k = value;
            OnPropertyChanged();
        }
    }

    private string _c;
    public string C
    {
        get => _c;
        set
        {
            _c = value;
            OnPropertyChanged();
        }
    }

    private double _rho;
    public double Rho
    {
        get => _rho;
        set
        {
            _rho = value;
            OnPropertyChanged();
        }
    }

    private double _po;
    public double Po
    {
        get => _po;
        set
        {
            _po = value;
            OnPropertyChanged();
        }
    }

    private double _ls;
    public double Ls
    {
        get => _ls;
        set
        {
            _ls = value;
            OnPropertyChanged();
        }
    }

    private double _lq;
    public double Lq
    {
        get => _lq;
        set
        {
            _lq = value;
            OnPropertyChanged();
        }
    }

    private double _ws;
    public double Ws
    {
        get => _ws;
        set
        {
            _ws = value;
            OnPropertyChanged();
        }
    }

    private double _wq;
    public double Wq
    {
        get => _wq;
        set
        {
            _wq = value;
            OnPropertyChanged();
        }
    }

    private double _rhoC;
    public double RhoC
    {
        get => _rhoC;
        set
        {
            _rhoC = value;
            OnPropertyChanged();
        }
    }

    private double _cWhitline;
    public double CWhitline
    {
        get => _cWhitline;
        set
        {
            _cWhitline = value;
            OnPropertyChanged();
        }
    }

    //cantidad de servidores activos
    private double _cActivos;
    public double CActivos
    {
        get => _cActivos;
        set
        {
            _cActivos = value;
            OnPropertyChanged();
        }
    }

    //Ws auxiliar
    private string _auxWs;
    public string AuxWs
    {
        get => _auxWs;
        set
        {
            _auxWs = value;
            OnPropertyChanged();
        }
    }

    //Wq auxiliar
    private string _auxWq;
    public string AuxWq
    {
        get => _auxWq;
        set
        {
            _auxWq = value;
            OnPropertyChanged();
        }
    }

    //Rho auxiliar
    private string _auxRho;
    public string AuxRho
    {
        get => _auxRho;
        set
        {
            _auxRho = value;
            OnPropertyChanged();
        }
    }

    //Lambda efectiva
    private double _lambdaEfect;
    public double LambdaEfect
    {
        get => _lambdaEfect;
        set
        {
            _lambdaEfect = value;
            OnPropertyChanged();
        }
    }

    //Lambda perdida
    private double _lambdaPer;
    public double LambdaPer
    {
        get => _lambdaPer;
        set
        {
            _lambdaPer = value;
            OnPropertyChanged();
        }
    }

    //ultimo Pn
    private double _lastPn;
    public double LastPn
    {
        get => _lastPn;
        set
        {
            _lastPn = value;
            OnPropertyChanged();
        }
    }

    //listas...
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

    private List<QueueDataMultiple> _queueDataMultiple = new List<QueueDataMultiple>();
    public List<QueueDataMultiple> QueueDataMultiple
    {
        get => _queueDataMultiple;
        set
        {
            _queueDataMultiple = value;
            OnPropertyChanged();
        }
    }

    private List<QueueDataMultipleTwo> _queueDataMultipleTwo = new List<QueueDataMultipleTwo>();
    public List<QueueDataMultipleTwo> QueueDataMultipleTwo
    {
        get => _queueDataMultipleTwo;
        set
        {
            _queueDataMultipleTwo = value;
            OnPropertyChanged();
        }
    }

    private List<Simulacion> _resultados = new List<Simulacion>();
    public List<Simulacion> Resultados
    {
        get => _resultados;
        set
        {
            _resultados = value;
            OnPropertyChanged();
        }
    }

    private List<Simulacion> _resultados2 = new List<Simulacion>();
    public List<Simulacion> Resultados2
    {
        get => _resultados2;
        set
        {
            _resultados2 = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
   
   private double rho = 0.0;
   private double po = 0;
   private double ls = 0;
   private double lq = 0;
   private double ws = 0;
   private double wq = 0;
   private double lastPn = 0;

   private bool isError = false;
   private string errorMessage = "";
   //CALCULOS DE UN SOLO SERVIDOR: SIN LIMITE en la cola

    //-->GENERALES DE UN SOLO SERVIDOR
    private double CalculatePN(int iteration, double po, double rho)
    {
        return po * Math.Pow(rho, iteration);
    }

    private double CalculateFN(double previousResult, double pn)
    {
        return previousResult + pn;
    }

    //-->CALCULOS DE LOS VALORES
    private void CalculateWithoutLimits()
    {
        rho = 5 / 6;

        po = 1 - rho;

        ls = rho / (1 - rho);

        lq = Math.Pow(rho, 2.0) / (1 - rho);

        ws = ls / 5;

        wq = lq / 5;

        CalculatesTableWithoutLimits(po, rho);

        rho = Math.Round(rho, 4);
        po = Math.Round(po,4);
        ls = Math.Round(ls, 4);
        lq = Math.Round(lq, 4);
        ws = Math.Round(ws, 4);
        wq = Math.Round(wq, 4);
    }
    //-->CALCULOS DE LA TABLA DE ITERACIONES
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

            if (n == 0)
            {
                fn = pn;
            }
            else

            {
                fn = CalculateFN(previousResult, pn);
            }

            previousResult = fn;

            if (fn > limit)
            {
                break;
            }

            results.Add(new QueueData(
                n,pn.ToString("0.0000"),
                fn.ToString("0.0000")
                ));
            n++;
        }
    }


}

//clases
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

public class QueueDataMultiple
{
    public int N { get; set; }
    public string Pn { get; set; }
    public string Fn { get; set; }

    public QueueDataMultiple(int n, string pn, string fn)
    {
        N = n;
        Pn = pn;
        Fn = fn;
    }
}

public class QueueDataMultipleTwo
{
    public int N { get; set; }
    public string Pn { get; set; }
    public string Fn { get; set; }
    public string CnMinusPn { get; set; }

    public QueueDataMultipleTwo(int n, string pn, string fn, string cnMinusPn)
    {
        N = n;
        Pn = pn;
        Fn = fn;
        CnMinusPn = cnMinusPn;
    }
}

public class QueueDataPO
{
    public int N { get; set; }
    public double Result { get; set; }

    public QueueDataPO(int n, double result)
    {
        N = n;
        Result = result;
    }
}

public class Simulacion
{
    public int NumeroSimulacion { get; set; }
    public List<SimulacionResultado> Resultados { get; set; }
    public double Media { get; set; }
    public double Desviacion { get; set; }

    public Simulacion(int numeroSimulacion, List<SimulacionResultado> resultados, double media, double desviacion)
    {
        NumeroSimulacion = numeroSimulacion;
        Resultados = resultados;
        Media = media;
        Desviacion = desviacion;
    }
}

public class SimulacionResultado
{
    public int NumeroIteracion { get; set; }
    public int Resultado { get; set; }

    public SimulacionResultado(int numeroIteracion, int resultado)
    {
        NumeroIteracion = numeroIteracion;
        Resultado = resultado;
    }
}

public class Iteracion
{
    public int N { get; set; }
    public double Pn { get; set; }
    public double Fn { get; set; }

    public Iteracion(int n, double pn, double fn)
    {
        N = n;
        Pn = pn;
        Fn = fn;
    }
}

public class Iteracion2
{
    public int N { get; set; }
    public double Fn { get; set; }

    public Iteracion2(int n, double fn)
    {
        N = n;
        Fn = fn;
    }
}
