using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace BoardgameStore.Utils
{
    public class ComponentCollection : Dictionary<string, Type>
    {
        public ComponentCollection() : base() { }
        public ComponentCollection(IDictionary<string, Type> dictionary) : base(dictionary) { }

        public static ComponentCollection FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            var components = assemblies
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IComponent))))
                .ToList();
            return new ComponentCollection(components.ToDictionary(c => c.FullName, c => c));
        }
    }
}
