using UnityEngine;
using UnityEngine.InputSystem;

namespace SuperGame
{
    [CreateAssetMenu(fileName = "GameInputSettings", menuName = "SuperGame/Game Input Settings")]
    public class GameInputSettings : ScriptableObject
    {
        public const string ResourcePath = "GameInputSettings";

        public InputActionAsset InputActions;
    }
}
