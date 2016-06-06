using System;
using System.IO;

namespace BackOffice.Common
{
    public class PathFinder
    {
        public static string ExecutionPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
        }

        public static DirectoryInfo SolutionDir
        {
            get
            {
                return new DirectoryInfo(new Uri(ExecutionPath).LocalPath).Parent.Parent.Parent;
            }
        }
    }
}
