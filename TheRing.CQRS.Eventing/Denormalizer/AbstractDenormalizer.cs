namespace TheRing.CQRS.Eventing
{
    public abstract class AbstractDenormalizer
    {
        #region Fields

        protected readonly IDenormalizerRepository Repository;

        #endregion

        #region Constructors and Destructors

        protected AbstractDenormalizer(IDenormalizerRepository repository)
        {
            this.Repository = repository;
        }

        #endregion
    }

}