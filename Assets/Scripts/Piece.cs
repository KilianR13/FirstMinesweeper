using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    [SerializeField] private int x, y;
    [SerializeField] private bool bomb, checkedPiece;
    [SerializeField] private Image bombSprite;
    [SerializeField] private Image flagSprite;

    private bool hasFlag = false;

    void Start()
    {
        bombSprite = transform.Find("Canvas/Bomb").GetComponent<Image>();

        GetComponent<SpriteRenderer>().material.color = Color.gray;
        if (bombSprite != null)
        {
            bombSprite.enabled = false;
        }
        if (flagSprite != null)
        {
            flagSprite.enabled = false;
        }
        // Para hacer debug de la posición de las bombas
        // if (bomb != false)
        // {
        //     bombSprite.enabled = true;
        // }
    }

    public int getX()
    {
        return x;
    }

    public void setX(int x)
    {
        this.x = x;
    }

    public int getY()
    {
        return y;
    }

    public void setY(int y)
    {
        this.y = y;
    }

    public void setBomb(bool bomb)
    {
        this.bomb = bomb;
    }

    public bool isBomb()
    {
        return bomb;
    }

    public void setChekedPiece(bool checkedPiece)
    {
        this.checkedPiece = checkedPiece;
    }

    public bool ischeckedPiece()
    {
        return checkedPiece;
    }

    public void triggerBomb()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
        bombSprite.enabled = true;
    }

    // Funcion que se llama cada vez que se pulsa el botón derecho.
    public void plantFlag()
    {
        if (!hasFlag)
        {
            if (GameManager.gm.CanPlaceFlag())
            {
                flagSprite.enabled = true;
                hasFlag = true;
                GameManager.gm.RegisterFlag(bomb);
            }
        }
        else
        {
            flagSprite.enabled = false;
            hasFlag = false;
            GameManager.gm.UnregisterFlag(bomb);
        }
    }

    public void drawbomb()
    {
        if (!ischeckedPiece())
        {
            setChekedPiece(true);

            if (isBomb())
            {
                GameManager.GameOver();
            }
            else
            {
                int bombCount = Generator.gen.getBombsAround(x, y);

                if (bombCount != 0)
                {
                    transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                        bombCount.ToString();
                }
                else
                {
                    Generator.gen.EmptyPiecesCheck(x, y);
                }
                // Quita las banderas ya plantadas y las deregistra.
                if (hasFlag)
                {
                    flagSprite.enabled = false;
                    hasFlag = false;
                    GameManager.gm.UnregisterFlag(bomb);
                }
                GetComponent<SpriteRenderer>().material.color = Color.white;
            }
            flagSprite.enabled = false;
        }
    }

    void OnMouseOver()
    {
        if (GameManager.gameOver) // Comprueba primero si no ha perdido
        {
            return;
        }

        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            drawbomb();
        }
        
        if (Input.GetMouseButtonDown(1)) // Click derecho
        {
            if (!ischeckedPiece())
            {
                plantFlag();
            }
        }
    }

}
