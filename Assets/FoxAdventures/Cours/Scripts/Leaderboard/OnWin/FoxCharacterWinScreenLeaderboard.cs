using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class FoxCharacterWinScreenLeaderboard : FoxCharacterWinScreenAddCrystals
{
    protected override void OnWin()
    {
        // Use player stats to register virtual currency increase
        FoxCharacterInventory foxCharacterInventory = this.FoxPlayer.GetComponentInChildren<FoxCharacterInventory>();
        if (foxCharacterInventory != null)
        {
            // Data that we want to keep for leaderboards
            int crystalsCount = foxCharacterInventory.jewelsCount;
            float levelDuration = Time.timeSinceLevelLoad;

            // Update our best score on level 1 for crystals count
            UpdateLeaderboard("level1_crystals", crystalsCount);

            // Update our best score on level 1 for speedrun
            UpdateLeaderboard("level1_speedrun", (int)Mathf.Floor(levelDuration * 100.0f * -1.0f));
            //// Note: We multiplied the time by -1 because leaderboards in Playfab are always ranked as "more is best"
        }

        // Call base function from the class "FoxCharacterWinScreenLeaderboard" that adds crystals we collected to our inventory
        base.OnWin();
    }

    private void OnUpdatePlayerStatisticsRequestSuccess(UpdatePlayerStatisticsResult result)
    {
        // Log
        Debug.Log("Leaderboard updated successfully");
    }

    private void OnUpdatePlayerStatisticsRequestError(PlayFabError error)
    {
        // Log error
        Debug.LogError("Leaderboard update failed: " + error.ErrorMessage);
    }

    private void UpdateLeaderboard(string leaderboardName, int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdatePlayerStatisticsRequestSuccess, OnUpdatePlayerStatisticsRequestError);
    }
}
