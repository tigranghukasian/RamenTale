using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenManager : Singleton<KitchenManager>
{
    [SerializeField] private Canvas canvas;

    public Canvas Canvas => canvas;
}
