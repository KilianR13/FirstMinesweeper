using TMPro;
using UnityEngine;

public class StartMenu : MonoBehaviour
{

    [Header("GameSettings")]
    [SerializeField] public TMP_InputField width;
    [SerializeField] public TMP_InputField height;
    [SerializeField] public TMP_InputField bombs;

    public static StartMenu instance;

    void Awake()
    {
        instance = this;
    }
    
    

}
