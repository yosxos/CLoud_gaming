using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public string leaderboardName = "";
    public GameObject leaderboardEntryPrefab = null;
    //
    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    //
    public bool isFloatExpected = false;
    public bool isReversed = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        // Deactivate prefab in case
        if (this.leaderboardEntryPrefab != null)
            this.leaderboardEntryPrefab.gameObject.SetActive(false);

        // Refresh prefab
        this.RefreshLeaderboard();
    }

    // Refresh
    public void RefreshLeaderboard()
    {
        // Check prefab
        if (this.leaderboardEntryPrefab == null)
            return;

        // Trigger leaderboard data retrieval
        int startPosition = 0;
        int maxEntries = 10;
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StartPosition = startPosition,
            MaxResultsCount = maxEntries,
            StatisticName = leaderboardName
        }, OnGetLeaderboardSuccess, OnGetLeaderboardError);
    }
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        // Clear existing entries
        this.ClearExistingEntries();

        // Go through scores
        foreach (var dataEntry in result.Leaderboard)
        {
            // Get data
            string username = dataEntry.DisplayName;
            int statValue = dataEntry.StatValue;

            // Instantiate object copy
            GameObject leaderboardEntryGameobjectCopy = GameObject.Instantiate(this.leaderboardEntryPrefab, this.leaderboardEntryPrefab.transform.parent);
            if (leaderboardEntryGameobjectCopy != null)
            {
                // Activate at our prefab is deactivated
                leaderboardEntryGameobjectCopy.gameObject.SetActive(true);

                // Get leaderboard entry
                LeaderboardEntry leaderboardEntry = leaderboardEntryGameobjectCopy.GetComponent<LeaderboardEntry>();
                if (leaderboardEntry != null)
                {
                    // Fix value
                    if (isReversed == true)
                        statValue *= -1;

                    // Set value
                    leaderboardEntry.SetValue(username, (isFloatExpected == true ? ((float)statValue / 100.0f).ToString("0.00") : statValue.ToString()));

                    // Add to list
                    if (this.leaderboardEntries == null)
                        this.leaderboardEntries = new List<LeaderboardEntry>();
                    this.leaderboardEntries.Add(leaderboardEntry);
                }
                // Else, destroy object we just spawned
                else
                {
                    GameObject.Destroy(leaderboardEntryGameobjectCopy);
                }
            }
        }
    }

    private void OnGetLeaderboardError(PlayFabError error)
    {
        // Log error
        Debug.LogError($"Leaderboard.OnGetLeaderboardError() - Error: {error.ErrorMessage}");
    }

    // Clear existing entries
    public void ClearExistingEntries()
    {
        if (this.leaderboardEntries != null)
        {
            while (this.leaderboardEntries.Count > 0)
            {
                if (this.leaderboardEntries[0] != null)
                {
                    GameObject.Destroy(this.leaderboardEntries[0].gameObject);
                }

                // Remove first entry
                this.leaderboardEntries.RemoveAt(0);
            }
        }
    }
}