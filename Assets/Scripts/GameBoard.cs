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
    

    private void Awake()
    {
        cam.orthographicSize = arraySize / 2f;

        Vector3 newCameraPosition = new Vector3(arraySize / 2f, arraySize / 2f - 0.4f, cam.transform.position.z);
        cam.transform.position = newCameraPosition;

        board = new Cell[arraySize][];
        futureGen = new Cell[arraySize][];
        // create grid of cells
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new Cell[arraySize];

            for (int j = 0; j< board[i].Length; j++)
            {
                board[i][j] = Instantiate<Cell>(cell, new Vector3(i, j, 0f), Quaternion.identity);
                
            }
        }
        //Board has been created. Now, we need to loop through this array again and find out how many neighbors they have.
        //Fill Cell Neighbors
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
        //Need to check the rules of life first on the seed before moving to the next generation.









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

    public void GameBoardUpdate()
    {
        //have this update the game board every second. Not every frame.
    }
}
