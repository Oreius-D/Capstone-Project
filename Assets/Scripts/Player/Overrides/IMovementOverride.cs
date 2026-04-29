using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementOverride
{
    // Bool method to check if the movement override is currently active, allowing other components to determine whether the movement override should be applied or not.
    bool TryOverrideMovement(in PlayerFrame f);
}
