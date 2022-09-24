using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ZBingWallpaper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        readonly string[] dlls = new string[] { "D3DCompiler_47_cor3", "PenImc_cor3", "PresentationNative_cor3", "vcruntime140_cor3", "wpfgfx_cor3" };    // 去掉后缀名
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resources = null;
            foreach (var item in dlls)
            {
                if (args.Name.StartsWith(item))
                {
                    resources = item + ".dll";
                    break;
                }
            }
            if (string.IsNullOrEmpty(resources))
            {
                return null;
            }

            var assembly = Assembly.GetExecutingAssembly();
            resources = assembly.GetManifestResourceNames().FirstOrDefault(s => s.EndsWith(resources));

            if (string.IsNullOrEmpty(resources))
            {
                return null;
            }

            using (Stream stream = assembly.GetManifestResourceStream(resources))
            {
                if (stream == null)
                {
                    return null;
                }

                var block = new byte[stream.Length];
                stream.Read(block, 0, block.Length);
                return Assembly.Load(block);
            }
        }
    }
}
