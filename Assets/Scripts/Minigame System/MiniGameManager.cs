using UnityEngine;

public static class MiniGameManager
{
    public static void StartMiniGame(MiniGame miniGame, GameObject player)
    {
        miniGame.Initialize(player);
    }
    public static void EndMiniGame(MiniGame miniGame)
    {
        miniGame.EndMiniGame();
    }
    public static void CompleteMiniGame(MiniGame miniGame)
    {
        miniGame.CompleteMiniGame();
    }
    public static void ResetMiniGame(MiniGame miniGame)
    {
        miniGame.ResetMiniGame();
    }
    public static OldState GetState(MiniGame miniGame)
    {
        return miniGame.GetState();
    }
}