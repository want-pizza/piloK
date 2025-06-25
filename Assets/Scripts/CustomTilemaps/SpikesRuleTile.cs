using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(fileName = "SpikeRuleTile", menuName = "Tiles/SpikeRuleTile")]
public class SpikeRuleTile : RuleTile<SpikeRuleTile.Neighbor>
{
    // Масив тайлів землі (можна один або кілька)
    public TileBase[] groundTiles;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Ground = 3; // Таке ж число для позначення землі
        public const int NotGround = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        if (neighbor == Neighbor.Ground)
        {
            // Перевіряємо чи tile є одним з groundTiles
            return tile != null && groundTiles.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}
