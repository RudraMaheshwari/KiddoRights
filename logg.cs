using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase;
using System;
using System.Threading.Tasks;
using Firebase.Extensions;

public class SUController : MonoBehaviour
{
    TMP_Text TEST;
   
    public GameObject loginpanel, profilepanel;
    public TMP_InputField loginEmail, loginPassword, loginUsername, signupEmail, signupPassword, signpCpass, signupUsername, signupName;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    bool isSignIn =false;


void Start() // Ensure 'Start' is capitalized
{
    Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
    {
        var dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            InitializeFirebase();
        }
        else
        {
            UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        }
    });

    }
    public void OpenLoginPanel1()
    {
        loginpanel.SetActive(true);
        profilepanel.SetActive(false);
    }

    public void OpenSignupPanel1()
    {
        loginpanel.SetActive(false);
        profilepanel.SetActive(true);
    }

public void LoginUser()
{

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    TEST = GetComponent<TMP_Text>();
    if (string.IsNullOrEmpty(loginEmail.text) || string.IsNullOrEmpty(loginPassword.text))
    {
        Debug.Log("Please fill in both email and password for login.");
        return;
    }

    SignInUser(loginEmail.text, loginPassword.text);
}

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(signupEmail.text) || string.IsNullOrEmpty(signupPassword.text) || string.IsNullOrEmpty(signpCpass.text) || string.IsNullOrEmpty(signupUsername.text) || string.IsNullOrEmpty(signupName.text))
        {
            Debug.Log("Please fill in all the required fields for signup.");
            return;
        }

        if (signupPassword.text != signpCpass.text)
        {
            Debug.Log("Passwords do not match.");
            return;
        }

        // Additional validation logic (e.g., email format, password strength) can be added here
        
        // If all validations pass, proceed with sign up logic
        Debug.Log("Signup successful!"); // Replace this with actual signup logic

        CreateUser(signupEmail.text,signupPassword.text,signupUsername.text);
    }
    

       void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        if (auth != null)
        {
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
        }
        else
        {
            Debug.LogError("Firebase Auth is not properly initialized!");
        }
    }

    void CreateUser(string email,string password,string Username)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
        if (task.IsCanceled) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            return;
        }

  // Firebase user has been created.
        Firebase.Auth.AuthResult result = task.Result;
        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
             result.User.DisplayName, result.User.UserId);
             UpdateUserProfile(Username);
        });

        
    }


public void SignInUser(string email, string password)
{
    if (auth != null) // Check if 'auth' is not null before using it
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);
        });
    }
    else
    {
        Debug.LogError("Firebase Auth is not properly initialized!");
    }
}


    

void AuthStateChanged(object sender, System.EventArgs eventArgs) {
  if (auth.CurrentUser != user) {
    bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
        && auth.CurrentUser.IsValid();
    if (!signedIn && user != null) {
      Debug.Log("Signed out " + user.UserId);
    }
    user = auth.CurrentUser;
    if (signedIn) {
      Debug.Log("Signed in " + user.UserId);
      isSignIn=true;
    }
  }
}

  void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
        else
        {
            Debug.LogWarning("Firebase Auth is already null in OnDestroy().");
        }
    }

void UpdateUserProfile(string Username)
{
    Firebase.Auth.FirebaseUser user = auth.CurrentUser;
if (user != null) {
  Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
    DisplayName = Username,
    PhotoUrl = new System.Uri("https://fastly.picsum.photos/id/546/200/300.jpg?hmac=WRVm_tMObPuM2HqJCr5D6N59Mboh73aqEno4MCuu5AE"),
  };
  user.UpdateUserProfileAsync(profile).ContinueWith(task => {
    if (task.IsCanceled) {
      Debug.LogError("UpdateUserProfileAsync was canceled.");
      return;
    }
    if (task.IsFaulted) {
      Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
      return;
    }

    Debug.Log("User profile updated successfully.");
    
  });
}
}
bool isSigned=false;

    void Update()
    {
        if(isSignIn)
        {
            if(isSigned)
            {
                isSigned=true;
            }
        }
    }
}