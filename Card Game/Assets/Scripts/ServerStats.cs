using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerStats : NetworkBehaviour
{
    public override void OnStartServer()
    {
        Debug.Log("ServerStats");
    }

    public void OnServerAddPlayer()
    {
        Debug.Log("PlayerAdded");
    }

}
