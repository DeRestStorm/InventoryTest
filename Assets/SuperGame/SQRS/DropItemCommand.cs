namespace SuperGame
{
    public record DropItemCommand(int ItemId)
    {
        public int ItemId { get; } = ItemId;
    }
}
