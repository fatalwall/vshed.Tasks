/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/vshed.Tasks
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        { 
            vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
            Settings.Processes["ipconfig Success"].Start();
            System.Diagnostics.Debug.WriteLine(String.Format("Processes: {0}", Settings.Processes.Count));
            foreach (var p in Settings.Processes) System.Diagnostics.Debug.WriteLine(String.Format("  {0}", p.ToString()));
            System.Diagnostics.Debug.WriteLine(String.Format("SerialCommands: {0}", Settings.SerialCommands.Count));
            foreach (var p in Settings.SerialCommands) System.Diagnostics.Debug.WriteLine(String.Format("  {0}", p.ToString()));

            System.Diagnostics.Debug.WriteLine(Settings.ExpandVariables("Ping Exit Code is: ${ipconfig Success:Property->ExitCode}"));
            System.Diagnostics.Debug.WriteLine(Settings.ExpandVariables("Ping Exit Code is: ${ipconfig Success:Var->IP}"));
        }
    }
}
