namespace TheRing.CQRS.Commanding
{
    public enum RequestResult
    {
        Ok, 

        ConcurrencyException, 

        Failed
    }
}