namespace TheRing.CQRS.Eventing
{
    public abstract class Denormalizer
    {
        #region Fields

        protected readonly IDenormalizerRepository Repository;

        #endregion

        #region Constructors and Destructors

        protected Denormalizer(IDenormalizerRepository repository)
        {
            this.Repository = repository;
        }

        #endregion
    }

}