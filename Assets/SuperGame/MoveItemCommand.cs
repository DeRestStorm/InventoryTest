namespace SuperGame
{
    public class MoveItemCommand
    {
        public int ItemId { get; }
        public short ToPosition { get; }

        public MoveItemCommand(int itemId, short toPosition)
        {
            ItemId = itemId;
            ToPosition = toPosition;
        }
    }
}
