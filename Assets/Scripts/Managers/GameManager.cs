using UnityEngine;

public class GameManager : MonoBehaviour
{
    private ItemManager itemManager;
    private void Start()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        itemManager = new ItemManager();
    }
}
