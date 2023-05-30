using System;
using UnityEngine;
using Firebase.Auth;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth _auth;

    public Action<FirebaseUser> OnUserAuthenticated;
    

    private void Start()
    {
        // Initialize Firebase Auth
        _auth = FirebaseAuth.DefaultInstance;

#if UNITY_EDITOR
        // Sign in with test account in the Unity Editor
        _auth.SignInWithEmailAndPasswordAsync("urakhblog3@gmail.com", "misomagic8283").ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Email/Password sign-in was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Email/Password sign-in encountered an error: " + task.Exception);
                return;
            }

            // Sign-in success
            var result = task.Result;
            FirebaseUser user = result.User;
            OnUserAuthenticated?.Invoke(user);
            
            Debug.Log("User signed in with ID: " + user.UserId);
        });
#else
        // Attempt anonymous sign-in on device
        _auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Anonymous sign-in was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Anonymous sign-in encountered an error: " + task.Exception);
                return;
            }

            // Sign-in success
            var result = task.Result;
            FirebaseUser user = result.User;
            OnUserAuthenticated?.Invoke(user);
            
            Debug.Log("Anonymous user signed in with ID: " + user.UserId);
        });
#endif
    }
}