using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public Cells[] cells { get; private set; }

    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<Cells>();
    }

    private void Start()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            for (int j = 0; j < rows[i].cells.Length; j++)
            {
                rows[i].cells[j].coordinates = new Vector2Int(i, j);
            }
        }
    }

    public Cells GetRandomEmptyCell()
    {
        int i = Random.Range(0, cells.Length);
        int totalIterations = 0;
        while (cells[i].occupied)
        {
            i = Random.Range(0, cells.Length);

            if (totalIterations > 200)
            {
                return null;
            }
            totalIterations++;
        }
        return cells[i];
    }

    public Cells GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[x].cells[y];
        }
        else { return null; }
    }

    public Cells GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public Cells GetAdjacentCell(Cells cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }
}
