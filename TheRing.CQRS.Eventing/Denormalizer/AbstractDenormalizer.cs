namespace TheRing.CQRS.Eventing.Denormalizer
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