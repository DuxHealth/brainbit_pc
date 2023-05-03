using NeuroSDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResistPage : MonoBehaviour, IPage
{
    [SerializeField] private TextMeshProUGUI _StartResistText;

    [SerializeField] private TextMeshProUGUI _O1Value;
    [SerializeField] private TextMeshProUGUI _O2Value;
    [SerializeField] private TextMeshProUGUI _T3Value;
    [SerializeField] private TextMeshProUGUI _T4Value;

    private IEnumerator _updateValuesCoroutine;
    private BrainBitResistData _lastResistData;
    private readonly object locker = new object();

    private bool _started = false;
    private bool started
    {
        get { return _started; }
        set
        {
            if (value != _started)
            {
                _started = value;
                _StartResistText.text = _started ? "Stop" : "Start";
            }

        }
    }

    private IEnumerator UpdateValues()
    {
        while (true)
        {
            lock (locker)
            {
                _O1Value.text = string.Format("{0:F2} Ohm", _lastResistData.O1);
                _O2Value.text = string.Format("{0:F2} Ohm", _lastResistData.O2);
                _T3Value.text = string.Format("{0:F2} Ohm", _lastResistData.T3);
                _T4Value.text = string.Format("{0:F2} Ohm", _lastResistData.T4);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdateResist()
    {
        if (started)
        {
            BrainBitController.Instance.StopResist();
        }
        else
        {
            BrainBitController.Instance.StartResist((sample) => {
                lock (locker)
                {
                    _lastResistData = sample;
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
        _updateValuesCoroutine = UpdateValues();
        StartCoroutine(_updateValuesCoroutine);
    }

    public void Exit()
    {
        StopCoroutine(_updateValuesCoroutine);
        BrainBitController.Instance.StopResist();
    }
}
