using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        InputProvider.Initialize();
    }
}
