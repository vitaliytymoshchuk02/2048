using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Cells : MonoBehaviour
{
    public Vector2Int coordinates;

    public Tile tile;

    public bool empty => tile == null;
    public bool occupied => tile != null;
}
