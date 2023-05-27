using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore _db;
    private string _userId = "mosk";

    private void Start()
    {
        _db = FirebaseFirestore.DefaultInstance;
        CurrencyManager.Instance.OnCoinsChanged += OnCoinsUpdated;
    }

    private void OnCoinsUpdated(float balance)
    {
        UserData userData = new UserData
        {
            Coins = balance
        };
        DocumentReference users = _db.Collection("users").Document(_userId);
        users.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("An error occurred: " + task.Exception.ToString());
            }
            else
            {
                Debug.Log("Updated counter");
            }
        });
    }
}
