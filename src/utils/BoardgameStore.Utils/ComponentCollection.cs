using System;
using System.Collections.Generic;

namespace BoardgameStore.Utils
{
    public class ComponentCollection : HashSet<Type>
    {
        public ComponentCollection(IEnumerable<Type> components) : base(components) { }
    }
}
