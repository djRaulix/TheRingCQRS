namespace WebSample.Commanding
{
    using TheRing.CQRS.Commanding;

    public class AddUserAddress : UpdateCommand
    {
        public string Address { get; set; }
    }
}