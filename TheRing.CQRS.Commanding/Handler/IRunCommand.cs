namespace TheRing.CQRS.Commanding.Handler
{
    public interface IRunCommand<in TCommand> where TCommand : AbstractCommand
    {
        void Run(TCommand command);
    }
}