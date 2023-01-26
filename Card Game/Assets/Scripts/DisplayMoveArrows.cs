using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMoveArrows : MonoBehaviour
{
    public Button Up, Down, Left, Right;
    private MoveTroop moveTroop;
    GameObject currentZone;

    // Start is called before the first frame update
    void Start()
    {
        Up.onClick.AddListener(delegate { ArrowClicked(0); });
        Down.onClick.AddListener(delegate { ArrowClicked(1); });
        Left.onClick.AddListener(delegate { ArrowClicked(2); });
        Right.onClick.AddListener(delegate { ArrowClicked(3); });
        moveTroop = transform.parent.GetComponent<MoveTroop>();
     //   sg = transform.parent.GetComponent<DragDrop>().DropZoneHolder.GetComponent<SpawnGrid>();


    }

    public void ArrowClicked(int index)
    {
        HideArrows();
        if(currentZone != null)
        {
            Debug.Log("Move to " + SpawnGrid.Instance.GetAdjacentZones(currentZone, index));
            moveTroop.MoveToZone(SpawnGrid.Instance.GetAdjacentZones(currentZone, index));
        }
        
    }

    public void DisplayArrows()
    {
        //ugly but prob works

        currentZone = SpawnGrid.Instance.cardInDropZones[moveTroop.gameObject];
        

        Debug.Log("Current Zone = " + currentZone.name);

        bool[] adjacent = SpawnGrid.Instance.IsAdjacent(currentZone);

        Up.gameObject.SetActive(adjacent[0]);
        Down.gameObject.SetActive(adjacent[1]);
        Left.gameObject.SetActive(adjacent[2]);
        Right.gameObject.SetActive(adjacent[3]);
    }

    public void HideArrows()
    {
        Up.gameObject.SetActive(false);
        Down.gameObject.SetActive(false);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
    }

}
