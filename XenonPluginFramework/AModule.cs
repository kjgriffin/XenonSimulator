using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Plugin.Framework;
using Xenon.Sim.Core;

namespace XenonPluginFramework
{
    public abstract class AModule : IXenonPlugin
    {

        public virtual string Type { get; }

        public virtual string Guid { get; set; } = "";

        public HashSet<string> mynoderefs;
        //IList<string> ISimModule.MyNodeRefs { get => mynoderefs; set => mynoderefs = value; }
        public HashSet<string> myportrefs;
        //IList<string> ISimModule.MyPortRefs { get => myportrefs; set => myportrefs = value; }
        public HashSet<string> mysubmodulerefs;
        //IList<string> ISimModule.MySubModuleRefs { get => mysubmodulerefs; set => mysubmodulerefs = value; }
        public Dictionary<string, string> myargs;
        //IList<string> ISimModule.MyArgs { get => myargs; set => myargs = value; }
        public Dictionary<string, string> mysimargs;
        //IList<string> ISimModule.MySimArgs { get => mysimargs; set => mysimargs = value; }

        public virtual void AddNode(string id)
        {
            mynoderefs.Add(id);
        }

        public virtual void AddPort(string id)
        {
            myportrefs.Add(id);
        }

        public virtual void AddSubModule(string id)
        {
            mysubmodulerefs.Add(id);
        }

        public virtual void AddArg(string arg, string value)
        {
            myargs.Add(arg, value);
        }

        public virtual void AddSimArg(string simarg, string value)
        {
            mysimargs.Add(simarg, value);
        }


        public virtual void Init(Func<string, ISimModule> getModule)
        {
            foreach (string id in mysubmodulerefs)
            {
                getModule(id).Init(getModule);
            }
        }

        public virtual void Render()
        {
            throw new NotImplementedException();
        }

        public virtual void Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode)
        {
            throw new NotImplementedException();
        }

        public virtual void Create()
        {
            mynoderefs = new HashSet<string>();
            myportrefs = new HashSet<string>();
            mysubmodulerefs = new HashSet<string>();
            myargs = new Dictionary<string, string>();
            mysimargs = new Dictionary<string, string>();
        }

        public virtual void DebugLog()
        {
            Console.WriteLine(string.Format("TestBox: {0}", Guid));
            Console.WriteLine("Args");
            foreach (KeyValuePair<string, string> arg in myargs)
            {
                Console.WriteLine(string.Format("{0} {1}", arg.Key, arg.Value));
            }
            Console.WriteLine("SimArgs");
            foreach (KeyValuePair<string, string> arg in mysimargs)
            {
                Console.WriteLine(string.Format("{0} {1}", arg.Key, arg.Value));
            }
            Console.WriteLine("PortRefs");
            foreach (string id in myportrefs)
            {
                Console.WriteLine(id);
            }
            Console.WriteLine("NodeRefs");
            foreach (string id in mynoderefs)
            {
                Console.WriteLine(id);
            }
            Console.WriteLine("SubModules");
            foreach (string id in mysubmodulerefs)
            {
                Console.WriteLine(id);
            }


            Console.WriteLine();
        }

    }
}
