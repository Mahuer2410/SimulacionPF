using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RelojController : MonoBehaviour
{
    public TextMeshProUGUI horas;
    public TextMeshProUGUI minutos;
    public int multipTime;
    private float _minutos;
    private float _horas;

    void Start()
    {

    }

    void Update()
    {
        _minutos += multipTime * Time.deltaTime;
        minutos.text = _minutos.ToString("f0");
        horas.text = _horas.ToString("f0");
        if (_minutos > 59)
        {
            _minutos = 0;
            _horas++;
        }
        if (_horas > 15)
        {
            _horas = 0;
        }

    }
}
