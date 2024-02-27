using UnityEngine;

public class TileRow : MonoBehaviour
{
    public Cells[] cells { get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<Cells>();
    }
}
