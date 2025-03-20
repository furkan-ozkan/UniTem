using UnityEngine;

namespace RedAxeGames
{
    public interface IInputAction
    {
        void OnMove(Vector2 direction);
        void OnJump();
        // void OnInteract();
        // void OnInteract();
    }
}