using System;
using System.Windows.Forms;

namespace Gamma_Manager
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window(HasMinimizedStartupArgument(args)));
        }

        private static bool HasMinimizedStartupArgument(string[] args)
        {
            foreach (string argument in args)
            {
                if (string.Equals(argument, Window.MinimizedStartupArgument, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
