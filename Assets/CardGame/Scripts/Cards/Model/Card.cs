using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Card
{
    public string ID;
    public string Name;
    public string Text;

    public Card(string name, string text)
    {
        this.Name = name;
        this.Text = text;
        this.ID = System.Guid.NewGuid().ToString();
    }
}

