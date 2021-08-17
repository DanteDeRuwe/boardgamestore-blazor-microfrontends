using System;

namespace MicrofrontendFramework.Blazor
{
    public sealed class FragmentAttribute : Attribute
    {
        public string SlotName { get; private set; }
        
        /// <summary> Registers a fragment </summary>
        public FragmentAttribute() { }

        /// <summary> Registers a fragment for rendering in a slot with a custom name</summary>
        public FragmentAttribute(string slotName)
        {
            SlotName = slotName;
        }
    }
}
