﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Interface for variables used in cards
public interface ICardVariable
{
    string Name { get; }
    string Description { get; }
    string Identifier { get; }

    string GetValue(int args0 = -1);

    Regex GetRegex();
}
