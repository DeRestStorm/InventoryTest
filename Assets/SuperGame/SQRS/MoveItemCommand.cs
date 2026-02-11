namespace SuperGame
{
    public record MoveItemCommand(int ItemId, short ToPosition)
    {
        public int ItemId { get; } = ItemId;
        public short ToPosition { get; } = ToPosition;
    }
}
