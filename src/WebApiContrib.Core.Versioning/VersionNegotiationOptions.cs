using System;
using System.Collections.Generic;

namespace WebApiContrib.Core.Versioning
{
    /// <summary>
    /// Options for the <see cref="VersionNegotiationResultFilter"/>.
    /// </summary>
    public class VersionNegotiationOptions
    {
        /// <summary>
        /// Creates a new instance of <see cref="VersionNegotiationOptions"/>.
        /// </summary>
        public VersionNegotiationOptions()
        {
            RequireVersionedObjectResult = true;
            ThrowOnMissingMapper = true;
            EmitVaryHeader = false;
            ConfigureStrategy = new Dictionary<Type, Action<object>>();
            StrategyTypes = new List<Type>();
        }

        /// <summary>
        /// Gets or sets whether the user is required to explicitly return a
        /// <see cref="VersionedObjectResult"/> instead of a plain
        /// <see cref="Microsoft.AspNetCore.Mvc.ObjectResult"/> in
        /// order to negotiate a versioned representation.
        /// </summary>
        /// <remarks>
        /// The default is <c>true</c>.
        /// </remarks>
        public bool RequireVersionedObjectResult { get; set; }

        /// <summary>
        /// Gets or sets whether the response will contain a Vary HTTP header
        /// relevant for the applied versioning strategy
        /// </summary>
        /// <remarks>
        /// The default is <c>false</c>.
        /// </remarks>
        public bool EmitVaryHeader { get; set; }

        /// <summary>
        /// Gets or sets whether the version negotiation filter will throw a
        /// <see cref="MissingModelMapperException"/> or just return the
        /// result directly when a mapper cannot be found for the current result.
        /// </summary>
        /// <remarks>
        /// The default is <c>true</c>.
        /// </remarks>
        public bool ThrowOnMissingMapper { get; set; }

        internal List<Type> StrategyTypes { get; set; }

        internal Dictionary<Type, Action<object>> ConfigureStrategy { get; set; }

        /// <summary>
        /// Uses the specified <typeparamref name="TStrategy"/> to determine
        /// resource versions, configured by the <paramref name="configure"/> delegate.
        /// </summary>
        /// <typeparam name="TStrategy">The strategy type to use.</typeparam>
        /// <param name="configure">The configuration delegate.</param>
        public VersionNegotiationOptions UseStrategy<TStrategy>(Action<TStrategy> configure)
            where TStrategy : IVersionStrategy
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            ConfigureStrategy[typeof(TStrategy)] = strategy => configure((TStrategy) strategy);

            return UseStrategy<TStrategy>();
        }

        /// <summary>
        /// Uses the specified <typeparamref name="TStrategy"/> to determine resource versions.
        /// </summary>
        /// <typeparam name="TStrategy">The strategy type to use.</typeparam>
        public VersionNegotiationOptions UseStrategy<TStrategy>()
            where TStrategy : IVersionStrategy
        {
            StrategyTypes.Add(typeof(TStrategy));
            return this;
        }
    }
}