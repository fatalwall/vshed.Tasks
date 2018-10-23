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
using System.Text.RegularExpressions;
using System.Configuration;


namespace vshed.Tasks
{

    public sealed class Tasks : ConfigurationSection
    {
        private static readonly Tasks instance = ConfigurationManager.GetSection("Tasks") as vshed.Tasks.Tasks;

        private Tasks()
        {
        }

        public static Tasks getCurrentInstance
        {
            get
            {
                return instance;
            }
        }

        public string ExpandVariables(string s)
        {
            if (s is null) { return null; }
            MatchCollection mc = Regex.Matches(s, @"\${(?'Task'.*?)(?:\:(?'Type'(?:[vV][aA][rR]|[pP][rR][oO][pP][eE][rR][tT][yY])))?(?:->(?'Item'.*?))?(?:\[(?'ItemSub1'.*?)\](?:\[(?'ItemSub2'.*?)\])?)?(?:\:[fF][oO][rR][mM][aA][tT]=(?'Format'.*?))?}");
            /* Groups
             *   Task       - Name of the element in Tasks. Returns the objects toString value
             *   Type       - (Optional) Type of value you want (Var, Property)
             *   Item       - (Optional unless Type is provided) Name of the Variable or Property you want
             *   ItemSub1   - (Optional) Sub property of Item (Row if a dataset)
             *   ItemSub2   - (Optional) for use with datasets as Column 
             *   Format     - (Optional - can be used even without any of the previous options set) Intended for decimal lengths and datetime formatting
             *   
             * Example Matchs
             *   ${test}
             *   ${ipconfig Success}
             *   ${ipconfig Success:Var->IP}
             *   ${ipconfig Success:Property->ExitCode}
             *   ${ipconfig Success:Property->ExitCode}
             *   ${Get Cellular Modem Number:Property->Port[BaudRate]}
             *   ${Get Cellular Modem Number:Property->DataSet[Row][Column]}
             *   ${Get Cellular Modem Number:Property->Port[BaudRate]:format=yyyy-MM-dd HH:mm:ss.fff}
             *   ${ipconfig Success:format=yyyy-MM-dd HH:mm:ss.fff}
             * 
            */
            foreach (Match m in mc)
            {
                
                object task = null;
                foreach (System.Configuration.ConfigurationProperty p in this.Properties)
                {
                    switch (p.Name)
                    { 
                        case "Processes":
                            task = ((vshed.Tasks.ProcessCollection)(this.GetType().GetProperty(p.Name).GetValue(this)))[m.Groups["Task"].Value];
                            break;
                        case "SerialCommands":
                            task = ((vshed.Tasks.SerialCommandCollection)(this.GetType().GetProperty(p.Name).GetValue(this)))[m.Groups["Task"].Value];
                            break;
                    }
                    if (!(task is null)) break;
                }

                if (!(task is null))
                {
                    switch (m.Groups["Type"].Value.ToUpper())
                    {
                        case "PROPERTY":                            
                            s = s.Replace(m.Value, task.GetType()?.GetProperty(m.Groups["Item"].Value)?.GetValue(task)?.ToString() ?? "") ;
                            break;
                        case "VAR":
                            var item = (Dictionary<string, dynamic>)(task.GetType()?.GetProperty("Variables")?.GetValue(task));
                            try
                            {   
                                var itemValue = item[m.Groups["Item"].Value];
                                if (itemValue.GetType() == typeof(System.Collections.Generic.Dictionary<string, dynamic>))
                                {
                                    if (itemValue[m.Groups["ItemSub1"].Value].GetType() == typeof(System.Collections.Generic.Dictionary<string, dynamic>))
                                    { s = s.Replace(m.Value, itemValue[m.Groups["ItemSub1"].Value][m.Groups["ItemSub2"].Value] ?? ""); }
                                    else { s = s.Replace(m.Value, itemValue[m.Groups["ItemSub1"].Value] ?? ""); }

                                }
                                //FIX ME - Add processing for Datasets
                                else { s = s.Replace(m.Value, itemValue ?? ""); }  
                            }
                            catch (KeyNotFoundException) { s = s.Replace(m.Value, ""); }  
                            break;
                    }
                }

                //Look for matching variable
                //    m.Groups['Task']
                //    m.Groups['Type']
                //    m.Groups['Item']
                //    m.Groups['ItemSub1']
                //    m.Groups['ItemSub2']
                //    m.Groups['Format']
            }
            return Environment.ExpandEnvironmentVariables(mc.Count == 0 ? s : ExpandVariables(s));
        }

        [ConfigurationProperty(ProcessCollection.XMLDocumentTag, IsDefaultCollection = false)]
        public ProcessCollection Processes
        {
            get
            {
                ProcessCollection TC = All; //Retrieve my Collection items from Task Tags
                ((ProcessCollection)base[ProcessCollection.XMLDocumentTag]).Add(TC); //Add Task Taged Items to correct collection
                foreach (Process p in TC) All.Remove(p.Name); //Remove from collection All
                return ((ProcessCollection)base[ProcessCollection.XMLDocumentTag]);
            }
        }

        [ConfigurationProperty(SerialCommandCollection.XMLDocumentTag, IsDefaultCollection = false)]
        public SerialCommandCollection SerialCommands
        {
            get
            {
                SerialCommandCollection TC = All; //Retrieve my Collection items from Task Tags
                ((SerialCommandCollection)base[SerialCommandCollection.XMLDocumentTag]).Add(TC); //Add Task Taged Items to correct collection
                foreach (SerialCommand p in TC) All.Remove(p.Name); //Remove from collection All
                return ((SerialCommandCollection)base[SerialCommandCollection.XMLDocumentTag]);
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        private TaskCollection All
        {
            get
            {
                return (TaskCollection)base[""];
            }
        }
    }
}
