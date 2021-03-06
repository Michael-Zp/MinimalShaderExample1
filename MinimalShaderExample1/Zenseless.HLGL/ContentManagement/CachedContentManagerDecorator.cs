﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Zenseless.HLGL
{
    /// <summary>
    /// 
    /// </summary>
    public class CachedContentManagerDecorator : IContentManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedContentManagerDecorator"/> class.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        public CachedContentManagerDecorator(IContentManager contentManager)
        {
            ContentManager = contentManager ?? throw new ArgumentNullException(nameof(contentManager));
        }

        /// <summary>
        /// Gets the decorated content manager.
        /// </summary>
        /// <value>
        /// The content manager.
        /// </value>
        public IContentManager ContentManager { get; private set; }

        /// <summary>
        /// Occurs after a new cache entry was created.
        /// </summary>
        public event EventHandler<NewCacheEntryEventArgs> NewCacheEntry;

        /// <summary>
        /// Enumerates all content resource names.
        /// </summary>
        /// <value>
        /// All content resource names.
        /// </value>
        public IEnumerable<string> Names => ContentManager.Names;

        /// <summary>
        /// Disposes all loaded content instances.
        /// </summary>
        public void DisposeInstances()
        {
            foreach (var instance in instanceCache.Values)
            {
                var disposable = instance as IDisposable;
                if (!(disposable is null))
                {
                    disposable.Dispose();
                }
                instanceCache.Clear();
            }
        }

        /// <summary>
        /// Creates an instance of a given type from the resources with the specified names.
        /// </summary>
        /// <typeparam name="TYPE">The type to create.</typeparam>
        /// <param name="names">A list of resource names.</param>
        /// <returns>
        /// An instance of the given type.
        /// </returns>
        public TYPE Load<TYPE>(IEnumerable<string> names) where TYPE : class
        {
            var name = Combine(names);
            if (instanceCache.TryGetValue(name, out var instance))
            {
                var typedInstance = instance as TYPE;
                if (typedInstance is null) throw new ArgumentException($"Wrong type '{typeof(TYPE).FullName}' for instance of type '{instance.GetType().FullName}'.");
                return typedInstance;
            }
            var fullNames = Names.GetFullNames(names);
            var result = ContentManager.Load<TYPE>(fullNames);
            instanceCache[name] = result;
            NewCacheEntry?.Invoke(this, new NewCacheEntryEventArgs(result, name, fullNames));
            return result;
        }

        /// <summary>
        /// Registers an importer.
        /// </summary>
        /// <typeparam name="TYPE">The return type of the importer.</typeparam>
        /// <param name="importer">The importer instance.</param>
        public void RegisterImporter<TYPE>(Func<IEnumerable<NamedStream>, TYPE> importer) where TYPE : class
        {
            ContentManager.RegisterImporter(importer);
        }

        private Dictionary<string, object> instanceCache = new Dictionary<string, object>();

        private string Combine(IEnumerable<string> names)
        {
            var result = new StringBuilder();
            foreach (var name in names)
            {
                result.Append(name);
            }
            return result.ToString();
        }
    }
}
