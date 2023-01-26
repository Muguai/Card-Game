using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DragDrop dragDrop;

    private GameObject Canvas;
    private GameObject showCardCanvas;
    private GameObject zoomCard = null;

    public float biggerX;
    public float biggerY;

    public void Awake()
    {
        dragDrop = this.gameObject.GetComponent<DragDrop>();
        showCardCanvas = GameObject.Find("ShowCardPos"); ;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        return;
        if(showCardCanvas.transform.childCount > 0)
        {
            Destroy(showCardCanvas.transform.GetChild(0).gameObject);
        }
        GameObject copy;
        copy = new GameObject();
        copy = gameObject;
        zoomCard = Instantiate(copy, showCardCanvas.transform.position, Quaternion.identity);
        zoomCard.transform.SetParent(showCardCanvas.transform, true);
        zoomCard.layer = LayerMask.NameToLayer("Zoom");

        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(biggerX, biggerY);

        
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        
    }


}
