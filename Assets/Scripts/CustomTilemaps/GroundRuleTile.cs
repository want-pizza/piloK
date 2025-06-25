using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GroundRuleTile", menuName = "Tiles/GroundRuleTile")]
public class GroundRuleTile : RuleTile<GroundRuleTile.Neighbor>
{
    // ¬изначаЇмо "ID" сус≥д≥в
    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Ground = 3;  // «наченн€ дл€ тайл≥в земл≥
        public const int NotGround = 4;
    }

    // ѕерев≥р€Їмо чи тайл сус≥да Ч земл€
    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        if (neighbor == Neighbor.Ground)
        {
            return tile is GroundRuleTile; // або перев≥рка на конкретний тип/значенн€
        }
        else if (neighbor == Neighbor.NotGround)
        {
            return !(tile is GroundRuleTile); // протилежна лог≥ка
        }

        return base.RuleMatch(neighbor, tile);
    }
}
