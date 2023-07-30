using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrominoLogic : MonoBehaviour
{
    private float lastTime;
    public float fallTime = 0.8f;

    public static int height = 20;
    public static int width = 10;

    public Vector3 rotationPoint;

    private static Transform[,] grid = new Transform[width, height];

    public static int score = 0;

    public static int difficultyLevel = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            transform.position += new Vector3(-1, 0, 0);

            if (!Limits())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!Limits())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        if(Time.time - lastTime > (Input.GetKeyDown(KeyCode.DownArrow) ? fallTime / 20 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!Limits())
            {
                transform.position -= new Vector3(0, -1, 0);

                AddToGrid(); 

                CheckLines();

                this.enabled = false;

                FindObjectOfType<GeneratorLogic>().AddTetromino();
            }

            lastTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            if (!Limits())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            } 
        }

        LevelUp();
        AumentarDificultad();
    }

    bool Limits()
    {
        foreach (Transform hijo in transform)   
        { 
            int enteroX = Mathf.RoundToInt(hijo.transform.position.x);
            int enteroY = Mathf.RoundToInt(hijo.transform.position.y);

            if (enteroX < 0 || enteroX >= width || enteroY < 0 || enteroY >= height)
            {
                return false;
            }
            
            if(grid[enteroX, enteroY] != null)  
            {
                return false;
            }
        }
        return true;
    }

    private void AddToGrid()
    {
        foreach (Transform hijo in transform)
        {
            int enteroX = Mathf.RoundToInt(hijo.transform.position.x);
            int enteroY = Mathf.RoundToInt(hijo.transform.position.y);

            grid[enteroX, enteroY] = hijo; 

            if(enteroY >= 19)
            {
                score = 0;
                difficultyLevel = 0;
                fallTime = 0.8f;

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                RemoveLine(i);
                DownLine(i);
            }
        }
    }

    private bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j,i] == null)
            {
                return false;
            }
        }

        score += 100;
        return true;
    }

    private void RemoveLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    private void DownLine(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null) 
                {
                    grid[j,y-1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void LevelUp()
    {
        switch (score)
        {
            case 200:
                difficultyLevel = 1;
                break;
            case 400:
                difficultyLevel = 1;
                break;
        }
    }

    void AumentarDificultad()
    {
        switch (difficultyLevel)
        {
            case 1:
                fallTime = 0.4f;
                break;
            case 2:
                fallTime = 0.2f;
                break;
        }
    }
}
