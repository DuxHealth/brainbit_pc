using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using NeuroSDK;

public class SignalsPage : MonoBehaviour, IPage
{
    [SerializeField] private TextMeshProUGUI _startSignalText;

    [SerializeField] private ChartManager _signalChart;

    [SerializeField] private TextMeshProUGUI _electrodeStateText;

    private Coroutine _updateSignalCoroutine;
    private List<CallibriSignalData> _signalData = new List<CallibriSignalData>();
    private readonly object locker = new object();

    private bool _started = false;
    private bool Started
    {
        get => _started;
        set
        {
            if (value != _started)
            {
                _started = value;
                _startSignalText.text = _started ? "Stop" : "Start";
            }
        }
    }

    private IEnumerator UpdateSignalCharts()
    {
        while (true)
        {
            lock (locker)
            {
                int totalSamplesLength = 0;
                foreach (var sample in _signalData)
                {
                    totalSamplesLength += sample.Samples.Length;
                }

                if (totalSamplesLength < 0) continue;

                var signalSamples = new double[totalSamplesLength];

                int signalSamplesIndex = 0;
                foreach (var sample in _signalData)
                {
                    for (int i = 0; i < sample.Samples.Length; i++)
                    {
                        signalSamples[signalSamplesIndex] = sample.Samples[i];
                        signalSamplesIndex++;
                    }
                }

                _signalChart.AddData(signalSamples);
                _signalData.Clear();
            }

            yield return new WaitForSeconds(0.06f);
        }
    }

    public void UpdateSignal()
    {
        if (Started)
        {
            CallibriController.Instance.StopSignal();
            CallibriController.Instance.signalReceived -= OnSignalReceived;

            CallibriController.Instance.StopElectrode();
            CallibriController.Instance.electrodeStateChanged -= OnElectrodeStateChanged;

            Started = false;
            return;
        }

        CallibriController.Instance.electrodeStateChanged += OnElectrodeStateChanged;
        CallibriController.Instance.StartElectrode();

        CallibriController.Instance.signalReceived += OnSignalReceived;
        CallibriController.Instance.StartSignal();
        Started = true;
    }

    private void OnSignalReceived(CallibriSignalData[] samples)
    {
        if (samples is null && samples.Length < 0) return;
        lock (locker)
        {
            _signalData.AddRange(samples);
        }
    }

    private void OnElectrodeStateChanged(CallibriElectrodeState state)
    {
        MainThreadDispatcher.RunOnMainThread(() => _electrodeStateText.text = state.ToString());
    }

    private void Start() {
        _electrodeStateText.text = CallibriElectrodeState.ElStDetached.ToString();
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
        _updateSignalCoroutine = StartCoroutine(UpdateSignalCharts());
    }

    public void Exit()
    {
        StopCoroutine(_updateSignalCoroutine);
        CallibriController.Instance.StopSignal();
    }
}
