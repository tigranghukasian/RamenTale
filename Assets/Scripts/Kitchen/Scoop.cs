using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoop : Moveable
{
    [SerializeField] private Image scoopSoup;

    public void SetColor(Color color)
    {
        scoopSoup.color = color;
    }
}
