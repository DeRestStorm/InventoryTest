using System.Collections.Generic;
using SuperGame.ECS;

namespace SuperGame
{
    public class World
    {
        private int _nextItemId = 1;
        public Dictionary<int, WorldItem> WorldItems = new ();

        public int AllocateItemId() => _nextItemId++;
    }
}