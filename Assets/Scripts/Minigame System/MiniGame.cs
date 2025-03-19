using System;
using Unity.Collections;
using UnityEngine;

public abstract class MiniGame : MonoBehaviour
{
    [ReadOnly] public GameObject player;

    public void AddInputProviders()
    {
        InputProvider.OnEscPressed += EndMiniGame;
    }

    public void RemoveInputProviders()
    {
        InputProvider.OnEscPressed -= EndMiniGame;
    }

    public abstract void Initialize(GameObject player);
    public abstract void EndMiniGame();
    public abstract void CompleteMiniGame();
    public abstract void ResetMiniGame();
    public abstract State GetState();
}