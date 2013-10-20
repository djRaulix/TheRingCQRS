namespace WebSample.App_Start
{
    #region using

    using System;
    using System.Linq.Expressions;

    using SimpleInjector;

    #endregion

    public static class ContextDependentExtensions
    {
        #region Public Methods and Operators

        public static void RegisterWithContext<TService>(
            this Container container, 
            Func<DependencyContext, TService> contextBasedFactory)
            where TService : class
        {
            if (contextBasedFactory == null)
            {
                throw new ArgumentNullException("contextBasedFactory");
            }

            Func<TService> rootFactory =
                () => contextBasedFactory(DependencyContext.Root);

            container.Register(rootFactory, Lifestyle.Transient);

            // Allow the Func<DependencyContext, TService> to be 
            // injected into parent types.
            container.ExpressionBuilding += (sender, e) =>
            {
                if (e.RegisteredServiceType != typeof(TService))
                {
                    var rewriter = new DependencyContextRewriter
                    {
                        ServiceType = e.RegisteredServiceType, 
                        ContextBasedFactory = contextBasedFactory, 
                        RootFactory = rootFactory, 
                        Expression = e.Expression
                    };

                    e.Expression = rewriter.Visit(e.Expression);
                }
            };
        }

        #endregion

        private sealed class DependencyContextRewriter : ExpressionVisitor
        {
            #region Properties

            internal object ContextBasedFactory { get; set; }

            internal Expression Expression { get; set; }

            internal Type ImplementationType
            {
                get
                {
                    var expression = this.Expression as NewExpression;

                    if (expression != null)
                    {
                        return expression.Constructor.DeclaringType;
                    }

                    return this.ServiceType;
                }
            }

            internal object RootFactory { get; set; }

            internal Type ServiceType { get; set; }

            #endregion

            #region Methods

            protected override Expression VisitInvocation(
                InvocationExpression node)
            {
                if (!this.IsRootedContextBasedFactory(node))
                {
                    return base.VisitInvocation(node);
                }

                return Expression.Invoke(
                    Expression.Constant(this.ContextBasedFactory), 
                    Expression.Constant(
                        new DependencyContext(
                            this.ServiceType, 
                            this.ImplementationType)));
            }

            private bool IsRootedContextBasedFactory(
                InvocationExpression node)
            {
                var expression =
                    node.Expression as ConstantExpression;

                if (expression == null)
                {
                    return false;
                }

                return ReferenceEquals(
                    expression.Value, 
                    this.RootFactory);
            }

            #endregion
        }
    }
}