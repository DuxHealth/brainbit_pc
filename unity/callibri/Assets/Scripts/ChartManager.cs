using System;
using System.Collections.Generic;
using NeuroSDK;
using Newtonsoft.Json.Linq;
using UnityEngine;
using XCharts.Runtime;

public class ChartManager : MonoBehaviour
{
    [SerializeField] private LineChart _chart;
    [SerializeField] private string _seriesName;

    [SerializeField]
    private int _samplingFrequency = 250;
    private int window = 5;

    private int allSamplesCount = 0;


    void Start()
    {
        _chart.RemoveData();
        _chart.AddSerie<Line>(_seriesName);
        _chart.ClearData();
        _chart.SetMaxCache(_samplingFrequency * window);
    }

    public void AddData(double[] samples)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            allSamplesCount++;
            _chart.AddData(0, allSamplesCount, samples[i]);
        }

        _chart.RefreshChart();
    }

}
