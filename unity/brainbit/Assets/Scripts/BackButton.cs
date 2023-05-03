using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void OnClicked()
    {
        GameObject page = gameObject.transform.parent?.gameObject;
        if (page != null)
        {
            page.SetActive(false);
        }
    }
}
