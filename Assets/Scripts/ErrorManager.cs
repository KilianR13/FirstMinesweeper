using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] public TMP_Text errMsg;
    [SerializeField] private GameObject errorMenu;
    public static ErrorManager errmng;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (errmng == null)
        {
            errmng = this;
        }
        else
        {
            Destroy(gameObject);            
        }
    }

    public static void setErrMsg(String error)
    {
        // if (errmng == null)
        // {
        //     Debug.LogError("ErrorManager no ha sido inicializado.");
        //     return;
        // }

        // if (errmng.errMsg == null)
        // {
        //     Debug.LogError("El campo errMsg no está asignado en el Inspector.");
        //     return;
        // }

        errmng.errMsg.SetText(error);
    }

    public void CloseError()
    {
        if (errorMenu != null)
        {
            errorMenu.SetActive(false);
        }
    }
}
