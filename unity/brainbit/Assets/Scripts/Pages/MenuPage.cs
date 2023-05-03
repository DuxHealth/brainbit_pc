using System;
using NeuroSDK;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Android;
using System.Collections;

public class MenuPage : MonoBehaviour, IPage
{
    [SerializeField] private Button _deviceInfoButton;
    [SerializeField] private Button _signalButton;

    [SerializeField] private TextMeshProUGUI _state;
    private SensorState _stateValue = SensorState.StateOutOfRange;

    [SerializeField] private TextMeshProUGUI _power;
    private int _powerValue = 0;

    private const string _ConnectedText = "Connected";
    private const string _DisconnectedText = "Disconnected";


    void Start()
    {
        BrainBitController.Instance.connectionStateChanged += ConnectionStateChanged;
        BrainBitController.Instance.batteryChanged += BatteryChanged;
    }

    private void ConnectionStateChanged(SensorState state)
    {
        _stateValue = state;
        MainThreadDispatcher.RunOnMainThread(() => {
            SetButtonsInteractable(_stateValue == SensorState.StateInRange);

            _state.text = _stateValue == SensorState.StateOutOfRange ? _DisconnectedText : _ConnectedText;
        });
    }

    private void BatteryChanged(int power)
    {
        _powerValue = power;
        MainThreadDispatcher.RunOnMainThread(() => {
            _power.text = _powerValue.ToString();
        });
    }

    public void OnDeviceInfoButtonClicked()
    {
        BackendManager.Instance.ToDeviceInfoPage();
    }

    public void OnSignalButtonClicked()
    {
        BackendManager.Instance.ToSignalPage();
    }

    public void OnResistButtonClicked()
    {
        BackendManager.Instance.ToResistPage();
    }

    public void OnReconnectButtonClicked()
    {
        if (BrainBitController.Instance.ConnectionState == SensorState.StateInRange)
        {
            BrainBitController.Instance.DisconnectCurrent();
        }
        else
        {
            BrainBitController.Instance.ConnectCurrent((state) => {
                Debug.Log($"Device connected: {state == SensorState.StateInRange}");
            });
        }
    }

    private void SetButtonsInteractable(bool active)
    {
        _deviceInfoButton.interactable = active;
        _signalButton.interactable = active;
    }

    private void OnDestroy()
    {
        BrainBitController.Instance.connectionStateChanged -= ConnectionStateChanged;
        BrainBitController.Instance.batteryChanged -= BatteryChanged;
    }

    public void Enter()
    {
        var connectionState = BrainBitController.Instance.ConnectionState;
        SetButtonsInteractable(connectionState == SensorState.StateInRange);
        _state.text = connectionState == SensorState.StateOutOfRange ? _DisconnectedText : _ConnectedText;
    }

    public void Exit()
    {
        
    }

    private void OnEnable()
    {
        Enter();
    }

    private void OnDisable()
    {
        Exit();
    }
}
