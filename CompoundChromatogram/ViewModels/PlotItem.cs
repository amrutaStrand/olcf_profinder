using DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agilent.OpenLab.CompoundChromatogram.ViewModels
{
    class PlotItem
    {
        public PlotItem(string name, string group, ICompound compound)
        {
            Name = name;
            Group = group;
            Compound = compound;
        }
        public string Name { get; set; }
        public string Group { get; set; }
        public ICompound Compound { get; set; }
        public Color Color { get; set; }
        public int HorizontalPosition { get; set; }
    }
}
