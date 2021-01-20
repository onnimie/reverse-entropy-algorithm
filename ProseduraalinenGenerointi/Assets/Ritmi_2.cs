using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritmi_2 : MonoBehaviour
{
    private readonly float unit = 0.1f;
    private Dictionary<Vector2, GameObject> Grid = new Dictionary<Vector2, GameObject>();
    public GameObject black;
    public GameObject white;
    public int xBound;
    public int yBound;
    public int density;

    public int iterationAmount;
    public int taikaluku = 4;
    public bool NegroBounds = true;

    private GameObject holder;

    private void CreateNoiseGrid(int density)
    {
        Grid.Clear();
        for (int x = 0; x < xBound; x++)
        {
            for (int y = 0; y < yBound; y++)
            {
                if (density > Random.Range(1, 100))
                {
                    Grid.Add(new Vector2(x, y), black);
                } else
                {
                    Grid.Add(new Vector2(x, y), white);
                }
            }
        }
    }

    private void IterateGrid()
    {
        Dictionary<Vector2, GameObject> tempGrid = new Dictionary<Vector2, GameObject>(Grid);
        foreach (Vector2 key in tempGrid.Keys)
        {
            int mustia = 0;
            int count = 0;
            foreach (Vector2 neighbour in GetNeighbourLocations(key))
            {
                if (!IsOutOfBounds(neighbour))
                {
                    if (tempGrid[neighbour].tag == "musta")
                    {
                        mustia++;
                    }
                } else
                {
                    if (NegroBounds)
                    {
                        mustia++;
                    }
                }
                count++;
            }

            //Debug.Log(""+ mustia+" ja count on " + count);

            if (mustia > taikaluku)
            {
                Grid[key] = black;
            } else
            {
                Grid[key] = white;
            }
        }
    }

    private void GenerateGrid()
    {
        foreach (Vector2 key in Grid.Keys)
        {
            Instantiate(Grid[key], key * unit, new Quaternion(0, 0, 0, 0), holder.GetComponent<Transform>());
        }
    }

    private List<Vector2> GetNeighbourLocations(Vector2 location)
    {
        List<Vector2> vektorit = new List<Vector2>();

        for (float x = location.x - 1; x < location.x + 1.5f; x++)
        {
            for (float y = location.y - 1; y < location.y + 1.5f; y++)
            {
                Vector2 currentPos = new Vector2(x, y);
                if (currentPos != location)
                {
                    vektorit.Add(currentPos);
                }
            }
        }
        return vektorit;
    }

    private bool IsOutOfBounds(Vector2 location) { return (location.x < 0 || location.x > xBound - 1 || location.y < 0 || location.y > yBound - 1); }

    private void ClearHolder()
    {
        foreach (Transform child in holder.transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != holder)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void Start()
    {
        holder = GameObject.FindGameObjectWithTag("grid");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearHolder();
            CreateNoiseGrid(density);
            GenerateGrid();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ClearHolder();
            IterateGrid();
            GenerateGrid();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearHolder();
            CreateNoiseGrid(density);
            for (int i = 0; i < iterationAmount; i++)
            {
                IterateGrid();
            }
            GenerateGrid();
        }
    }
}
