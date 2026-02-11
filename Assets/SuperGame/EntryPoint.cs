using Definitions;
using Unity.Scenes;
using UnityEngine;

namespace SuperGame
{
    public class EntryPoint : MonoBehaviour
    {
        private DefData _defData;
        [SerializeField] private SubScene _subScene;

        private void Start()
        {
            _defData = InitDefs.LoadFromJson();
            Defs.Init(_defData);
        }
    }
}