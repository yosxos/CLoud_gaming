using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// TODO: Inherit or fill data in this class
[System.Serializable]
public class PlayfabInventoryItem
{

}

public class PlayfabInventory : MonoBehaviour
{
    private static PlayfabInventory instance = null;
    public static PlayfabInventory Instance
    {
        get
        {
            //if (instance == null)
            //    instance = FindObjectOfType<PlayfabInventory>();
            return instance;
        }
    }

    [Header("Inventory")]
    /// <summary>
    /// Array of inventory items belonging to the user.
    /// </summary>
    public List<PlayfabInventoryItem> Inventory;
    /// <summary>
    /// Array of virtual currency balance(s) belonging to the user.
    /// </summary>
    public Dictionary<string, int> VirtualCurrency;

    [Header("Events")]
    public UnityEvent OnInventoryUpdateSuccess = new UnityEvent();
    public UnityEvent OnInventoryUpdateError = new UnityEvent();

    // Start is called before the first frame update
    void OnEnable()
    {
        // Only keep one singleton if another is already set
        if (PlayfabInventory.Instance != null && PlayfabInventory.Instance != this)
        {
            GameObject.Destroy(this.gameObject);
        }
        // No singleton set or its us
        else
        {
            // Set ourselves as the singleton
            PlayfabInventory.instance = this;

            // Keep cross scene ?
            DontDestroyOnLoad(this.gameObject);

            // Update Inventory
            this.UpdateInventory();
        }
    }

    private float nextUpdateInventory = 0.0f;
    private const float UpdateInventoryEvery = 15.0f;
    void Update()
    {
        if (this.nextUpdateInventory > 0.0f)
            this.nextUpdateInventory -= Time.deltaTime;
        
        if (this.nextUpdateInventory <= 0.0f)
        {
            this.UpdateInventory();
            this.nextUpdateInventory = PlayfabInventory.UpdateInventoryEvery;
        }
    }

    public void UpdateInventory()
    {
        // Trigger news show if logged in
        if (PlayfabAuth.IsLoggedIn == true)
        {
            // TODO: Retrieve inventory from the PlayfabAPI
        }

        // Refresh in X seconds
        this.nextUpdateInventory = PlayfabInventory.UpdateInventoryEvery;
    }

    private void OnGetUserInventorySuccess()
    {
        //// TODO: Update inventory here
        // > Update this.Inventory
        // > Update this.VirtualCurrency

        // Callback
        if (this.OnInventoryUpdateSuccess != null)
            this.OnInventoryUpdateSuccess.Invoke();
    }

    private void OnGetUserInventoryError()
    {
        // Log
        Debug.LogError("PlayfabInventory.OnGetUserInventoryError() - Error: TODO");

        // Callback
        if (this.OnInventoryUpdateError != null)
            this.OnInventoryUpdateError.Invoke();
    }

    // Accessor
    public bool Possess(string catalogItemID)
    {
        if (string.IsNullOrWhiteSpace(catalogItemID) == false && this.Inventory != null)
        {
            for (int i = 0; i < this.Inventory.Count; i++)
            {
                //// TODO: Find item by... ID ? Name?
                //bool itemFound = false;
                //if (itemFound == true)
                //    return true;
            }
        }
        return false;
    }
}