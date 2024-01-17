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
        Authenticated = true;
        _userId = user.UserId;

        GetUserData();

    }
    public void UnlockItem(ShopItem shopItem)
    {
        ShopItemData itemData = new ShopItemData
        {
            Id = shopItem.Id,
            //Type = shopItem.GetType();
        };
        Debug.Log(" SHOP ITEM TYPE " + shopItem.GetType());
        _db.Collection("users").Document(_userId).Collection("unlockedItems").Document(shopItem.Id).SetAsync(itemData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update unlock item due to error: " + task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Failed to update unlock item because the task was canceled.");
            }
            else
            {
                Debug.Log("Item unlock updated successfuly");
            }
        });
    }
    
    public void SelectItem(ShopItem shopItem)
    {
        string subCategory = shopItem.Subcategory;
        ShopItemSubcategorySelectedData itemSelectedData = new ShopItemSubcategorySelectedData
        {
            Id = shopItem.Id,
        };
        _db.Collection("users").Document(_userId).Collection("selectedItems").Document(subCategory).SetAsync(itemSelectedData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to select item due to error: " + task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Failed to select item because the task was canceled.");
            }
            else
            {
                Debug.Log("Item selected successfuly");
            }
        });
    }

    public void GetShopItemSubcategorySelected(Action<List<ShopItemSubcategorySelectedData>> callback = null)
    {
        _db.Collection("users").Document(_userId).Collection("selectedItems").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Task is faulted" + task.Exception);
            }
            if (task.IsCanceled)
            {
                Debug.Log("task is cancelled");
            }
            if (task.IsCompleted)
            {
                List<ShopItemSubcategorySelectedData> shopItemSubCategorySelectedDatas = new List<ShopItemSubcategorySelectedData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    ShopItemSubcategorySelectedData shopItemSubCategorySelectedData = document.ConvertTo<ShopItemSubcategorySelectedData>();
                    shopItemSubCategorySelectedDatas.Add(shopItemSubCategorySelectedData);
                }
                callback?.Invoke(shopItemSubCategorySelectedDatas);
        
                // Update your game state with the retrieved items here.
                // This could involve displaying the items in the user's inventory, enabling functionality related to the items, etc.
        
                Debug.Log("Retrieved user items subcategory selected successfully");
            }
            else
            {
                Debug.Log("Failed to retrieve user items subcategory selected ");
            }

            
        });
    }
    
    public void GetUnlockedItems(Action<List<ShopItemData>> callback = null)
    {
        //Debug.Log("get unlocked items");
        _db.Collection("users").Document(_userId).Collection("unlockedItems").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Task is faulted" + task.Exception);
            }
            if (task.IsCanceled)
            {
                Debug.Log("task is cancelled");
            }
            if (task.IsCompleted)
            {
                List<ShopItemData> shopItemDatas = new List<ShopItemData>();
                foreach (DocumentSnapshot document in task.Result.Documents)
                {
                    ShopItemData shopItemData = document.ConvertTo<ShopItemData>();
                    shopItemDatas.Add(shopItemData);
                }
                //OnUnlockedShopItemDatasReceived?.Invoke();
                callback?.Invoke(shopItemDatas);
        
                // Update your game state with the retrieved items here.
                // This could involve displaying the items in the user's inventory, enabling functionality related to the items, etc.
        
                Debug.Log("Retrieved user items successfully");
            }
            else
            {
                Debug.Log("Failed to retrieve user items");
            }

            
        });
    }
    
    public void UpdateUserData()
    {
        UserData userData = new UserData
        {
            Coins = (float)Decimal.Round((decimal)CurrencyManager.Instance.CoinBalance,2),
            Diamonds = (float)Decimal.Round((decimal)CurrencyManager.Instance.DiamondsBalance,2),
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
            if (task.IsFaulted)
            {
                Debug.LogError("Task is faulted " + task.Exception?.Message);
            }

            if (task.Result.Exists)
            {
                UserData userData = task.Result.ConvertTo<UserData>();

                CurrencyManager.Instance.CoinBalance = userData.Coins;
                CurrencyManager.Instance.DiamondsBalance = userData.Diamonds;
                GameManager.Instance.DayNumber = (int)userData.Day;
                //KitchenManager.Instance.SetupUnlockedItems();
            
                Debug.Log("--- Get User Data ---");
            
                OnUserSetup?.Invoke();
            }
            else
            {
                UserData newUser = new UserData
                {
                    Coins = 150, 
                    Diamonds = 10,
                    Day = 1
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

    public void DeleteUserData(Action callback = null)
    {
        Debug.Log("DELETE DOCUMENT " + _userId);
        _db.Collection("users").Document(_userId).DeleteAsync().ContinueWithOnMainThread(deleteTask =>
        {
            if (deleteTask.IsCompleted)
            {
                callback?.Invoke();
                Debug.Log("User data deleted");
            }
            else
            {
                Debug.Log("Failed to delete user data");
            }
        });
    }
}
