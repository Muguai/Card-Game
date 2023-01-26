using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject Card1;
    public GameObject Card2;
    private GameObject PlayerArea;
    private GameObject OpponentArea;
    private GameObject DropZoneHolder;


    private List<GameObject> cards = new List<GameObject>();
    // Start is called before the first frame update
    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("PlayerArea");
        OpponentArea = GameObject.Find("OpponentArea");
        DropZoneHolder = GameObject.Find("DropZoneHolder");
    }

    [Server]
    public override void OnStartServer()
    {
        cards.Add(Card1);
        cards.Add(Card2);
    }

    [Command]
    public void CmdDealCards()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject card = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, null, "Dealt");
        }
    }

    public void PlayCard(GameObject card, string dropZone)
    {
        CmdPlayCard(card, dropZone);
    }



    [Command]
    private void CmdPlayCard(GameObject card, string dropZone)
    {
        //this gets nulled for some reason
           
        Debug.Log("Send in this card " + card.name + "To this zone " + dropZone);

        RpcShowCard(card, dropZone, "Played");
    }


    [Command]
    private void CmdSpawnGrid(List<string> names, List<GameObject> zones)
    {
        for(int i = 0; i < zones.Count; i++)
        {
            zones[i].name = names[i];
            zones[i].transform.SetParent(DropZoneHolder.transform, false);
        }
    }

    [ClientRpc]
    private void RpcShowCard(GameObject card, string dropZone ,string type)
    {
        if(type == "Dealt")
        {
            if(hasAuthority)
            {
                card.transform.SetParent(PlayerArea.transform, false);
            }
            else
            {
                card.transform.SetParent(OpponentArea.transform, false);
            }
        }
        else if(type == "Played")
        {
            if (hasAuthority)
            {
                if (SpawnGrid.Instance.dropZones.Count > 0 && dropZone != null)
                {
                    GameObject currentZone = SpawnGrid.Instance.dropZones[dropZone];
                    card.transform.SetParent(currentZone.transform, false);
                    card.transform.position = currentZone.transform.position;
                    SpawnGrid.Instance.cardInDropZones.Add(card, currentZone);
                }
                
            }
            else
            {
                RectTransform rect = card.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(SpawnGrid.Instance.glg.cellSize.x - 20, SpawnGrid.Instance.glg.cellSize.y - 20);
                if (SpawnGrid.Instance.dropZones.Count > 0 && dropZone != null)
                {
                    GameObject currentZone = SpawnGrid.Instance.dropZones[SpawnGrid.Instance.zoneToNameOpponent(dropZone)];
                    card.transform.SetParent(currentZone.transform, false);
                    card.transform.position = currentZone.transform.position;
                    SpawnGrid.Instance.cardInDropZones.Add(card, currentZone);
                }
            }
        }
    }

}
