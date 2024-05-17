namespace MicrofrontendFramework.Blazor;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class FragmentAttribute : Attribute
{
    public string SlotName { get; private set; }

    /// <summary> Registers a fragment for rendering in a slot with a custom name</summary>
    public FragmentAttribute(string slotName)
    {
        SlotName = slotName;
    }
}