using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : PersistentSingleton<Transition>
{
    [SerializeField] private Image image;

    public void SetImageAlpha(float a)
    {
        image.color = new Color(image.color.r, image.color.b, image.color.g, a);
    }
}
