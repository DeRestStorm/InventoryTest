using Definitions;
using UnityEngine;

namespace SuperGame
{
    public class EntryPoint : MonoBehaviour
    {
        private DefData _defData;

        private void Awake()
        {
            _defData = InitDefs.LoadFromJson();
        }
    }
}