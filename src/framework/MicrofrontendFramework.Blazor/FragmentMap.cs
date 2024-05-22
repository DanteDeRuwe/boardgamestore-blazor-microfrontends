namespace MicrofrontendFramework.Blazor;

internal class FragmentMap
{
    private readonly IDictionary<string, IEnumerable<Type>> _dict;

    private FragmentMap(IDictionary<string, IEnumerable<Type>> dict) => _dict = dict;

    public static implicit operator FragmentMap(Dictionary<string, IEnumerable<Type>> dict) => new(dict);

    public static FragmentMap FromComponents(IEnumerable<Type> components) => components
        .SelectMany(GetSlotNames, (component, slot) => (component, slot))
        .GroupBy(p => p.slot)
        .ToDictionary(g => g.Key, g => g.Select(p => p.component));

    public IEnumerable<Type> Get(string slotName) =>
        _dict.TryGetValue(slotName, out var components) ? components : [];

    private static IEnumerable<string> GetSlotNames(Type component) => component
        .GetCustomAttributes<FragmentAttribute>()
        .Select(attr => attr.SlotName);
}
