using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    //Cell SpriteRenderer.
    private SpriteRenderer cellSpriteRenderer;
    //Array of neighbouring cells.
    private Cell[] cellNeighbours;
    //Alive or dead state of cell.
    private CellState cellState;
    //How many live neighbours does this cell have
    private int liveNeighbourCount = 0;
    //How many dead neighbours does this cell have
    private int deadNeighbourCount = 0;

    public void Awake()
    {
        cellSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    //Get the state of the cell.
    public CellState GetCellState() { return cellState; }
    //Get our cell neighbour array.
    public Cell[] GetCellNeighbours() { return cellNeighbours; }
    //Get the number of live neighbours this cell has.
    public int GetLiveNeighbourCount() { return liveNeighbourCount; }
    //Set the state of this cell.
    public void SetCellState(CellState cellState) { this.cellState = cellState; }
    //Setup our cell neighbour array.
    public void SetCellNeighbours(Cell[] neighbours) { cellNeighbours = neighbours; }
    //Set the colour of this cell.
    public void SetCellColor(Color color) { this.cellSpriteRenderer.color = color; } 
    //Add live cell neighbours
    public void AddLiveNeighbourCount(int count) {  liveNeighbourCount += count; }
    //Add dead cell neighbours
    public void AddDeadNeighbourCount(int count) {  deadNeighbourCount += count; }
    //Reset cell neighbour count.
    public void ResetNeigbourCounts() { liveNeighbourCount = 0; 
                                        deadNeighbourCount = 0; }

}
//Enum that keeps track of the cell's state of life.
public enum CellState
{
    Dead,
    Alive
}
