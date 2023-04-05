using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsView : MonoBehaviour
{
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text usernameText = null;
    //
    public Text crystalsCountText = null;
    public Image crystalsIcon = null;

    void OnEnable()
    {
        // Hide
        this.Hide();

        // Trigger news show if logged in
        if (PlayfabAuth.IsLoggedIn == true)
        {
            // Trigger immediate update
            this.UpdateView();
        }
    }

    // Update View
    public void UpdateView()
    {
        this.UpdateUserAccountInfos();
        this.UpdateUserStats();
    }

    // Show / Hide
    void Show()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    void Hide()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }

    // Update Account Infos
private async void UpdateUserAccountInfos()
{
    try
    {
        var result = await PlayfabAPI.GetAccountInfo();
        OnUpdateUserAccountInfosSuccess(result);
    }
    catch
    {
        OnUpdateUserAccountInfosError();
    }
}

private void OnUpdateUserAccountInfosSuccess(PlayfabAPI.AccountInfoResult result)
{
    if (usernameText != null)
        usernameText.text = result.Username;

    this.Show();
}

    private void OnUpdateUserAccountInfosError()
    {
        // Log
        Debug.LogError("PlayerStatsView.OnUpdateUserAccountInfosError() - Error: TODO");
    }

private async void UpdateUserStats()
{
    try
    {
        var result = await PlayfabAPI.GetInventory();
        OnGetUserInventorySuccess(result);
    }
    catch
    {
        OnGetUserInventoryError();
    }
}

private void OnGetUserInventorySuccess(PlayfabAPI.InventoryResult result)
{
    int crystalsCount = result.VirtualCurrency["CR"];

    if (this.crystalsCountText != null)
    {
        this.crystalsCountText.gameObject.SetActive(true);
        this.crystalsCountText.text = crystalsCount.ToString();
    }

    if (this.crystalsIcon != null)
        this.crystalsIcon.gameObject.SetActive(true);

    this.Show();
}


    private void OnGetUserInventoryError()
    {
        // Update crystals count
        {
            // Update crystal text
            if (this.crystalsCountText != null)
            {
                this.crystalsCountText.gameObject.SetActive(true);
                this.crystalsCountText.text = "???";
            }

            // Show crystal icon
            if (this.crystalsIcon != null)
                this.crystalsIcon.gameObject.SetActive(true);
        }
    }
}