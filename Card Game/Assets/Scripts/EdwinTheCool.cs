using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Edwin", menuName = "Specific Cards/Edwin", order = 1)]
public class EdwinTheCool : Troop
{
    public override void SampleMethod()
    {
        base.SampleMethod();
        Debug.Log("Hello 2");
    }
}
