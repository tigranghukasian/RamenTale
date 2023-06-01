using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public struct ShopItemData
{
    [FirestoreProperty]
    public string Id { get; set; }
    
    [FirestoreProperty]
    public int Level { get; set; }
    
}
