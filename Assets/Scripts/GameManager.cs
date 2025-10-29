using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] GameObject startMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject errorMenu;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
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
        if (!int.TryParse(stringWidth, out int width))
        {
            TriggerError("Please only input whole numbers.");
            return;
        }

        String stringHeight = StartMenu.instance.height.GetComponentInChildren<TMP_InputField>().text.ToString();
        if (!int.TryParse(stringHeight, out int height))
        {
            TriggerError("Please only input whole numbers.");
            return;
        }

        String stringBombs = StartMenu.instance.bombs.GetComponentInChildren<TMP_InputField>().text.ToString();
        if (!int.TryParse(stringBombs, out int bombs))
        {
            TriggerError("Please only input whole numbers.");
            return;
        }


        if (!checkErrors(width, height, bombs))
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
        Generator.gen.triggerAllBombs();
        if (gm != null)
        {
            gm.loseMenu.SetActive(true);
        }
    }

    public static void GameWon()
    {
        gameOver = true;
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
    
    private bool checkErrors(int width, int height, int bombs)
    {
        bool clearOfErrors = true;
        if (width <= 0)
        {
            TriggerError("The width must be equal or greater than one.");
            clearOfErrors = false;
            return clearOfErrors;
        }
        if (height <= 0)
        {
            TriggerError("The height must be equal or greater than one.");
            clearOfErrors = false;
            return clearOfErrors;
        }
        if (bombs <= 0)
        {
            TriggerError("You must have at least one bomb. Otherwise it would be boring, right?");
            clearOfErrors = false;
            return clearOfErrors;
        }
        if ((width * height) < bombs)
        {
            TriggerError("The number of bombs is greater than the available space!");
            clearOfErrors = false;
            return clearOfErrors;
        }
        return clearOfErrors;
    }

    public void TriggerError(String error)
    {
        ErrorManager.setErrMsg(error);
        errorMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
