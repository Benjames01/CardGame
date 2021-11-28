﻿using System;
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
        if (string.IsNullOrEmpty((name)) || string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException();
        }
        this.Name = name;
        this.Text = text;
        this.ID = GUID.Generate().ToString();
    }
}

