using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseFirestore _db;
    private string _userId = "mosk";
    [SerializeField] private AuthManager authManager;
    public AuthManager AuthManager => authManager;
    public Action OnUserSetup;

    public bool Authenticated { get; set; }

    private void Start()
    {
        _db = FirebaseFirestore.DefaultInstance;
        authManager.OnUserAuthenticated += SetupUser;
        GameManager.Instance.OnDayEnded += UpdateUserData;
    }

    private void SetupUser(FirebaseUser user)
    {
        Debug.Log("USER AUTHENTICATED TRUE");
        Authenticated = true;
        _userId = user.UserId;

        GetUserData();

    }
    
    private void UpdateUserData()
    {
        UserData userData = new UserData
        {
            Coins = (float)Decimal.Round((decimal)CurrencyManager.Instance.CoinBalance,2),
            Day = GameManager.Instance.DayNumber
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
                Debug.Log(" --- Update User Data");
            }
        });
    }

    public void GetUserData()
    {
        _db.Collection("users").Document(_userId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result.Exists)
            {
                UserData userData = task.Result.ConvertTo<UserData>();

                CurrencyManager.Instance.CoinBalance = userData.Coins;
                GameManager.Instance.DayNumber = (int)userData.Day;
            
                Debug.Log("--- Get User Data" + userData.Day);
            
                OnUserSetup?.Invoke();
            }
            else
            {
                UserData newUser = new UserData
                {
                    Coins = 50, 
                    Day = 0
                };

                _db.Collection("users").Document(_userId).SetAsync(newUser).ContinueWithOnMainThread(createUserTask =>
                {
                    if (createUserTask.IsCompleted)
                    {
                        Debug.Log("New user created successfully");
                        
                        CurrencyManager.Instance.CoinBalance = newUser.Coins;
                    
                        GameManager.Instance.DayNumber = (int)newUser.Day;

                        OnUserSetup?.Invoke();
                    }
                    else
                    {
                        Debug.Log("Failed to create new user");
                    }
                });
            }
            
        });
    }
}
