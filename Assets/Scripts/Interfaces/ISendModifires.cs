using System.Collections.Generic;

internal interface ISendModifires
{
    void Apply(PlayerStats stats);
    void Remove(PlayerStats stats);
}