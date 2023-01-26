using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DragDrop : NetworkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public GameObject Canvas;
    [HideInInspector] public GameObject DropZoneHolder;

    private GridLayoutGroup glg;
    private SpawnGrid sg;
    private Color orgColorDropZone;
    private bool isDraggable = true;
    private bool isOverDropZone = false;
    private bool isDragging = false;
    private GameObject dropZone = null;
    private GameObject startParent;
    private Vector2 startPos;
    private RectTransform rect;
    private Vector2 originalScale;

    private List<GameObject> dropZonesOver = new List<GameObject>();

    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        DropZoneHolder = Canvas.transform.Find("DropZoneHolder").gameObject;
    }

    private void Start()
    {      
        glg = DropZoneHolder.GetComponent<GridLayoutGroup>();
        sg = DropZoneHolder.GetComponent<SpawnGrid>();
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;


        orgColorDropZone = DropZoneHolder.transform.GetChild(0).gameObject.GetComponent<Image>().color;
        if (!hasAuthority)
        {
            isDraggable = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Add check that only adds legit dropZones. So no zones where cards are already in or that are blocked or on opponents side etc..
        if(col.tag == "DropZone")
        {
            GameObject temp = col.gameObject;
            if (dropZonesOver.Contains(temp) == false)
            {
                dropZonesOver.Add(temp);
            }
            if (dropZone == null && isDragging) 
            {
                dropZone = temp;
                dropZone.GetComponent<Image>().color = Color.blue;
            }

            isOverDropZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "DropZone")
        {
            GameObject temp = col.gameObject;
            if (dropZonesOver.Contains(temp))
            {
                dropZonesOver.Remove(temp);
            }

            if (dropZonesOver.Count == 0f)
            {
                if(dropZone != null)
                {
                    dropZone.GetComponent<Image>().color = orgColorDropZone;
                }
                isOverDropZone = false;
                dropZone = null;
            }
           
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (!isDraggable) return;
        data.pointerDrag = gameObject;
        //  rect.sizeDelta = new Vector2(glg.cellSize.x - 20, glg.cellSize.y - 20);
        float x = (glg.cellSize.x + 20) / sg.OriginalCellSizeX;
        float y = (glg.cellSize.y + 20) / sg.OriginalCellSizeY;
        /*
        if(x > 1f)
        {
            x = 1f;
            y = 1f;
        }
        */
        rect.localScale = new Vector2(x, y);
        isDragging = true;
        startPos = transform.position;
        startParent = transform.parent.gameObject;
        transform.SetParent(Canvas.transform, true);
    }

    public void OnDrag(PointerEventData data)
    {
        if (!isDraggable) return;
        if (data.dragging)
        {
            if(dropZonesOver.Count > 0)
            {
                GameObject temp = null;
                float longest = 200f;
                foreach (GameObject go in dropZonesOver)
                {
                    if (Vector2.Distance(go.transform.position, data.position) < longest)
                    {
                        longest = Vector2.Distance(go.transform.position, data.position);
                        temp = go;
                    }
                }
                if (dropZone != null && temp != null)
                {
                    if (temp.Equals(dropZone) == false)
                    {
                        dropZone.GetComponent<Image>().color = orgColorDropZone;
                        
                        dropZone = temp;
                        Debug.Log("Target " + temp.name);
                        dropZone.GetComponent<Image>().color = Color.blue;
                    }
                }
            }
           
            transform.position = data.position;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (!isDraggable) return;
        data.pointerDrag = null;
        isDragging = false;
        if (isOverDropZone)
        {
            dropZone.GetComponent<Image>().color = orgColorDropZone;
            isDraggable = false;
           // transform.SetParent(dropZone.transform, false);
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            PlayerManager playerManager = networkIdentity.GetComponent<PlayerManager>();
            playerManager.PlayCard(this.gameObject, dropZone.name);



        }
        else
        {
            rect.localScale = originalScale;
            transform.SetParent(startParent.transform, false);
            transform.position = startPos;
        }
    }
}
