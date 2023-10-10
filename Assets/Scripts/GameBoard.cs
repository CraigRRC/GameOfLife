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
    public Cell cell;

    private void Awake()
    {
        cam.orthographicSize = arraySize / 2f;

        Vector3 newCameraPosition = new Vector3(arraySize / 2f, arraySize / 2f - 0.4f, cam.transform.position.z);
        cam.transform.position = newCameraPosition;
        

        board = new Cell[arraySize][];
        // create grid of cells
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new Cell[arraySize];

            for (int j = 0; j< board[i].Length; j++)
            {
                board[i][j] = Instantiate<Cell>(cell, new Vector3(i, j, 0f), Quaternion.identity);
            }
        }
    }

    public void GameBoardUpdate()
    {
        //have this update the game board every second. Not every frame.
    }
}
