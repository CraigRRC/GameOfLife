using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Cell[] cellNeighbors;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

       
    }

    private void Start()
    {
   
    }


}
