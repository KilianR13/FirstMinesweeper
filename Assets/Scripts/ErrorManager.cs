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
