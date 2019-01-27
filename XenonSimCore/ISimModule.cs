using System;
using System.Collections.Generic;
using System.Text;

namespace Xenon.Sim.Core
{
    public interface ISimModule
    {
        //IList<string> MyNodeRefs { get; set; }
        //IList<string> MyPortRefs { get; set; }
        //IList<string> MySubModuleRefs { get; set; }

        //IList<string> MyArgs { get; set; }
        //IList<string> MySimArgs { get; set; }

        string Guid { get; set; }

        void Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode);
        void Render(); // Allowed to do nothing

        void AddNode(string id);
        void AddSubModule(string id);
        void AddPort(string id);

        void AddArg(string arg, string value);
        void AddSimArg(string simarg, string value);

        void Init(Func<string, ISimModule> getModule);

        void Create();

        void DebugLog();

    }
}
