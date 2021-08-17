using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BoardgameStore.Utils
{
    public class FragmentMap : Dictionary<string, Type>
    {
        public FragmentMap() { }
        public FragmentMap(IDictionary<string, Type> fragmentMap) : base(fragmentMap) { }

        public static FragmentMap FromComponents(IEnumerable<Type> components)
        {
            var dictionary = components.ToDictionary(GetFragmentName, c => c);
            return new FragmentMap(dictionary);
        }

        private static string GetFragmentName(Type component)
        {
            var slotName = component.GetCustomAttributes<FragmentAttribute>()?.FirstOrDefault()?.SlotName;
            return slotName ?? component.FullName;
        }
    }
}
