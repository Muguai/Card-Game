using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public Troop card;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(card.attackDown);
        card.SampleMethod();
        Debug.Log(card.name);
        card.cardObject = gameObject;
    }

}
