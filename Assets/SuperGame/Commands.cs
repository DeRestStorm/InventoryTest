namespace SuperGame
{
    public class Commands
    {
        private readonly MoveItemHandler _moveItemHandler;

        public Commands(MoveItemHandler moveItemHandler)
        {
            _moveItemHandler = moveItemHandler;
        }

        public void MoveItem(int itemId, short slot)
        {
            var command = new MoveItemCommand(itemId, slot);
            _moveItemHandler.Handle(command);
        }
    }
}