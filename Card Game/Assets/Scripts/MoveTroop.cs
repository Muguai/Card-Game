using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoveTroop : NetworkBehaviour
{
    public float moveSpeed;
    public DisplayMoveArrows moveArrows;
    private bool moving { get; set; }
    private GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GetComponent<DragDrop>().Canvas;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(transform.parent.gameObject.name != "PlayerArea" && transform.parent.gameObject.name != "OpponentArea")
            {
                moveArrows.DisplayArrows();
            }
        }
    }

    public void MoveToZone(GameObject Zone)
    {
        transform.SetParent(Canvas.transform, true);
        SpawnGrid.Instance.cardInDropZones.Remove(gameObject);
        StartCoroutine(MoveTowardsZone(moveSpeed, Zone));
       
    }

    private IEnumerator MoveTowardsZone(float moveSpeed, GameObject Zone)
    {
        moving = true;

        while (Vector2.Distance(transform.position, Zone.transform.position) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, Zone.transform.position, moveSpeed);
            yield return null;
        }

        transform.position = Zone.transform.position;
        transform.SetParent(Zone.transform, true);
        SpawnGrid.Instance.cardInDropZones.Add(gameObject, Zone);
        moveArrows.DisplayArrows();

        moving = false;
    }
}
