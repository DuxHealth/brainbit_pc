using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using NeuroSDK;

public class SignalsPage : MonoBehaviour, IPage
{
    [SerializeField] private TextMeshProUGUI _StartSignalText;

    [SerializeField] private ChartManager _o1chart;
    [SerializeField] private ChartManager _o2chart;
    [SerializeField] private ChartManager _t3chart;
    [SerializeField] private ChartManager _t4chart;

    private IEnumerator _updateChartsCoroutine;
    private List<BrainBitSignalData> _signalData = new List<BrainBitSignalData>();
    private readonly object locker = new object();

    private bool _started = false;
    private bool started {
        get { return _started; }
        set
        {
            if (value != _started) {
                _started = value;
                _StartSignalText.text = _started ? "Stop" : "Start";
            }

        }
    }
    
    public void ChannelsDropdownItemSelected(TMP_Dropdown dropdown)
    {
        //BackendManager.Instance.ChartManager.SelectedChannel = dropdown.value;
    }

    private IEnumerator UpdateCharts() {
        while (true) {
            lock (locker) 
            {
                int samplesCount = _signalData.Count;
                if (samplesCount > 0) {
                    var dataO1 = new double[samplesCount];
                    var dataO2 = new double[samplesCount];
                    var dataT3 = new double[samplesCount];
                    var dataT4 = new double[samplesCount];

                    for (int i = 0; i < samplesCount; i++)
                    {
                        dataO1[i] = _signalData[i].O1;
                        dataO2[i] = _signalData[i].O2;
                        dataT3[i] = _signalData[i].T3;
                        dataT4[i] = _signalData[i].T4;
                    }

                    _o1chart.AddData(dataO1);
                    _o2chart.AddData(dataO2);
                    _t3chart.AddData(dataT3);
                    _t4chart.AddData(dataT4);

                    _signalData.Clear();
                }
            } 

            yield return new WaitForSeconds(0.06f);
        }
    }

    public void UpdateSignal()
    {
        if (started)
        {
            BrainBitController.Instance.StopSignal();
        }
        else 
        {
            BrainBitController.Instance.StartSignal((samples) => {

                if (samples != null && samples.Length > 0) 
                {
                    lock (locker) {
                        _signalData.AddRange(samples);
                    }
                        
                }
            });
        }
        started = !started;
    }

    private void OnEnable()
    {
        Enter();
    }

    private void OnDisable()
    {
        Exit();
    }

    public void Enter()
    {
        _updateChartsCoroutine = UpdateCharts();
        StartCoroutine(_updateChartsCoroutine);
    }

    public void Exit()
    {
        StopCoroutine(_updateChartsCoroutine);
        BrainBitController.Instance.StopSignal();
    }
}
