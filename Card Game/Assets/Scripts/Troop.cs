using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Troop", menuName = "Cards/Troop", order = 1)]
public class Troop : Card, TroopInterface
{
    public virtual void SampleMethod()
    {
        Debug.Log("Hello 1");
    }

    public int attackRight;
    public int attackLeft;
    public int attackUp;
    public int attackDown;
    
    public virtual void MoveToZone()
    {
    }


}

interface TroopInterface
{
    void SampleMethod();
}
