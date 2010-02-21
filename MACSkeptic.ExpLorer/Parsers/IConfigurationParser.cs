using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MACSkeptic.ExpLorer.Parsers
{
    public interface IConfigurationParser
    {
        Configuration LoadGlobal(string path);
        Configuration LoadLocal(string path);
    }
}
