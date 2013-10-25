namespace TheRing.CQRS.Commanding
{
    public interface IHandlesCommand<in TCommand>
        where TCommand : class, ICommand
    {
        void Handles(IHandlesContext<TCommand> handlesContext);
    }
}