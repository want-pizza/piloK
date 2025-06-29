using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class SpikeColliderBySprite : MonoBehaviour
{
    private BoxCollider2D[] colliders;

    void Awake()
    {
        colliders = GetComponents<BoxCollider2D>();
        if (colliders.Length == 0)
        {
            Debug.LogWarning("BoxCollider2D �� ��������.");
        }
    }

    void Start()
    {
        Tilemap tilemap = GetComponentInParent<Tilemap>();
        if (tilemap == null)
        {
            return;
        }

        Vector3Int cellPos = tilemap.WorldToCell(transform.position);
        Sprite sprite = tilemap.GetSprite(cellPos);
        if (sprite == null)
        {
            return;
        }

        string spriteName = sprite.name;

        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        switch (spriteName)
        {
            case "SpikeTiles_0": // ����
                SetCollider(0, new Vector2(0f, 0.4f), new Vector2(0.7f, 0.2f));
                break;

            case "SpikeTiles_3": // ˳��
                SetCollider(0, new Vector2(-0.4f, 0f), new Vector2(0.2f, 0.7f));
                break;

            case "SpikeTiles_4": // �����
                SetCollider(0, new Vector2(0.4f, 0f), new Vector2(0.2f, 0.7f));
                break;

            case "SpikeTiles_7": // ���
                SetCollider(0, new Vector2(0f, -0.4f), new Vector2(0.7f, 0.2f));
                break;

            case "SpikeTiles_5": // ��� + ˳��
                SetCollider(0, new Vector2(0f, -0.4f), new Vector2(0.7f, 0.2f));
                SetCollider(1, new Vector2(-0.4f, 0f), new Vector2(0.2f, 0.7f));
                break;

            case "SpikeTiles_2": // ���� + �����
                SetCollider(0, new Vector2(0f, 0.4f), new Vector2(0.7f, 0.2f));
                SetCollider(1, new Vector2(0.4f, 0f), new Vector2(0.2f, 0.7f));
                break;

            case "SpikeTiles_6": // ��� + �����
                SetCollider(0, new Vector2(0f, -0.4f), new Vector2(0.7f, 0.2f));
                SetCollider(1, new Vector2(0.4f, 0f), new Vector2(0.2f, 0.7f));
                break;

            case "SpikeTiles_1": // ���� + ˳��
                SetCollider(0, new Vector2(0f, 0.4f), new Vector2(0.7f, 0.2f));
                SetCollider(1, new Vector2(-0.4f, 0f), new Vector2(0.2f, 0.7f));
                break;

            default:
                SetCollider(0, Vector2.zero, Vector2.one);
                Debug.LogWarning($"unnoun sprite \"{spriteName}\"");
                break;
        }
    }

    void SetCollider(int index, Vector2 offset, Vector2 size)
    {
        if (index >= colliders.Length)
        {
            Debug.LogWarning($"���� ��������� BoxCollider2D ��� index: {index}");
            return;
        }

        colliders[index].enabled = true;
        colliders[index].offset = offset;
        colliders[index].size = size;
    }
}
