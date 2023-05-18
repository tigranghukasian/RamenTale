using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RarityFunctions {
    public static T GenerateItem<T>(List<T> itemList) where T : IRarity {
        float totalRarity = 0f;
        for (int i = 0; i < itemList.Count; i++) {
            totalRarity += itemList[i].Rarity;
        }

        float randomValue = Random.Range(0f, totalRarity);

        for (int i = 0; i < itemList.Count; i++) {
            if (randomValue < itemList[i].Rarity) {
                return itemList[i];
            }
            randomValue -= itemList[i].Rarity;
        }

        // This should never happen, but just in case
        return default(T);
    }
}
