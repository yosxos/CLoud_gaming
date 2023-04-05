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
    private void UpdateUserAccountInfos()
    {
        // TODO: Call Playfab to retrieve various infos from our online account (ex. username, device or account ID, etc)
        this.OnUpdateUserAccountInfosSuccess();
    }

    private void OnUpdateUserAccountInfosSuccess()
    {
        // Log
        Debug.LogError("PlayerStatsView.OnUpdateUserAccountInfosSuccess() - Error: TODO");

        // TODO: Update username from remote service?

        // Show
        this.Show();
    }

    private void OnUpdateUserAccountInfosError()
    {
        // Log
        Debug.LogError("PlayerStatsView.OnUpdateUserAccountInfosError() - Error: TODO");
    }

    // Update User Stats
    private void UpdateUserStats()
    {
        // TODO: Call Playfab to retrieve our player's data/inventory
        //       in order to count the crystals (virtual currency? item in inventory?) we have
        this.OnGetUserInventorySuccess();   // Fake
    }

    private void OnGetUserInventorySuccess()
    {
        // Crystals we have
        int crystalsCount = 0;

        // TODO: Get crystals from Playfab feedback

        // Update crystals count
        {
            if (this.crystalsCountText != null)
            {
                this.crystalsCountText.gameObject.SetActive(true);
                this.crystalsCountText.text = crystalsCount.ToString();
            }

            if (this.crystalsIcon != null)
                this.crystalsIcon.gameObject.SetActive(true);
        }

        // Show
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