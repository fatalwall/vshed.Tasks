/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/vshed.Tasks
 */

using System.Collections.Generic;

namespace vshed.Tasks
{
    public class ConfigurationElement : System.Configuration.ConfigurationElement
    {
        public  ConfigurationElement() { this.Variables = new Dictionary<string, dynamic>(); }
        public const string XMLDocumentTag = "ConfigurationElement";

        [System.Configuration.ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["Name"]; }
            set { this["Name"] = value; }
        }

        public override sealed string ToString()
        {
            return this.Name;
        }
   
        public Dictionary<string, dynamic> Variables { get; set; }
    }
}
