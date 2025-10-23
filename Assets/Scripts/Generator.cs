using UnityEngine;

public class Generator : MonoBehaviour
{

    // Variables
    [SerializeField] private GameObject cell;
    private int width, height, bombsNumber;
    [SerializeField] private GameObject[][] map;
    [SerializeField] public Transform boardContainer;
    


    public static Generator gen;

    void Awake()
    {
        if (gen == null)
        {
            gen = this;
        }
        else
        {
            Destroy(gameObject); // O log de advertencia
        }
    }

    public void setWidth(int width)
    {
        this.width = width;
    }

    public void setHeight(int height)
    {
        this.height = height;
    }

    public void setBombs(int bombs)
    {
        this.bombsNumber = bombs;
    }


    public void Generate()
    {
        // Crear el array con las dimensiones correctas
        map = new GameObject[width][];

        for (int i = 0; i < width; i++)
        {
            map[i] = new GameObject[height];
        }

        // Crear las celdas con índices consistentes
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i][j] = Instantiate(cell, new Vector3(i, j, 0), Quaternion.identity, boardContainer);
                map[i][j].GetComponent<Piece>().setX(i);
                map[i][j].GetComponent<Piece>().setY(j);
            }
        }

        // Posiciona la cámara en el centro
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -1);

        // Ajusta el número de bombas si es mayor que las casillas
        if (width * height < bombsNumber)
        {
            bombsNumber = width * height;
        }

        // Coloca bombas de forma aleatoria
        for (int i = 0; i < bombsNumber; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if (!map[x][y].GetComponent<Piece>().isBomb())
            {
                map[x][y].GetComponent<Piece>().setBomb(true);
            }
            else
            {
                i--; // Reintentar si ya hay bomba
            }
        }
    }


    public int getBombsAround(int x, int y)
    {
        int count = 0;

        // Esquina superior izquierda
        if (x > 0 && y < (height - 1) && map[x - 1][y + 1].GetComponent<Piece>().isBomb())
        {
            count++;
        }

        // Encima
        if (y < (height - 1) && map[x][y + 1].GetComponent<Piece>().isBomb())
        {
            count++;
        }

        // Esquina superior derecha
        if (x < (width - 1) && (y < height - 1) && map[x + 1][y + 1].GetComponent<Piece>().isBomb())
        {
            count++;
        }

        // Izquierda
        if (x > 0 && map[x - 1][y].GetComponent<Piece>().isBomb())
        {
            count++;
        }
        // Derecha
        if (x < width - 1 && map[x + 1][y].GetComponent<Piece>().isBomb())
        {
            count++;
        }

        // Esquina inferior izquierda
        if (x > 0 && y > 0 && map[x - 1][y - 1].GetComponent<Piece>().isBomb())
        {
            count++;
        }

        // Debajo
        if (y > 0 && map[x][y - 1].GetComponent<Piece>().isBomb())
        {
            count++;
        }

        // Esquina inferior derecha
        if (x < (width - 1) && y > 0 && map[x + 1][y - 1].GetComponent<Piece>().isBomb())
        {
            count++;
        }


        return count;
    }

    public void EmptyPiecesCheck(int x, int y)
    {
        // Esquina superior izquierda
        if (x > 0 && y < (height - 1))
        {
            map[x - 1][y + 1].GetComponent<Piece>().drawbomb();
        }

        // Encima
        if (y < (height - 1))
        {
            map[x][y + 1].GetComponent<Piece>().drawbomb();
        }

        // Esquina superior derecha
        if (x < (width - 1) && (y < height - 1))
        {
            map[x + 1][y + 1].GetComponent<Piece>().drawbomb();
        }

        // Izquierda
        if (x > 0)
        {
            map[x - 1][y].GetComponent<Piece>().drawbomb();
        }
        // Derecha
        if (x < width - 1)
        {
            map[x + 1][y].GetComponent<Piece>().drawbomb();
        }

        // Esquina inferior izquierda
        if (x > 0 && y > 0)
        {
            map[x - 1][y - 1].GetComponent<Piece>().drawbomb();
        }

        // Debajo
        if (y > 0)
        {
            map[x][y - 1].GetComponent<Piece>().drawbomb();
        }

        // Esquina inferior derecha
        if (x < (width - 1) && y > 0)
        {
            map[x + 1][y - 1].GetComponent<Piece>().drawbomb();
        }
    }

    public void triggerAllBombs()
    {

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (map[i][j].GetComponent<Piece>().isBomb())
                {
                    map[i][j].GetComponent<Piece>().triggerBomb();
                }
            }
        }
    }

    
    
    public void ClearBoard()
    {
        if (boardContainer != null)
        {
            foreach (Transform child in boardContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }

}
