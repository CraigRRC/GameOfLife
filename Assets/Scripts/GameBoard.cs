using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameBoard : MonoBehaviour
{
    //Size of the array
    public int arraySize = 25;
    //Camera
    public Camera cam;
    //Generation number that is on the UI.
    public TextMeshProUGUI generationNumber;
    //How I count the generation internally.
    public int generationCounter = 1;
    //The text box for array size.
    public TMP_InputField arraySizeUI;
    //2D array of cells representing the current generation.
    public Cell[][] board;
    //2D array of cells representing the next generation.
    public Cell[][] futureGen;
    //Represents one cell.
    public Cell cell;
    //Control the flow when we are paused.
    public bool paused = false;

    private void Awake()
    {
        //Set our target frame rate to 4 FPS.
        Application.targetFrameRate = 4;
        SetupCamera();
        //Generate our game boards based on array size.
        board = new Cell[arraySize][];
        futureGen = new Cell[arraySize][];
        CreateInitialGrid();
        CreateNextGeneration();   
        FillCellNeighbors();        
        CountNeighbours();
        RulesOfLife();
    }

    //Updates the state of our game board for the next generation.
    private void UpdateStateOfBoard()
    {
        generationCounter++;
        generationNumber.text = generationCounter.ToString();
        for (int i = 0; i < futureGen.Length; i++)
        {
            for (int j = 0; j < futureGen[i].Length; j++)
            {
                board[i][j] = futureGen[i][j];
            }
        }
    }

    //Fill our next generation with our current generation cells.
    private void CreateNextGeneration()
    {
        //if (!arraySizeUI.text.Equals(String.Empty))
        //{
        //    arraySize = Int32.Parse(arraySizeUI.text);
        //}

        //Create new board based on the current generation board to put the calculation results into.
        for (int i = 0; i < board.Length; i++)
        {
            futureGen[i] = new Cell[arraySize];

            for (int j = 0; j < board[i].Length; j++)
            {
                futureGen[i][j] = board[i][j];
                board[i][j].ResetNeigbourCounts();
            }
        }
    }

    //Performs the rules of life calculations.
    private void RulesOfLife()
    {
        // Determine if that cell should live or die based on it's neighbours.
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                if (board[i][j].GetCellState() == CellState.Alive)
                {
                    futureGen[i][j].SetCellColor(Color.yellow);

                    if (board[i][j].GetLiveNeighbourCount() < 2)
                    {
                        //UnderPopulation
                        KillFutureCell(i, j);
                    }
                    else if (board[i][j].GetLiveNeighbourCount() == 2 || board[i][j].GetLiveNeighbourCount() == 3)
                    {
                        //Lives on
                        FutureCellLives(i, j);
                    }
                    else if (board[i][j].GetLiveNeighbourCount() > 3)
                    {
                        //OverPopulation
                        KillFutureCell(i, j);
                    }
                }
                else
                {
                    if (board[i][j].GetLiveNeighbourCount() == 3)
                    {
                        //Reproduction.
                        FutureCellLives(i, j);
                    }
                }
            }
        }
    }
    
    //Turn future cells "on"
    private void FutureCellLives(int i, int j)
    {
        futureGen[i][j].SetCellState(CellState.Alive);
        futureGen[i][j].SetCellColor(Color.yellow);
    }

    //Turn future cells "off"
    private void KillFutureCell(int i, int j)
    {
        futureGen[i][j].SetCellState(CellState.Dead);
        futureGen[i][j].SetCellColor(Color.gray);
    }


    //Count how many live/dead neighbours each cell has.
    private void CountNeighbours()
    {
        for (int i = 0; i < board.Length; i++)
        {
            //loop through each cell array.
            for (int j = 0; j < board[i].Length; j++)
            {
                //loop through each cell in the cell array.
                for (int k = 0; k < board[i][j].GetCellNeighbours().Length; k++)
                {
                    
                    //loop through each cell neighbour array within each cell.
                    if (board[i][j].GetCellNeighbours()[k] != null)
                    {
                        if (board[i][j].GetCellNeighbours()[k].GetCellState() == CellState.Dead)
                        {
                            board[i][j].AddDeadNeighbourCount(1);
                        }
                        else if(board[i][j].GetCellNeighbours()[k].GetCellState() == CellState.Alive)
                        {
                            board[i][j].AddLiveNeighbourCount(1);
                        }
                    }
                    else
                    {
                        //Handling the edge case by making edges dead cells.
                        board[i][j].AddDeadNeighbourCount(1);
                    }
                }
            }
        }
    }

    //Fill our neighbour array with cell neighbours.
    private void FillCellNeighbors()
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                //Create neighbors array.
                board[i][j].SetCellNeighbours(new Cell[8]);

                //Fill neighbors array with all neighbors.
                if (i - 1 >= 0 && j + 1 <= arraySize - 1)
                {
                    board[i][j].GetCellNeighbours()[0] = board[i - 1][j + 1]; //top left
                }
                if (j + 1 <= arraySize - 1)
                {
                    board[i][j].GetCellNeighbours()[1] = board[i][j + 1];     //top
                }
                if (i + 1 <= arraySize - 1 && j + 1 <= arraySize - 1)
                {
                    board[i][j].GetCellNeighbours()[2] = board[i + 1][j + 1]; //top right
                }
                if (i + 1 <= arraySize - 1)
                {
                    board[i][j].GetCellNeighbours()[3] = board[i + 1][j];     //right
                }
                if (i + 1 <= arraySize - 1 && j - 1 >= 0)
                {
                    board[i][j].GetCellNeighbours()[4] = board[i + 1][j - 1]; //bottom right
                }
                if (j - 1 >= 0)
                {
                    board[i][j].GetCellNeighbours()[5] = board[i][j - 1];     //bottom
                }
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    board[i][j].GetCellNeighbours()[6] = board[i - 1][j - 1]; //bottom left
                }
                if (i - 1 >= 0)
                {
                    board[i][j].GetCellNeighbours()[7] = board[i - 1][j];     //left
                }
            }
        }
    }

    //Starting grid of all dead cells.
    public void CreateInitialGrid()
    {
        
        generationCounter = 1;
        generationNumber.text = generationCounter.ToString();
        // Create grid of cells.
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new Cell[arraySize];

            for (int j = 0; j < board[i].Length; j++)
            {
                board[i][j] = Instantiate<Cell>(cell, new Vector3(i, j, 0f), Quaternion.identity);
                CurrentCellDies(i, j);

            }
        }
    }

   
    //UnityEvent callback function that creates a random grid of alive/dead cells.
    public void RandomGrid()
    {
        generationCounter = 1;
        generationNumber.text = generationCounter.ToString();
        // Create grid of cells.
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new Cell[arraySize];

            for (int j = 0; j < board[i].Length; j++)
            {
                board[i][j] = Instantiate<Cell>(cell, new Vector3(i, j, 0f), Quaternion.identity);
                int decider = UnityEngine.Random.Range(0, 2);
                if (decider == 0)
                {
                    CurrentCellLives(i, j);
                }
                else
                {
                    CurrentCellDies(i, j);
                }
            }
        }
    }

    //Turning "off" a current generation cell.
    private void CurrentCellDies(int i, int j)
    {
        board[i][j].SetCellState(CellState.Dead);
        board[i][j].SetCellColor(Color.gray);
    }
    //Turning "on" a current generation cell.
    private void CurrentCellLives(int i, int j)
    {
        board[i][j].SetCellState(CellState.Alive);
        board[i][j].SetCellColor(Color.yellow);
    }

    //Set the camera to follow the size of the array.
    private void SetupCamera()
    {
        cam.orthographicSize = arraySize / 2f;

        Vector3 newCameraPosition = new Vector3(arraySize / 2f, arraySize / 2f - 0.4f, cam.transform.position.z);
        cam.transform.position = newCameraPosition;
    }

    //Only update the game board if the game isnt paused.
    private void Update()
    {
        MouseClicked();

        if (paused) return;
        GameBoardUpdate();
    }

    //Flips the state of a cell when the user clicks the left mouse button over the cell.
    private void MouseClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseToWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            if (MouseWithinTheBoundsOfArray(mouseToWorldPoint))
            {

                if (MouseClickedOnLivingCell(mouseToWorldPoint))
                {
                    ClickedOnCellDies(mouseToWorldPoint);
                }
                else
                {
                    ClickedOnCellLives(mouseToWorldPoint);
                }
            }
        }
    }

    //If we click on this cell, it lives.
    private void ClickedOnCellLives(Vector3 mouseToWorldPoint)
    {
        futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].SetCellState(CellState.Alive);
        futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].SetCellColor(Color.yellow);
    }

    //If we click on this cell, it dies.
    private void ClickedOnCellDies(Vector3 mouseToWorldPoint)
    {
        futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].SetCellState(CellState.Dead);
        futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].SetCellColor(Color.gray);
    }
    //Determine if the cell we click on is alive.
    private bool MouseClickedOnLivingCell(Vector3 mouseToWorldPoint)
    {
        return board[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].GetCellState() == CellState.Alive;
    }

    //Determine if the cell we clicked on is within the bounds of the array.
    private bool MouseWithinTheBoundsOfArray(Vector3 mouseToWorldPoint)
    {
        return Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x)) < arraySize &&
            Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x)) >= 0 &&
            Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y)) < arraySize &&
            Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y)) >= 0;
    }

    //Update the game board based on the future generation calculations.
    public void GameBoardUpdate()
    {
        CreateNextGeneration();
        FillCellNeighbors();
        CountNeighbours();
        RulesOfLife();
        UpdateStateOfBoard();
    }

    //UnityEvent callback function that pauses the game
    public void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
    }

    //UnityEvent callback function that resumes the game
    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
    }

    //UnityEvent callback function that steps through one generation.
    public void Step()
    {
        paused = true;
        Time.timeScale = 1;
        GameBoardUpdate();
        Time.timeScale = 0;
    }
}
