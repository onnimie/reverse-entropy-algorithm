using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritmi_1 : MonoBehaviour
{
    private readonly float unit = 0.1f;

    private GameObject theGrid;
    private Dictionary<Vector2, GameObject> theGridChildrenDic = new Dictionary<Vector2, GameObject>();
    public int theGridx;
    public int theGridy;

    public List<GameObject> dots;
    public int density = 50;

    public Vector2 locusNeighboursForDebug;
    public int iterationCount;

    // Start is called before the first frame update
    void Start()
    {
        theGrid = GameObject.FindGameObjectWithTag("grid");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ClearGrid();
            GenerateNoise(density);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (theGrid.transform.childCount == 0)
            {
                GenerateNoise(density);
            } else
            {
                Debug.Log("Iteroidaan...");
                IterateWithRitmi();
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            string teksti = "Sijainnin " + locusNeighboursForDebug + " naapurit:\n";
            foreach (Vector2 lolz in GetNeighbourLocations(locusNeighboursForDebug))
            {
                teksti = teksti + lolz + ", ";
            }
            teksti = teksti + "\n";
            /*foreach (Vector2 lolz in GetNeighbourLocations(locusNeighboursForDebug))
            {
                teksti = teksti + theGridChildrenDic[lolz].tag + ", ";
            }*/
            Debug.Log(teksti);
            //Debug.Log("vasen alanaapuri: " + (locusNeighboursForDebug.x - unit, locusNeighboursForDebug.y - unit) + ", " + theGridChildrenDic[new Vector2(locusNeighboursForDebug.x - unit, locusNeighboursForDebug.y - unit)].tag);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < iterationCount; i++)
            {
                IterateWithRitmi();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            List<Vector2> testilista = new List<Vector2>();
            for (float x = 0; x < theGridx; x++)
            {
                for (float y = 0; y < theGridy; y++)
                {
                    testilista.Add(new Vector2(x * unit, y * unit));
                }
            }

            foreach (Vector2 vektori in testilista)
            {
                if (!theGridChildrenDic.ContainsKey(vektori))
                {
                    Debug.Log("ei ole: " + vektori);
                }
            }

            Debug.Log("tarkastus tehty, hakurakenteessa on kaikki pisteet");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("" + locusNeighboursForDebug + ": " + theGridChildrenDic[locusNeighboursForDebug].tag);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (Vector2 key in theGridChildrenDic.Keys)
            {
                Debug.Log(key);
            }
        }
    }

    private void GenerateNoise(int density)
    {
        for (int x = 0;  x < theGridx; x++)
        {
            for (int y = 0; y < theGridy; y++)
            {
                Vector2 location = new Vector2(x * unit, y * unit);
                bool exchange = density > Random.Range(1, 100);
                if (exchange)
                {
                    theGridChildrenDic.Add(location, dots[1]);
                } else
                {
                    theGridChildrenDic.Add(location, dots[0]);
                }
            }
        }

        GenerateGridWithDic(theGridChildrenDic);
    }

    private void CreateDot(GameObject dot, Vector2 pos)
    {
        Instantiate(dot, pos, new Quaternion(0, 0, 0, 0), theGrid.GetComponent<Transform>());
    }

    private void ClearGrid()
    {
        foreach (Transform child in theGrid.transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != theGrid)
            {
                Destroy(child.gameObject);
            }
        }

        theGridChildrenDic.Clear();
    }

    private void GenerateGridWithDic(Dictionary<Vector2, GameObject> dic)
    {
        foreach (Vector2 key in dic.Keys)
        {
            CreateDot(dic[key], key);
        }
    }

    private void CopyNewGridToMaster(Dictionary<Vector2, GameObject> copy)
    {
        foreach (Vector2 key in copy.Keys)
        {
            theGridChildrenDic.Add(key, copy[key]);
        }
    }

    private List<Vector2> GetNeighbourLocations(Vector2 location)
    {
        List<Vector2> vektorit = new List<Vector2>();

        for (float x = location.x - unit; x < (location.x + unit + unit/10); x = x + unit)
        {
            for (float y = location.y - unit; y < (location.y + unit + unit/10); y = y + unit)
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

    private (int, int) GetNeighbourCountsOfDots01(Vector2 location)
    {
        int mustia = 0;
        int valkoisia = 0;

        foreach(Vector2 vektori in GetNeighbourLocations(location))
        {
            if (!(theGridChildrenDic.ContainsKey(vektori)) || theGridChildrenDic[vektori].tag == "musta")
            {
                mustia++;
            }
            else
            {
                valkoisia++;
            }
        }
         
        //Debug.Log("ol: " + mustia + " ja v: " + valkoisia + "   YHT: " + (mustia+valkoisia));
        

        return (valkoisia, mustia);
    }

    private void IterateWithRitmi()
    {
        
        Dictionary<Vector2, GameObject> newGrid = new Dictionary<Vector2, GameObject>();

        foreach (Vector2 key in theGridChildrenDic.Keys)
        {
            (int, int) valuePair01 = GetNeighbourCountsOfDots01(key);
            if (valuePair01.Item2 > 4)
            {
                newGrid.Add(key, dots[1]);
            } else
            {
                newGrid.Add(key, dots[0]);
            }
        }

        ClearGrid();
        CopyNewGridToMaster(newGrid);
        GenerateGridWithDic(newGrid);
    }
}



