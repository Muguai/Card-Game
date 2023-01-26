using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Statemachine : MonoBehaviour
{
    protected State state;

    public void SetState(State state)
    {
        this.state = state;
        StartCoroutine(state.Start());
    }
}
