using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class FoxCharacterWinScreenAddCrystals : FoxCharacterWinScreen
{
    protected override void OnWin()
    {
        // Data from the level we just finished
        int crystalsCount = this.FoxCharacterInventory.jewelsCount;

        // Increase virtual currency
        var request = new AddUserVirtualCurrencyRequest { Amount = crystalsCount, VirtualCurrency = "CR" };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnAddUserVirtualCurrencySuccess, OnAddUserVirtualCurrencyError);

        // Call base function from the class "FoxCharacterWinScreen" to display our score on the end screen & show buttons to go back to the Menu
        base.OnWin();
    }

    private void OnAddUserVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Virtual currency increased successfully.");
    }

    private void OnAddUserVirtualCurrencyError(PlayFabError error)
    {
        Debug.LogError("Failed to increase virtual currency: " + error.ErrorMessage);
    }
}
