using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Sim.Core;

namespace Xenon.Plugin.Framework
{
    public interface IXenonPlugin : ISimModule
    {
        string Type { get; }
    }
}
