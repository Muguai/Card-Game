using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject, CardInterface
{
    public new string name;
    public string description;

    public Sprite mainArt;
    public GameObject cardObject;

    public virtual void TestMethod()
    {

    }

}

interface CardInterface
{
    void TestMethod();
}
