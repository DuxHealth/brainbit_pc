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
        CallibriController.Instance.connectionStateChanged += OnConnectionStateChanged;
        CallibriController.Instance.batteryChanged += OnBatteryChanged;
    }

    private void OnConnectionStateChanged(SensorState state)
    {
        _stateValue = state;
        MainThreadDispatcher.RunOnMainThread(() => {
            SetButtonsInteractable(_stateValue == SensorState.StateInRange);
            _state.text = _stateValue == SensorState.StateOutOfRange ? _DisconnectedText : _ConnectedText;
        });
    }

    private void OnBatteryChanged(int power)
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

    public void OnReconnectButtonClicked()
    {
        if (CallibriController.Instance.ConnectionState == SensorState.StateInRange) CallibriController.Instance.DisconnectCurrent();
        else CallibriController.Instance.ConnectCurrent();
    }

    private void SetButtonsInteractable(bool active)
    {
        _deviceInfoButton.interactable = active;
        _signalButton.interactable = active;
    }

    private void OnDestroy()
    {
        CallibriController.Instance.connectionStateChanged -= OnConnectionStateChanged;
        CallibriController.Instance.batteryChanged -= OnBatteryChanged;
    }

    public void Enter()
    {
        var connectionState = CallibriController.Instance.ConnectionState;
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
