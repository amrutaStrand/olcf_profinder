namespace Events
{
    using Microsoft.Practices.Prism.Events;
    using DataTypes;
    using System.Collections.Generic;

    /// <summary>
    ///     The loaded songs changed event.
    /// </summary>
    public class CompoundSelectionChanged : CompositePresentationEvent<ICompoundGroup>
    {

    }
}