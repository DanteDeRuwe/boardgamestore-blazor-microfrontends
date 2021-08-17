using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BoardgameStore.Utils
{
    public class FragmentMap : Dictionary<string, ComponentCollection>
    {
        public FragmentMap() { }
        public FragmentMap(IDictionary<string, ComponentCollection> dict) : base(dict) { }

        public static FragmentMap FromComponents(ComponentCollection components)
        {
            var map = new FragmentMap();
            foreach (var component in components)
            {
                var name = GetFragmentName(component);
                if (map.ContainsKey(name))
                {
                    map[name].Add(component);
                }
                else
                {
                    map[name] = new ComponentCollection(new[] { component });
                }
            }

            return map;
        }

        private static string GetFragmentName(Type component)
        {
            var slotName = component.GetCustomAttributes<FragmentAttribute>()?.FirstOrDefault()?.SlotName;
            return slotName ?? component.FullName;
        }
    }
}
