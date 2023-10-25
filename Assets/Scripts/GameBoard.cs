using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int arraySize = 25;
    public Camera cam;
    public float boardTimer = 0f;
    public TextMeshProUGUI generationNumber;
    public int generationCounter = 1;
    
    public Cell[][] board;
    public Cell[][] futureGen;
    public Cell cell;
    public float timer = 0f;
    public bool paused = false;

    private void Awake()
    {
        Application.targetFrameRate = 4;
        SetupCamera();
        board = new Cell[arraySize][];
        futureGen = new Cell[arraySize][];
        CreateInitialGrid();
        CreateNextGeneration();
        // Board has been created. Now, we need to loop through this array again and find out how many neighbors they have.
        // Fill Cell Neighbors
        FillCellNeighbors();
        // Need to check the rules of life first on the seed before moving to the next generation.
        // Loop through board and grab the neighbours in the cell. 
        // Loop through the neighbours and grab their enum status.
        // If the neighbours are dead, consider making them null to make the determination much much easier. 
        CountNeighbours();
        RulesOfLife();
    }

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

    private void CreateNextGeneration()
    {
        //Create new board based on the current generation board to put the calculation results into.
        for (int i = 0; i < board.Length; i++)
        {
            futureGen[i] = new Cell[arraySize];

            for (int j = 0; j < board[i].Length; j++)
            {
                futureGen[i][j] = board[i][j];
                board[i][j].liveNeighbourCount = 0;
                board[i][j].deadNeighbourCount = 0;
            }
        }
    }

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

                    if (board[i][j].liveNeighbourCount < 2)
                    {
                        //UnderPopulation
                        futureGen[i][j].SetCellState(CellState.Dead);
                        futureGen[i][j].SetCellColor(Color.gray);
                    }
                    else if (board[i][j].liveNeighbourCount == 2 || board[i][j].liveNeighbourCount == 3)
                    {
                        //Lives on
                        futureGen[i][j].SetCellState(CellState.Alive);
                        futureGen[i][j].SetCellColor(Color.yellow);
                    }
                    else if (board[i][j].liveNeighbourCount > 3)
                    {
                        //OverPopulation
                        futureGen[i][j].SetCellState(CellState.Dead);
                        futureGen[i][j].SetCellColor(Color.gray);
                    }
                }
                else
                {
                    if (board[i][j].liveNeighbourCount == 3)
                    {
                        //Reproduction.
                        futureGen[i][j].SetCellState(CellState.Alive);
                        futureGen[i][j].SetCellColor(Color.yellow);
                    }
                }
            }
        }
    }
    private void CountNeighbours()
    {
        for (int i = 0; i < board.Length; i++)
        {
            //loop through each cell array.
            for (int j = 0; j < board[i].Length; j++)
            {
                //loop through each cell in the cell array.
                for (int k = 0; k < board[i][j].cellNeighbors.Length; k++)
                {
                    
                    //loop through each cell neighbour array within each cell.
                    if (board[i][j].cellNeighbors[k] != null)
                    {
                        if (board[i][j].cellNeighbors[k].GetCellState() == CellState.Dead)
                        {
                            board[i][j].deadNeighbourCount++;
                        }
                        else if(board[i][j].cellNeighbors[k].GetCellState() == CellState.Alive)
                        {
                            board[i][j].liveNeighbourCount++;
                        }
                    }
                    else
                    {
                        //Edgecase
                        board[i][j].deadNeighbourCount++;
                    }
                }
            }
        }
    }

    private void FillCellNeighbors()
    {
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                //Create neighbors array.
                board[i][j].cellNeighbors = new Cell[8];

                //fill neighbors array with all neighbors.
                if (i - 1 >= 0 && j + 1 <= arraySize - 1)
                {
                    board[i][j].cellNeighbors[0] = board[i - 1][j + 1]; //top left
                }
                if (j + 1 <= arraySize - 1)
                {
                    board[i][j].cellNeighbors[1] = board[i][j + 1];     //top
                }
                if (i + 1 <= arraySize - 1 && j + 1 <= arraySize - 1)
                {
                    board[i][j].cellNeighbors[2] = board[i + 1][j + 1]; //top right
                }
                if (i + 1 <= arraySize - 1)
                {
                    board[i][j].cellNeighbors[3] = board[i + 1][j];     //right
                }
                if (i + 1 <= arraySize - 1 && j - 1 >= 0)
                {
                    board[i][j].cellNeighbors[4] = board[i + 1][j - 1]; //bottom right
                }
                if (j - 1 >= 0)
                {
                    board[i][j].cellNeighbors[5] = board[i][j - 1];     //bottom
                }
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    board[i][j].cellNeighbors[6] = board[i - 1][j - 1]; //bottom left
                }
                if (i - 1 >= 0)
                {
                    board[i][j].cellNeighbors[7] = board[i - 1][j];     //left
                }
            }
        }
    }

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
                board[i][j].SetCellState(CellState.Dead);
                board[i][j].SetCellColor(Color.gray);
              
            }
        }
    }

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
                    board[i][j].SetCellState(CellState.Alive);
                    board[i][j].SetCellColor(Color.yellow);
                }
                else
                {
                    board[i][j].SetCellState(CellState.Dead);
                    board[i][j].SetCellColor(Color.gray);
                }
            }
        }
    }

    private void SetupCamera()
    {
        cam.orthographicSize = arraySize / 2f;

        Vector3 newCameraPosition = new Vector3(arraySize / 2f, arraySize / 2f - 0.4f, cam.transform.position.z);
        cam.transform.position = newCameraPosition;
    }

    private void Update()
    {
        

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseToWorldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            if(Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x)) < arraySize && Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x)) >= 0 && Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y)) < arraySize && Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y)) >= 0)
            {
               
                if (board[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].cellState == CellState.Alive)
                {
                   
                    futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].cellState = CellState.Dead;
                    futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].SetCellColor(Color.gray);
                }
                else
                {
                    futureGen[Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.x))][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].cellState = CellState.Alive;
                    futureGen[Mathf.Max(Mathf.RoundToInt(mouseToWorldPoint.x), 0)][Mathf.Abs(Mathf.RoundToInt(mouseToWorldPoint.y))].SetCellColor(Color.yellow);
                }
            }
        }

        if(paused) return;
        GameBoardUpdate();
    }

    public void GameBoardUpdate()
    {
        //have this update the game board every second. Not every frame.
        CreateNextGeneration();
        FillCellNeighbors();
        CountNeighbours();
        RulesOfLife();
        UpdateStateOfBoard();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        paused = false;
    }

    public void Step()
    {
        paused = true;
        Time.timeScale = 1;
        GameBoardUpdate();
        Time.timeScale = 0;
    }
}
