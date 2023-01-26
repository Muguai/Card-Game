using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Mirror;

public class SpawnGrid : NetworkBehaviour
{

    public GameObject zonePrefab;

    [SyncVar]
    public int Ygrid;

    [SyncVar]
    public int Xgrid;


    public int OriginalCellSizeX;
    public int OriginalCellSizeY;

    [HideInInspector]public GridLayoutGroup glg;
    [HideInInspector]public Dictionary<string, GameObject> dropZones = new Dictionary<string, GameObject>();
    [HideInInspector] public Dictionary<GameObject, GameObject> cardInDropZones = new Dictionary<GameObject, GameObject>();
    [HideInInspector]public GameObject currentZone;


    public static SpawnGrid Instance { get; private set; }

    private void Awake()
    {
        glg = GetComponent<GridLayoutGroup>();

        Instance = this;

    }

    public override void OnStartClient()
    {
        Debug.Log("Gets there");
        base.OnStartClient();

        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        dropZones.Clear();
        cardInDropZones.Clear();


        glg.cellSize = new Vector2(OriginalCellSizeX, OriginalCellSizeY);
        glg.constraintCount = Ygrid;

        if (Ygrid > 3)
        {
            glg.cellSize = new Vector2(glg.cellSize.x - (10 * Ygrid - 3), glg.cellSize.y - (10 * Ygrid - 3));
        }
        else if (Xgrid > 8)
        {
            glg.cellSize = new Vector2(glg.cellSize.x - (7 * Xgrid - 8), glg.cellSize.y - (7 * Xgrid - 8));

        }


        for (int y = 1; y < Ygrid + 1; y++)
        {
            for (int x = 1; x < Xgrid + 1; x++)
            {


                int[] grid = { y, x };
                GameObject zone = Instantiate(zonePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                zone.name = "Y: " + grid[0] + " X: " + grid[1];
               // NetworkServer.Spawn(zone);
                dropZones.Add(zone.name, zone);
                zone.transform.SetParent(this.transform, false);
                

            }
            
        }

        /*
        glg.CalculateLayoutInputHorizontal();
        glg.CalculateLayoutInputVertical();
        glg.SetLayoutHorizontal();
        glg.SetLayoutVertical();
        */

        Vector2 boxColliderSize = new Vector2(glg.cellSize.x, glg.cellSize.y);
        BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D col in colliders)
        {
            col.size = boxColliderSize;
        }

    }

    public int[] zoneNameToIntArray(string name)
    {
        
        string digits = AllNumbersInString(name);

        int parsedDigits = int.Parse(digits); 
        int y = parsedDigits / 10;
        int x = parsedDigits % 10;
        int[] gridArray = { y , x };
        return gridArray;
    }

    public string zoneToNameOpponent(string name)
    {
        int[] tempGrid = zoneNameToIntArray(name);
        int numberY = tempGrid[0] + ((2 * (Ygrid - tempGrid[0])) - (Ygrid - 1));
        int numberX = tempGrid[1] + ((2 * (Xgrid - tempGrid[1])) - (Xgrid - 1));
        Debug.Log(name + " Became " + "Y: " + numberY + " X: " + numberX);
        int[] gridArray = { numberY, numberX };
        return "Y: " + numberY + " X: " + numberX;
    }

    public bool[] IsAdjacent(GameObject zone)
    {
       bool[] adjacency = new bool[4];

       for(int i = 0; i < adjacency.Length; i++)
        {
            adjacency[i] = true;
        }
       
       int[] gridArray = zoneNameToIntArray(zone.name);
     

       if (gridArray[0] == Ygrid)
       {
            adjacency[0] = false;
       }

       if (gridArray[0] == 1)
       {
           adjacency[1] = false;
       }

        if (gridArray[1] == 1)
        {
            adjacency[2] = false;
        }

        if (gridArray[1] == Xgrid)
        {
            adjacency[3] = false;
        }

        return adjacency;

    }


    public GameObject GetAdjacentZones(GameObject zone, int UpDownLeftRight)
    {
        // 0 = Up, 1 = Down , 2 = Left , 3 = Right

        int[] gridArray = zoneNameToIntArray(zone.name);
        string use = "";       
        switch (UpDownLeftRight)
        {
            case 0:
                use = "Y: " + (gridArray[0] + 1) + " X: " + gridArray[1];
                break;
            case 1:
                use = "Y: " + (gridArray[0] - 1) + " X: " + gridArray[1];
                break;
            case 2:
                use = "Y: " + gridArray[0] + " X: " + (gridArray[1] - 1);
                break;
            case 3:
                use = "Y: " + gridArray[0] + " X: " + (gridArray[1] + 1);
                break;
        }

        GameObject adjacent = dropZones[use];

        return adjacent;

    }

    private string AllNumbersInString(string name)
    {
        string result = "";
        foreach (char c in name)
        {
            if (char.IsNumber(c))
            {
                result += c;
            }
        }
        return result;
    }
}
