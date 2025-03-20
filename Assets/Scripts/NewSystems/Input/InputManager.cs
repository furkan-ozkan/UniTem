using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace RedAxeGames
{
    public class InputManager : MonoBehaviour
    {
        private static InputManager instance;
        [FormerlySerializedAs("playerInputHandler")] [SerializeField] private OldPlayerInputHandler oldPlayerInputHandler;
        [SerializeField] private PlayerInputs playerInputs;
        [SerializeField] private InputActionAsset inputActionAsset;
        private List<IInputAction> inputHandlers = new();

        public static InputActionAsset InputActionAsset { get; private set; }
        public static PlayerInputs PlayerInputs { get; private set; }

        public void RegisterInputHandler(IInputAction inputHandler)
        {
            if (!inputHandlers.Contains(inputHandler))
            {
                inputHandlers.Add(inputHandler);
            }
        }

        public void UnregisterInputHandler(IInputAction inputHandler)
        {
            if (inputHandlers.Contains(inputHandler))
            {
                inputHandlers.Remove(inputHandler);
            }
        }

        public void ProcessInput()
        {
            foreach (var handler in inputHandlers)
            {
                // Call input methods on handlers
                // Example: handler.OnMove(Vector2.zero);
            }
        }

        public static void SetInputEnability(InputMapType mapType, bool enable)
        {
            switch (mapType)
            {
                case InputMapType.Gameplay:
                    if (enable)
                        PlayerInputs.Gameplay.Enable();
                    else
                        PlayerInputs.Gameplay.Disable();
                    break;

                case InputMapType.UI:
                    if (enable)
                        PlayerInputs.UI.Enable();
                    else
                        PlayerInputs.UI.Disable();
                    break;
            }
        }

        public static void SetInputMap(InputMapType mapType)
        {
            switch (mapType)
            {
                case InputMapType.UI:
                    InputActionAsset.FindActionMap("Gameplay").Disable();
                    InputActionAsset.FindActionMap("UI").Enable();
                    // PlayerInputs.Gameplay.Disable();
                    // PlayerInputs.UI.Enable();
                    break;

                case InputMapType.Gameplay:
                    // PlayerInputs.UI.Disable();
                    // PlayerInputs.Gameplay.Enable();
                    InputActionAsset.FindActionMap("UI").Disable();
                    InputActionAsset.FindActionMap("Gameplay").Enable();
                    break;
            }
        }

        //MONOBEHAVIOUR
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                PlayerInputs = new PlayerInputs();
                InputActionAsset = inputActionAsset;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public enum InputMapType
        {
            Gameplay,
            UI,
        }
    }
}