using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public int arraySize = 25;
    public Camera cam;
    public float boardTimer = 0f;
    
    public Cell[][] board;
    public Cell[][] futureGen;
    public Cell cell;
    public float timer = 0f;

    private void Awake()
    {
        SetupCamera();
        board = new Cell[arraySize][];
        futureGen = new Cell[arraySize][];
        CreateInitialGrid();
        CreateNextGeneration();
        //Board has been created. Now, we need to loop through this array again and find out how many neighbors they have.
        //Fill Cell Neighbors
        FillCellNeighbors();
        //Need to check the rules of life first on the seed before moving to the next generation.
        // Loop through board and grab the neighbours in the cell. 
        // Loop through the neighbours and grab their enum status.
        // If the neighbours are dead, consider making them null to make the determination much much easier. 
        CountNeighbours();
        RulesOfLife();
    }

    private void UpdateStateOfBoard()
    {
        for (int i = 0; i < futureGen.Length; i++)
        {
            for (int j = 0; j < futureGen[i].Length; j++)
            {
                board[i][j] = futureGen[i][j];
                board[i][j].liveNeighbourCount = 0;
                board[i][j].deadNeighbourCount = 0;
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
            }
        }
    }

    private void RulesOfLife()
    {
        // Determine if that cell should live or die based on it's neighbours
        for (int i = 0; i < board.Length; i++)
        {
            for (int j = 0; j < board[i].Length; j++)
            {
                if (board[i][j].GetCellState() == CellState.Alive)
                {
                    futureGen[i][j].SetCellColor(Color.yellow);

                    if (board[i][j].liveNeighbourCount < 2)
                    {
                        Debug.Log("length < 2");
                        //UnderPopulation
                        futureGen[i][j].SetCellState(CellState.Dead);
                        futureGen[i][j].SetCellColor(Color.gray);
                    }
                    else if (board[i][j].liveNeighbourCount == 2 || board[i][j].liveNeighbourCount == 3)
                    {
                        Debug.Log("2 or 3");
                        Debug.Log(board[i][j].liveNeighbourCount);
                        //Lives on
                        futureGen[i][j].SetCellColor(Color.yellow);
                        futureGen[i][j].SetCellState(CellState.Alive);
                       
                    }
                    else if (board[i][j].liveNeighbourCount > 3)
                    {
                        Debug.Log("over 3");
                        //OverPopulation
                        futureGen[i][j].SetCellState(CellState.Dead);
                        futureGen[i][j].SetCellColor(Color.gray);
                    }
                }
                else
                {
                    
                    if (board[i][j].deadNeighbourCount == 3)
                    {
                        //Reproduction.
                        futureGen[i][j].SetCellState(CellState.Alive);
                    }

                }

            }
        }
    }

    private void CountNeighbours()
    {
        

        for (int i = 0; i < board.Length; i++)
        {
            //loop through each cell array
            for (int j = 0; j < board[i].Length; j++)
            {
                //loop through each cell in the cell array
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

    private void CreateInitialGrid()
    {
        // Create grid of cells
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new Cell[arraySize];

            for (int j = 0; j < board[i].Length; j++)
            {
                board[i][j] = Instantiate<Cell>(cell, new Vector3(i, j, 0f), Quaternion.identity);
                board[i][j].SetCellState(CellState.Alive);
                board[i][j].SetCellColor(Color.yellow);
              
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
        timer += Time.deltaTime;
        if(timer > 10)
        {
            GameBoardUpdate();
            timer = 0;
        }
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
}
