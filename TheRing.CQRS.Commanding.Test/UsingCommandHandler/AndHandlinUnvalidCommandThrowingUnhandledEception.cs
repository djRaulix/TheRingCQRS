namespace TheRing.CQRS.Commanding.Test.UsingCommandHandler
{
    #region using

    using System;

    using FakeItEasy;
    using FakeItEasy.ExtensionSyntax.Full;

    using FluentAssertions;

    using NUnit.Framework;

    #endregion

    public class AndHandlinUnvalidCommandThrowingUnhandledEceptions : UsingCommandHandler
    {
        #region Fields

        private Exception exception;

        #endregion

        #region Public Methods and Operators

        [Test]
        public void ThenExceptionShouldBeRaised()
        {
            this.exception.Should().NotBe(null);
        }

        #endregion

        #region Methods

        protected override void BecauseOf()
        {
            base.BecauseOf();
            try
            {
                this.Response = this.CommandHandler.Handle(this.Command);
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }
        }

        protected override void EstablishContext()
        {
            base.EstablishContext();
            this.CommandRunner.CallsTo(r => r.Run(this.Command)).Throws(new Exception());
        }

        #endregion
    }
}