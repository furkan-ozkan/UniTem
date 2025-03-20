using UnityEngine;
using UnityEngine.InputSystem;
namespace RedAxeGames
{
    public static class InputHelper
    {
        public static string GetBindingDisplayString(InputActionReference actionReference, int bindingIndex = 0)
        {
            if (actionReference == null || actionReference.action == null)
                return "Unknown";

            var action = actionReference.action;
            var binding = action.bindings[bindingIndex];

            return InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
}