using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Customer")]
public class Customer : ScriptableObject, IRarity
{

    [PreviewField]
    [SerializeField] private Sprite sprite;
    [SerializeField] private float rarity;

    public Sprite Sprite => sprite;

    public float Rarity => rarity;
    
    public Order Order { get; set; }
    public Dialogue Dialogue { get; set; }
    //image
    //name
    //other info
    //preferences
    
}
