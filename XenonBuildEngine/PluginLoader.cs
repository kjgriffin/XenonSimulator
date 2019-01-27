using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Xenon.Plugin.Framework;
using System.IO;

namespace Xenon.Build.Engine
{
    static class PluginLoader
    {
        public static Dictionary<string, Type> Plugins { get; set; } = new Dictionary<string, Type>();

        public static void LoadPlugins(string path)
        {
            string[] dlls = null;
            if (Directory.Exists(path))
            {
                dlls = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories);
            }
            ICollection<Assembly> assemblies = new List<Assembly>(dlls.Length);
            foreach (string dll in dlls)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dll);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }
            Type plugintype = typeof(IXenonPlugin);
            ICollection<Type> plugintypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(plugintype.FullName) != null)
                            {
                                plugintypes.Add(type);
                            }
                        }
                    }
                }
            }
            foreach (Type type in plugintypes)
            {
                IXenonPlugin p = (IXenonPlugin)Activator.CreateInstance(type);
                p.Create();
                Plugins.Add(p.Type, type);
            }
        }
    }
}
