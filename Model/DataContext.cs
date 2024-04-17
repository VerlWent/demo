using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo2024.Model
{
    public partial class DataContext:DemoContext
    {
        public static DemoContext context { get; } = new Model.DemoContext();
    }
}
