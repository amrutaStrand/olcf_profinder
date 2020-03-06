using DataTypes;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events
{
    public class SampleSelectionChanged: CompositePresentationEvent<ISample>
    {
    }
}
