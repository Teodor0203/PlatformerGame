using Unity.Collections;
using UnityEngine;

[CreateAssetMenu]

public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public ItemType itemType;

    public Loot(string lootName, int dropChance, ItemType itemType)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
        this.itemType = itemType;
    }
}
