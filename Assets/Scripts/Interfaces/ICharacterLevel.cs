using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterLevel
{
    Field<int> CurrentLevel { get; }
    Field<int> CurrentXP { get; }

    public void GainXP(int value);
    public int GetXPToNextLevel(int level);
}
