using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Shop/ShopItemUpgradeable")]
public class ShopItemUpgradeable : ShopItem
{
    [SerializeField] private List<ShopItemLevel> levels;

    public List<ShopItemLevel> Levels => levels;

    public int CurrentLevel { get; private set; } = 0;
    

    public void LevelUp()
    {
        int newLevel = CurrentLevel + 1;
        if (Utilities.IsIndexValid(levels, newLevel))
        {
            CurrentLevel = newLevel;
        }
    }

    public void SetLevel(int newLevel)
    {
        if (Utilities.IsIndexValid(levels, newLevel))
        {
            CurrentLevel = newLevel;
        }
    }
}
