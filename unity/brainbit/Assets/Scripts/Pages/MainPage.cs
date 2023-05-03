using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPage : MonoBehaviour
{

    public void OnStartButtonClicked()
    {
        BackendManager.Instance.ToSearchPage();
    }

}
