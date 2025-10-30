using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Menus")]
    [SerializeField] GameObject startMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject errorMenu;

    [Header("Game SFX")]
    [SerializeField] private AudioSource bombExplosion;
    [SerializeField] private AudioSource gameWin;

    private int totalBombs;
    private int flagsPlanted;
    private int correctFlags;


    public static bool gameOver;
    public static GameManager gm;


    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject); // El Game Manager persiste entre partidas.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // Activa y desactiva los paneles correctos para hacer la UI verse bien.
        startMenu.SetActive(true);
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        errorMenu.SetActive(false);
    }

    
    public void GameStart()
    {
        gameOver = false;
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        Generator.gen.boardContainer.gameObject.SetActive(true);
        Generator.gen.ClearBoard();

        String stringWidth = StartMenu.instance.width.GetComponentInChildren<TMP_InputField>().text.ToString();
        if (!int.TryParse(stringWidth, out int width)) // Se asegura de que el jugador no introduzca letras.
        {
            TriggerError("Please only input whole numbers.");
            return;
        }

        String stringHeight = StartMenu.instance.height.GetComponentInChildren<TMP_InputField>().text.ToString();
        if (!int.TryParse(stringHeight, out int height)) // Se asegura de que el jugador no introduzca letras.
        {
            TriggerError("Please only input whole numbers.");
            return;
        }

        String stringBombs = StartMenu.instance.bombs.GetComponentInChildren<TMP_InputField>().text.ToString();
        if (!int.TryParse(stringBombs, out int bombs)) // Se asegura de que el jugador no introduzca letras.
        {
            TriggerError("Please only input whole numbers.");
            return;
        }


        if (!checkErrors(width, height, bombs)) // Comprueba que no haya otro tipo de errores. Si es falso, es que hay errores.
        {
            return;
        }

        Generator.gen.setWidth(width);
        Generator.gen.setHeight(height);
        Generator.gen.setBombs(bombs);
        totalBombs = bombs;
        flagsPlanted = 0;
        correctFlags = 0;
        Generator.gen.Generate();
        startMenu.SetActive(false);
    }

    public static void GameOver()
    {
        gameOver = true;
        gm.bombExplosion.Play();
        Generator.gen.triggerAllBombs();
        if (gm != null)
        {
            gm.loseMenu.SetActive(true);
        }
    }

    public static void GameWon()
    {
        gameOver = true;
        gm.gameWin.Play();
        if (gm != null)
        {
            gm.winMenu.SetActive(true);
        }
    }

    public bool CanPlaceFlag()
    {
        return flagsPlanted < totalBombs;
    }

    public void RegisterFlag(bool isCorrect)
    {
        flagsPlanted++;
        if (isCorrect) correctFlags++;

        CheckVictory();
    }

    public void UnregisterFlag(bool isCorrect)
    {
        flagsPlanted--;
        if (isCorrect) correctFlags--;
    }

    private void CheckVictory()
    {
        if (correctFlags == totalBombs && flagsPlanted == totalBombs)
        {
            GameWon();
        }
    }

    public void ChangeSettings()
    {
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
        startMenu.SetActive(true);
        Generator.gen.boardContainer.gameObject.SetActive(false);
    }
    
    // Comprueba errores con los valores introducidos. Si detecta un error, devuelve falso inmediatamente.
    private bool checkErrors(int width, int height, int bombs)
    {
        bool clearOfErrors = true;
        if (width <= 0) // Número inválido de casillas de anchura
        {
            TriggerError("The width must be equal or greater than one.");
            clearOfErrors = false;
            return clearOfErrors;
        }
        if (height <= 0) // Número inválido de casillas de altura
        {
            TriggerError("The height must be equal or greater than one.");
            clearOfErrors = false;
            return clearOfErrors;
        }
        if (bombs <= 0) // Número inválido de bombas.
        {
            TriggerError("You must have at least one bomb. Otherwise it would be boring, right?");
            clearOfErrors = false;
            return clearOfErrors;
        }
        if ((width * height) < bombs) // Más bombas que casillas.
        {
            TriggerError("The number of bombs is greater than the available space!");
            clearOfErrors = false;
            return clearOfErrors;
        }
        return clearOfErrors;
    }

    // Hace visible el popup de error y escribe el mensaje de error en el susodicho.
    public void TriggerError(String error)
    {
        errorMenu.SetActive(true); // Importante poner esto en True antes de modificar el menú de errores.
        ErrorManager.setErrMsg(error);
    }
    
    // Cierra el juego.
    public void QuitGame()
    {
        Application.Quit();
    }
}
