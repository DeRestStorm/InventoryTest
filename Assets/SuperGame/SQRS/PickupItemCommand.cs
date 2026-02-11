using Definitions;

namespace SuperGame
{
    public record PickupItemCommand(int ItemId)
    {
        public int ItemId { get; } = ItemId;
    }
}
