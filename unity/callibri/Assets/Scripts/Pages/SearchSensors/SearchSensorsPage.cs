using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NeuroSDK;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.Android;
using System.Linq;
using System.Threading.Tasks;

public class SearchSensorsPage : MonoBehaviour, IPage
{
    [SerializeField] private TextMeshProUGUI _startSearchButtonText;

    [SerializeField] private ListView _devicesList;

    private const string _startSearchText = "Start Search";
    private const string _stopSearchText = "Stop Search";

    private bool _isSearching = false;
    private bool isSearching
    {
        get
        {
            return _isSearching;
        }
        set
        {
            if (_isSearching != value)
            {
                _isSearching = value;
                _startSearchButtonText.text = _isSearching ? _stopSearchText : _startSearchText;
            }
        }
    }

    private void SensorsFounded(IReadOnlyList<SensorInfo> sensors)
    {
        MainThreadDispatcher.RunOnMainThread(() =>
        {
            _devicesList.AddAll(sensors);
        });
    }

    private void OnStartSearchButtonClicked()
    {
        if (!isSearching)
        {
#if UNITY_ANDROID
            if (SystemInfo.operatingSystem.Contains("31") ||
                SystemInfo.operatingSystem.Contains("32") ||
                SystemInfo.operatingSystem.Contains("33"))
            {
                Permission.RequestUserPermission("android.permission.BLUETOOTH_SCAN");
                Permission.RequestUserPermission("android.permission.BLUETOOTH_CONNECT");
            }
            else
            {
                Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
                Permission.RequestUserPermission("android.permission.ACCESS_COARSE_LOCATION");
            }
#endif

            isSearching = true;
            CallibriController.Instance.sensorsFound += SensorsFounded;
            CallibriController.Instance.StartSearch();

        }
        else
        {
            isSearching = false;
            _devicesList.HideList();
            CallibriController.Instance.StopSearch();
            CallibriController.Instance.sensorsFound -= SensorsFounded;
        }
    }

    public void DeviceItemClicked(int index, SensorInfo info)
    {
        StartCoroutine(WaitForSensorConnection(CallibriController.Instance.CreateAndConnect(info)));
    }

    private IEnumerator WaitForSensorConnection(Task<SensorState> connectionTask)
    {
        yield return new WaitUntil(() => connectionTask.IsCompleted);
        var state = connectionTask.Result;

        if (state == SensorState.StateInRange)
        {
            BackendManager.Instance.ToMenuPage();
        }
        else
        {
            Debug.Log("Device not connected!");
        }
    }

    private void OnEnable()
    {
        Enter();
    }

    public void OnDisable()
    {
        Exit();
    }

    public void Enter()
    {
        isSearching = false;
    }

    public void Exit()
    {
        CallibriController.Instance.StopSearch();

        _devicesList.HideList();
    }
}
