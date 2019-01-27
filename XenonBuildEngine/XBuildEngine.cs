using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Xenon.Simulator;
using Xenon.Sim.Core;
using System.Text.RegularExpressions;
using System.IO;

namespace Xenon.Build.Engine
{
    public class XBuildEngine
    {

        Dictionary<string, Action<string>> commandTable = new Dictionary<string, Action<string>>();


        const string UNIVERSEPREFIX = @"/.";

        // State
        string prefix = "";
        Dictionary<string, string> symbolTable = new Dictionary<string, string>();
        SimUniverse universe = new SimUniverse();

        bool runEngine;

        public void StartEngine()
        {
            runEngine = true;
            while (runEngine)
            {
                // accept build arguments
                Console.Write(">");
                string cmd = Console.ReadLine();
                RunCommand(cmd);
            }
        }

        public SimUniverse GetSimUniverse()
        {
            return universe;
        }

        public void RunCommand(string cmd)
        {
            DispatchCommand(cmd);
        }

        bool DispatchCommand(string cmd)
        {
            // split to command and args
            // if command found dispatch, else error
            if (commandTable.ContainsKey(GetCommand(cmd)))
            {
                try
                {
                    commandTable[GetCommand(cmd)](cmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("XBUILDENGINE: error02 [bad cmd]{1}{0}", ex.ToString(), Environment.NewLine);
                }
            }
            else
            {
                Console.WriteLine("XBUILDENGINE: error01 [unknown cmd]");
            }
            

            return true;
        }


        string GetCommand(string cmd)
        {
            // split excess and get cmd
            return cmd.Split(new[] { " " }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        List<string> GetArgs(string cmd)
        {
            // split into args
            List<string> all = cmd.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            all.Remove(all.FirstOrDefault());
            List<string> all_expanded = new List<string>();
            foreach (string arg in all)
            {
                all_expanded.Add(expand_expression(arg));
            }

            return all_expanded;
        }

        public XBuildEngine()
        {
            // Add supported args
            commandTable.Add("#reset", _cmd_Reset);
            commandTable.Add("#load", _cmd_Load);
            commandTable.Add("#make", _cmd_Make);
            commandTable.Add("#pushprefix", _cmd_PushPrefix);
            commandTable.Add("#popprefix", _cmd_PopPrefix);
            commandTable.Add("#runscript", _cmd_RunScript);
            commandTable.Add(".start", _cmd_Start);
            commandTable.Add(".add", _cmd_Add);
            commandTable.Add(".register", _cmd_Register);
            commandTable.Add(".link", _cmd_Link);
            commandTable.Add(".arg", _cmd_Arg);
            commandTable.Add(".simarg", _cmd_SimArg);
            commandTable.Add(".log", _cmd_debug_Log);
            commandTable.Add(".end", _cmd_End);
        }

        private void _cmd_RunScript(string obj)
        {
            List<string> args = GetArgs(obj);

            if (File.Exists(args[0]))
            {
                var lines = File.ReadLines(args[0]);
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        Console.WriteLine("XBUILDENGINE::running_script:  {0}", line);
                        RunCommand(line);
                    }
                }
            }
            else
            {
                Console.WriteLine("XBUILDENGINE: error02 [bad script]");
            }
        }

        private void _cmd_End(string cmd)
        {
            runEngine = false;
        }

        private void _cmd_debug_Log(string cmd)
        {
            Console.Write("XBUILDENGINE::LOG::");
            List<string> args = GetArgs(cmd);
            if (args.Count == 0)
            {
                Console.WriteLine();
                return;
            }
            switch (args[0])
            {
                case "prefix":
                    _cmd_debug_Log_Prefix();
                    break;
                case "symbols":
                    _cmd_debug_Log_SymbolTable();
                    break;
                case "universe":
                    _cmd_debug_Log_Universe_Structure();
                    break;
                default:
                    break;
            }
        }

        private void _cmd_debug_Log_Universe_Structure()
        {
            universe.DebugLog();
        }

        private void _cmd_debug_Log_Prefix()
        {
            Console.WriteLine("PREFIX");
            Console.WriteLine(string.Format("Prefix{0}{1}", Environment.NewLine, prefix));
        }

        private void _cmd_debug_Log_SymbolTable()
        {
            Console.WriteLine("SymbolTable:");
            foreach (KeyValuePair<string, string> pair in symbolTable)
            {
                Console.WriteLine(string.Format("\t{0} : {1}", pair.Key, pair.Value));
            }
        }

        void _cmd_Reset(string cmd)
        {
            prefix = UNIVERSEPREFIX;
            symbolTable.Clear();
        }

        private void _cmd_Load(string cmd)
        {
            List<string> args = GetArgs(cmd);
            PluginLoader.LoadPlugins(args[0]);
        }

        void _cmd_Make(string cmd)
        {
            List<string> args = GetArgs(cmd);
            symbolTable.Add(args[0], args[1]);
        }

        void _cmd_PushPrefix(string cmd)
        {
            string append = GetArgs(cmd).FirstOrDefault();
            string mod = "";
            if (!prefix.EndsWith("."))
            {
                mod = ".";
            }
            prefix += string.Format("{1}{0}", append, mod);
        }

        void _cmd_PopPrefix(string cmd)
        {
            prefix = __remove_suffix(prefix);
        }

        string __remove_suffix(string str)
        {
            string[] split = str.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            return string.Format("{0}", string.Join(".", split.Take(split.Length - 1)));
        }

        string __get_suffix(string str)
        {
            string[] split = str.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            return split.Last();
        }

        void _cmd_Start(string cmd)
        {
            universe = new SimUniverse();
            universe.Create();
        }

        void _cmd_Add(string cmd)
        {
            List<string> args = GetArgs(cmd);
            string type = args.ElementAt(0);
            args.RemoveAt(0);
            switch (type)
            {
                case "node":
                    __cmd_Add_Node(args);
                    break;
                case "port":
                    __cmd_Add_Port(args);
                    break;
                case "module":
                    __cmd_Add_Module(args);
                    break;
                default:
                    break;
            }
        }

        void __cmd_Add_Node(List<string> args)
        {
            ISimNode node = new SimNode();
            node.Guid = args[0];
            universe.AddNode(args[0], node);
        }

        void __cmd_Add_Port(List<string> args)
        {
            ISimEndpoint port = new SimEndpoint();
            port.SetMode(args[2].ToSimEndpointMode());
            port.DataType = args[1];
            port.Guid = args[0];
            universe.AddPort(args[0], port);
        }

        void __cmd_Add_Module(List<string> args)
        {
            if (args.Count == 2)
            {
                ISimModule module = (ISimModule)Activator.CreateInstance(PluginLoader.Plugins[args[1]]);
                module.Guid = args[0];
                universe.AddModule(args[0], module);
            }
            else
            {
                ISimModule module = new SimModule();
                module.Guid = args[0];
                universe.AddModule(args[0], module);
            }
        }

        void _cmd_Register(string cmd)
        {
            List<string> args = GetArgs(cmd);
            string type = args.ElementAt(0);
            args.RemoveAt(0);
            switch (type)
            {
                case "node":
                    __cmd_Register_Node(args);
                    break;
                case "port":
                    __cmd_Register_Port(args);
                    break;
                case "module":
                    __cmd_Register_Module(args);
                    break;
                default:
                    break;
            }
        }

        void __cmd_Register_Node(List<string> args)
        {
            if (args[1] == UNIVERSEPREFIX)
            {
                universe.AddInternalNode(args[0]);
            }
            else
            {
                universe.GetModule(args[1]).AddNode(args[0]);
            }
        }

        void __cmd_Register_Port(List<string> args)
        {
            if (args[1] == UNIVERSEPREFIX)
            {
                Console.WriteLine("ERROR: universe cant have ports, it's self contained");
            }
            else
            {
                universe.GetModule(args[1]).AddPort(args[0]);
            }
        }

        void __cmd_Register_Module(List<string> args)
        {
            if (args[1] == UNIVERSEPREFIX)
            {
                universe.AddSubModule(args[0]);
            }
            else
            {
                universe.GetModule(args[1]).AddSubModule(args[0]);
            }
        }

        void _cmd_Link(string cmd)
        {
            List<string> args = GetArgs(cmd);
            universe.GetNode(args[1]).AddConnection(args[0]);
        }

        void _cmd_Arg(string cmd)
        {
            List<string> args = GetArgs(cmd);
            universe.GetModule(args[0]).AddArg(__get_suffix(args[1]), getSymbolValue(args[1]));
        }

        void _cmd_SimArg(string cmd)
        {
            List<string> args = GetArgs(cmd);
            universe.GetModule(args[0]).AddSimArg(__get_suffix(args[1]), getSymbolValue(args[2]));
        }

        string getSymbolValue(string id)
        {
            string value = symbolTable[id];
            while(!value.StartsWith("%"))
            {
                value = symbolTable[value];
            }
            return value.Substring(1);
        }


        string expand_expression(string expr)
        {
            // expand "*" to current prefix, if * is not end of string, add .
            // expand @ to remove last prefix on item
            Regex expandre = new Regex(@"(\*)");

            string expansion = prefix;

            if (expr.StartsWith("@"))
            {
                expr = expr.Substring(1);
                expansion =__remove_suffix(expansion);
            }

            if (!expr.EndsWith("*") && !expansion.EndsWith("."))
            {
                expansion += ".";
            }

            return expandre.Replace(expr, expansion);
        }


    }
}
