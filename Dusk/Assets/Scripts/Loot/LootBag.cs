using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    Loot GetDroppedItem()
    {
        int randomChance = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();

        foreach (Loot item in lootList)
        {
            if (randomChance <= item.dropChance)
                possibleItems.Add(item);
        }

        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }

        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 lootSpawnPosition)
    {
        Loot droppedItem = GetDroppedItem();

        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, lootSpawnPosition, Quaternion.identity);
            Loot_Dropped lootDropped = lootGameObject.GetComponent<Loot_Dropped>();

            if (lootDropped != null)
            {
                lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

                lootDropped.SetLootType(droppedItem.itemType);
            }
        }

    }

}
