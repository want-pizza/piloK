using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "GroundRuleTile", menuName = "Tiles/GroundRuleTile")]
public class GroundRuleTile : RuleTile<GroundRuleTile.Neighbor>
{
    // ��������� "ID" �����
    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Ground = 3;  // �������� ��� ����� ����
        public const int NotGround = 4;
    }

    // ���������� �� ���� ����� � �����
    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        if (neighbor == Neighbor.Ground)
        {
            return tile is GroundRuleTile; // ��� �������� �� ���������� ���/��������
        }
        else if (neighbor == Neighbor.NotGround)
        {
            return !(tile is GroundRuleTile); // ���������� �����
        }

        return base.RuleMatch(neighbor, tile);
    }
}
