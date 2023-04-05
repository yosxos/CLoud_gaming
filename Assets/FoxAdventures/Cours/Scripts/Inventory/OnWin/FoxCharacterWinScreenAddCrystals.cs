using UnityEngine;

public class FoxCharacterWinScreenAddCrystals : FoxCharacterWinScreen
{
 protected override async void OnWin()
{
    int crystalsCount = this.FoxCharacterInventory.jewelsCount;

    try
    {
        await PlayFab.PlayFabClientAPI.AddUserVirtualCurrency("CR", crystalsCount);
        await PlayFabAPI.AddVirtualCurrency("CR", crystalsCount);
    }
    catch
    {
        Debug.LogError("Error adding virtual currency");
    }

    base.OnWin();
}

}