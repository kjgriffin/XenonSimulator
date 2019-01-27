using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Plugin.Framework;
using Xenon.Sim.Core;

namespace XenonCorePlugins
{
    class TestBox : IXenonPlugin
    {
        string IXenonPlugin.Type => "std.test.TestBox";

        string guid = "";
        string ISimModule.Guid { get => guid; set => guid = value; }

        HashSet<string> mynoderefs;
        //IList<string> ISimModule.MyNodeRefs { get => mynoderefs; set => mynoderefs = value; }
        HashSet<string> myportrefs;
        //IList<string> ISimModule.MyPortRefs { get => myportrefs; set => myportrefs = value; }
        HashSet<string> mysubmodulerefs;
        //IList<string> ISimModule.MySubModuleRefs { get => mysubmodulerefs; set => mysubmodulerefs = value; }
        Dictionary<string, string> myargs;
        //IList<string> ISimModule.MyArgs { get => myargs; set => myargs = value; }
        Dictionary<string, string> mysimargs;
        //IList<string> ISimModule.MySimArgs { get => mysimargs; set => mysimargs = value; }

        void ISimModule.AddNode(string id)
        {
            mynoderefs.Add(id);
        }

        void ISimModule.AddPort(string id)
        {
            myportrefs.Add(id);
        }

        void ISimModule.AddSubModule(string id)
        {
            mysubmodulerefs.Add(id);
        }

        void ISimModule.AddArg(string arg, string value)
        {
            myargs.Add(arg, value);
        }

        void ISimModule.AddSimArg(string simarg, string value)
        {
            mysimargs.Add(simarg, value);
        }


        void ISimModule.Init(Func<string, ISimModule> getModule)
        {
            foreach (string id in mysubmodulerefs)
            {
                getModule(id).Init(getModule);
            }
        }

        void ISimModule.Render()
        {
            throw new NotImplementedException();
        }

        void ISimModule.Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode)
        {
            Console.WriteLine(string.Format("TestBox::{0} update_tick:{1}", guid, g_tick));
            // expects simargs for channels
            int channels = Convert.ToInt32(myargs["channels"]);
            // simply log the output to console for now and move inputs to outputs
            for (int i = 0; i < channels; i++)
            {
                getPort(string.Format("{1}.out${0}", i, guid)).SetValue(getPort(string.Format("{1}.in${0}", i, guid)).GetValue());
            }

        }
        
        void ISimModule.Create()
        {
            mynoderefs = new HashSet<string>();
            myportrefs = new HashSet<string>();
            mysubmodulerefs = new HashSet<string>();
            myargs = new Dictionary<string, string>();
            mysimargs = new Dictionary<string, string>();
        }

        void ISimModule.DebugLog()
        {
            Console.WriteLine(string.Format("TestBox: {0}", guid));
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


        /* use
         * 
         * #define std.test.TestBox
         * .TestBox^channels:std.types.vector
         * .TestBox>in[channels]:input:std.data.any
         * .TestBox>out[channels]:output:std.data.any
         * #end define
         * 
        */

    }
}
