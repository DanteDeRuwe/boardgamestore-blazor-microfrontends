using System;
using System.Collections.Generic;

namespace BoardgameStore.Shared
{
    public class ComponentCollection : HashSet<Type>
    {
        public ComponentCollection(IEnumerable<Type> components) : base(components) { }
    }
}
