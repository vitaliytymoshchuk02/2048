using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
    public Tile tilePrefab;
    private TileGrid grid;
    private List<Tile> tiles;
    private bool waiting;

    float spawnProbability = 0.9f;

    [SerializeField] private Sprite _2;
    [SerializeField] private Sprite _4;
    [SerializeField] private Sprite _8;
    [SerializeField] private Sprite _16;
    [SerializeField] private Sprite _32;
    [SerializeField] private Sprite _64;
    [SerializeField] private Sprite _128;
    [SerializeField] private Sprite _256;
    [SerializeField] private Sprite _512;
    [SerializeField] private Sprite _1024;
    [SerializeField] private Sprite _2048;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }
    void Start()
    {
        GenerateInEmptyCell();
        GenerateInEmptyCell();
        //TestGenerate();
        //TestGenerate();
    }

    private void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
        }

    }
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                Cells cell = grid.GetCell(x, y);
                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if(changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cells newCell = null;
        Cells adjacentCell = grid.GetAdjacentCell(tile.cell, direction);
        while (adjacentCell != null)
        {
            if (adjacentCell.occupied)
            {
                if(CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }
                break;
            }
            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        if(a.number == b.number && !b.locked) return true;
        else return false;
    }
    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);
        int number = b.number * 2;
        Sprite sprite = GetSprite(number);
        b.SetState(sprite, number);

        GameManager.instance.IncreaseScore(number);
        if(number == 2048)
        {
            GameManager.instance.Finish();
        }
    }
    private void GenerateInEmptyCell()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);

        if (Random.value < spawnProbability)
        {
            tile.SetState(_2, 2);
        }
        else
        {
            tile.SetState(_4, 4);
        }

        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    private void TestGenerate()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);

        tile.SetState(_1024, 1024);

        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    private Sprite GetSprite(int number)
    {
        switch (number)
        {
            case 2:
                return _2;
            case 4:
                return _4;
            case 8:
                return _8;
            case 16:
                return _16;
            case 32:
                return _32;
            case 64:
                return _64;
            case 128:
                return _128;
            case 256:
                return _256;
            case 512:
                return _512;
            case 1024:
                return _1024;
            case 2048:
                return _2048;
            default: Debug.Log("Can not identify the number!"); return null;
        }
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;

        if (tiles.Count != grid.size)
        {
            GenerateInEmptyCell();
        }

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (CheckForGameOver())
        {
            GameManager.instance.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if(tiles.Count < grid.size)
        {
            return false;
        }
        else
        {
            foreach(var tile in tiles)
            {
                Cells up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
                Cells down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
                Cells left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
                Cells right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

                if (up != null && CanMerge(up.tile, tile))
                {
                    return false;
                }
                if (down != null && CanMerge(down.tile, tile))
                {
                    return false;
                }
                if (left != null && CanMerge(left.tile, tile))
                {
                    return false;
                }
                if (right != null && CanMerge(right.tile, tile))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}