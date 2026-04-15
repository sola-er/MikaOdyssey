using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    public enum ItemType
    {
        Shell,
        Starfish,
        HouseItem
    }

    [SerializeField] private ItemType itemType;

    public void Interact()
    {

        // Beach
        if (BeachManager.Singleton != null && itemType == ItemType.Shell)
        {
            BeachManager.Singleton.AddShell();
        }

        // Underwater
        else if (UnderwaterManager.Singleton != null && itemType == ItemType.Starfish)
        {
            UnderwaterManager.Singleton.AddStarfish();
        }

        // House
        else if (HouseManager.Singleton != null && itemType == ItemType.HouseItem)
        {
            HouseManager.Singleton.CollectItem();
        }

        Destroy(gameObject);
    }
}