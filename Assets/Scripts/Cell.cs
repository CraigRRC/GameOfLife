using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer cellSpriteRenderer;
    public Cell[] cellNeighbors;
    private CellState cellState;

    public void Awake()
    {
       cellSpriteRenderer = GetComponent<SpriteRenderer>();
       cellState = CellState.Dead;
       
    }

    private void Start()
    {
   
    }

    public SpriteRenderer GetCellSpriteRenderer() { return cellSpriteRenderer; }
    public CellState GetCellState() {  return cellState; }

}

public enum CellState
{
    Dead,
    Alive
}
