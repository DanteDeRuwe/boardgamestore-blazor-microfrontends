using System.Reflection;

namespace MicrofrontendFramework.Blazor;

internal class FragmentMap
{
    private readonly IDictionary<string, ComponentCollection> _dict;
    private FragmentMap(IDictionary<string, ComponentCollection> dict) => _dict = dict;

    public static FragmentMap FromComponents(ComponentCollection components)
    {
        var dict = components.SelectMany(GetSlotNames, (component, slot) => (component, slot))
            .GroupBy(p => p.slot)
            .ToDictionary(g => g.Key, g => new ComponentCollection(g.Select(p => p.component)));

        return new FragmentMap(dict);
    }

    public ComponentCollection Get(string slotName) =>
        _dict.TryGetValue(slotName, out var components) ? components : new ComponentCollection();

    private static IEnumerable<string> GetSlotNames(Type component) =>
        component.GetCustomAttributes<FragmentAttribute>().Select(attr => attr.SlotName);
}