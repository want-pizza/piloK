using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanBePaused 
{
    public void OnPausedChanged(bool paused);
}
