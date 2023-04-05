using System;
using UnityEngine;

public static class PlayfabAuth
{
    // Const - Save email/password
    public const string PlayfabAuthPlayerPrefsKeyUsername = "playfab_auth_username";
    public const string PlayfabAuthPlayerPrefsKeyEmail = "playfab_auth_email";
    public const string PlayfabAuthPlayerPrefsKeyPassword = "playfab_auth_password";

    // Getter
    public static bool IsLoggedIn
    {
        get
        {

            return PlayFab.PlayFabClientAPI.IsClientLoggedIn();
        }
    }

    // Functions

    public static void TryRegisterWithEmail(string email, string password, string username, Action registerResultCallback, Action errorCallback)
    {
        var request = new PlayFab.ClientModels.RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            Username = username,
            RequireBothUsernameAndEmail = true
        };
        PlayFab.PlayFabClientAPI.RegisterPlayFabUser(request, (result) =>
        {
            // Save email/password
            PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail, email);
            PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword, password);

            // Callback
            registerResultCallback.Invoke();
        }, (error) =>
        {
            // Callback
            errorCallback.Invoke();
        });

    }

    public static void TryLoginWithEmail(string email, string password, Action loginResultCallback, Action errorCallback)
    {
        var request = new PlayFab.ClientModels.LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };
        PlayFab.PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            // Save email/password
            PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail, email);
            PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword, password);

            // Callback
            loginResultCallback.Invoke();
        }, (error) =>
        {
            // Callback
            errorCallback.Invoke();
        });
    }

    // Logout
    public static void Logout(Action logoutResultCallback, Action errorCallback)
    {
        // Clear all keys from PlayerPrefs
        PlayerPrefs.DeleteKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyUsername);
        PlayerPrefs.DeleteKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail);
        PlayerPrefs.DeleteKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword);

        // Callback
        logoutResultCallback.Invoke();
    }
}
