namespace WebSample.Commanding
{
    using TheRing.CQRS.Commanding;

    public class AddUserAddressCommand : UpdateCommand
    {
        public string Address { get; set; }
    }
}