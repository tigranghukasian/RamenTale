using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public struct ShopItemData
{
    [FirestoreProperty]
    public string Id { get; set; }

}
[FirestoreData]
public struct ShopItemSubcategorySelectedData
{
    [FirestoreProperty]
    public string Id { get; set; }
}
