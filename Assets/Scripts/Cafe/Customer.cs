using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Customer")]
public class Customer : ScriptableObject, IRarity
{

    [PreviewField]
    [SerializeField] private Sprite sprite;

    [SerializeField] private List<Sprite> spriteSheet;
    [SerializeField] private float rarity;

    public Sprite Sprite => sprite;

    public List<Sprite> SpriteSheet => spriteSheet;

    public float Rarity => rarity;
    
    public Order Order { get; set; }
    public Dialogue Dialogue { get; set; }
    //image
    //name
    //other info
    //preferences
    
}
