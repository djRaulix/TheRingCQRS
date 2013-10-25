namespace WebSample.Commanding
{
    using TheRing.CQRS.Commanding;

    public class CreateUser : CreateCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}