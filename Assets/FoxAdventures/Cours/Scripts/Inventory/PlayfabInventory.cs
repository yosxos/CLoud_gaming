using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;

[System.Serializable]
public class PlayfabInventoryItem
{
public string ItemId;
public string DisplayName;
public int Count;
}

public class PlayfabInventory : MonoBehaviour
{
private static PlayfabInventory instance = null;
public static PlayfabInventory Instance
{
get
{
if (instance == null)
instance = FindObjectOfType<PlayfabInventory>();
return instance;
}
}
[Header("Inventory")]
public List<PlayfabInventoryItem> Inventory;
public Dictionary<string, int> VirtualCurrency;

[Header("Events")]
public UnityEvent OnInventoryUpdateSuccess = new UnityEvent();
public UnityEvent OnInventoryUpdateError = new UnityEvent();

void OnEnable()
{
    if (PlayfabInventory.Instance != null && PlayfabInventory.Instance != this)
    {
        GameObject.Destroy(this.gameObject);
    }
    else
    {
        PlayfabInventory.instance = this;

        DontDestroyOnLoad(this.gameObject);

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
    if (PlayfabAuth.IsLoggedIn == true)
    {
        var request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, OnGetUserInventorySuccess, OnGetUserInventoryError);
    }

    this.nextUpdateInventory = PlayfabInventory.UpdateInventoryEvery;
}

private void OnGetUserInventorySuccess(GetUserInventoryResult result)
{
    this.Inventory = new List<PlayfabInventoryItem>();
    foreach (var item in result.Inventory)
    {
        PlayfabInventoryItem newItem = new PlayfabInventoryItem();
        newItem.ItemId = item.ItemId;
        newItem.DisplayName = item.DisplayName;
        newItem.Count = item.RemainingUses.Value;
        this.Inventory.Add(newItem);
    }

    this.VirtualCurrency = result.VirtualCurrency;

    if (this.OnInventoryUpdateSuccess != null)
        this.OnInventoryUpdateSuccess.Invoke();
}

private void OnGetUserInventoryError(PlayFabError error)
{
    Debug.LogError("PlayfabInventory.OnGetUserInventoryError() - Error: " + error.GenerateErrorReport());

    if (this.OnInventoryUpdateError != null)
        this.OnInventoryUpdateError.Invoke();
}

public bool Possess(string catalogItemID)
{
    if (!string.IsNullOrWhiteSpace(catalogItemID) && this.Inventory != null)
    {
        foreach (var item in this.Inventory)
        {
            if (item.ItemId == catalogItemID)
                return true;
        }
    }
    return false;
}
}