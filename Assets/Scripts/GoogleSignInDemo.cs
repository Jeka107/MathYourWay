using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Google;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GoogleSignInDemo : MonoBehaviour
{
    public Text infoText;
    public string webClientId = "542556702867-4fos9a8s2snpf1a1mihuv3ej9dt9stod.apps.googleusercontent.com";

    private FirebaseAuth auth;
    private FirebaseDatabase database;
    private DatabaseReference databaseReference;

    private FirebaseUser logonUser;
    private GoogleSignInConfiguration configuration;

    List<LevelSimple> levels = new List<LevelSimple>();

    private void Awake()
    {
        Debug.Log($"user id is: {AnalyticsSessionInfo.userId}");
    }

    public void Start()
    {
        configuration = new GoogleSignInConfiguration { 
            WebClientId = "542556702867-4fos9a8s2snpf1a1mihuv3ej9dt9stod.apps.googleusercontent.com", 
            RequestEmail = true, 
            RequestIdToken = true 
        };
        CheckFirebaseDependencies();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Debug.Log("[MORIS] AuthStateChanged call");
        if (auth.CurrentUser != logonUser)
        {
            Debug.Log($"auth.CurrentUser is null? -> {auth.CurrentUser == null}");
            Debug.Log($"user is null? -> {logonUser == null}");

            bool signedIn = logonUser != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && logonUser != null)
            {
                Debug.Log($"Signed out {logonUser.UserId}");
            }

            logonUser = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log($"Signed in {logonUser.UserId}");
            }
        }
    }

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                    auth.StateChanged += AuthStateChanged;
                    AuthStateChanged(this, null);

                    database = FirebaseDatabase.DefaultInstance;

                    if (Application.isEditor)
                    {
                        database.SetPersistenceEnabled(false);
                    }

                    databaseReference = database.RootReference;

                    AddToInformation("Available");
                }
                else
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    public void OnClickWriteDB()
    {
        AddToInformation("OnClickWriteDB call");

        WriteNewLevel("120", "120");

        AddToInformation("OnClickWriteDB call finished");
    }

    public void WriteNewLevel(string level, string points)
    {
        AddToInformation("WriteNewLevel call");

        string userId = Guid.NewGuid().ToString();

        LevelSimple levelSimple = new LevelSimple(level, points);

        /*if(logonUser != null && logonUser.UserId != null)
        {
            userId = logonUser.UserId;
        }*/

        var list = new LevelSimpleList();

        levels.Add(levelSimple);
        levels.Add(levelSimple);
        levels.Add(levelSimple);
        levels.Add(levelSimple);

        list.levels = levels;

        string json = JsonUtility.ToJson(list);

        Debug.Log($"the json generate: {json}");

        string key = databaseReference.Child("levels").Child(userId).Child("test").Push().Key;

        Debug.Log($"the key generate: {key}");

        List<string> test = levels.Select(x => x.level).ToList();

        try
        {
            databaseReference.Child("levels").Child(userId).SetRawJsonValueAsync(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"exception while saving {ex.Message}");
        }


        Debug.Log("WriteNewLevel finished");
        AddToInformation("WriteNewLevel finished");
    }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        try
        {
            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
        }
        catch (Exception ex)
        {
            AddToInformation($"Exception -> {ex.Message}");
        }
    }

    private void OnSignOut()
    {
        try
        {
            Debug.Log("*************OnSignOut starting***************");

            Debug.Log("OnSignOut call.");

            AddToInformation("Calling SignOut from google");

            Debug.Log("GoogleSignIn.DefaultInstance.SignOut call.");
            GoogleSignIn.DefaultInstance.SignOut();
            Debug.Log("GoogleSignIn.DefaultInstance.SignOut finished.");

            AddToInformation("GoogleSignIn.DefaultInstance.SignOut finished.");

            Debug.Log("auth.SignOut call.");
            auth.SignOut();
            Debug.Log("auth.SignOut finished.");

            Debug.Log("*************OnSignOut finished***************");
        }
        catch (Exception ex)
        {
            Debug.LogError($"EXCEPTION WHILE OnSignOut {ex}");
        }
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            //test
            AddToInformation("Welcome: " + task.Result.DisplayName + "!");
            AddToInformation("Email = " + task.Result.Email);
            //AddToInformation("Google ID Token = " + task.Result.IdToken);
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        AddToInformation("trying log in to firebase");

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                Debug.Log("Exception while SignInWithGoogleOnFirebase");
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                Debug.Log("Sign In Successful.");
                AddToInformation("Sign In Successful.");
            }
        });
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str) { infoText.text += "\n" + str; }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }
}