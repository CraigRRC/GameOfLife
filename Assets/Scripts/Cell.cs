using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer cellSpriteRenderer;
    public Cell[] cellNeighbors;
    public CellState cellState;
    public int liveNeighbourCount = 0;
    public int deadNeighbourCount = 0;

    public void Awake()
    {
        cellSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {

    }

    public void SetCellColor(Color color) { this.cellSpriteRenderer.color = color; }
    public CellState GetCellState() {  return cellState; }
    public void SetCellState(CellState cellState) { this.cellState = cellState; }

    public Color GetSpriteRenderer() { return cellSpriteRenderer.color;}

}

public enum CellState
{
    Dead,
    Alive
}
