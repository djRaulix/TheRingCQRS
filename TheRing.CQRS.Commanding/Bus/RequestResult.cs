namespace TheRing.CQRS.Commanding.Bus
{
    public enum RequestResult
    {
        Ok, 

        ConcurrencyException, 

        Failed
    }
}