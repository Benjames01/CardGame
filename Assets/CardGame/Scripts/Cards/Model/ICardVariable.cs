using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardVariable
{
    string Name { get; }
    string Description { get; }
    string Identifier { get; }

    string GetValue();
}
