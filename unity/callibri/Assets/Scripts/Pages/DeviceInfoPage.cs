using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeviceInfoPage : MonoBehaviour, IPage
{
    [SerializeField] private TextMeshProUGUI _infoText;

    public void Enter()
    {
        _infoText.text = CallibriController.Instance.FullInfo();
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
