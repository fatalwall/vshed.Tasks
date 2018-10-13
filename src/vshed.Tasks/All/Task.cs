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

namespace vshed.Tasks
{
    public class Task : ConfigurationElement
    {
        public new const string XMLDocumentTag = "Task";

        [ConfigurationProperty("Type", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)base["Type"]; }
            set { this["Type"] = value; }
        }

        private List<KeyValuePair<string, string>> Attributes
        { get; set; }

        public string Attribute(string Key)
        {
            foreach (KeyValuePair<string, string> pair in Attributes)
            {
                if (pair.Key == Key)
                    return pair.Value;
            }
            return null;
        }

        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            Attributes = new List<KeyValuePair<string, string>>();
            //Read Attributes
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                switch (reader.Name)
                {
                    case "Name":
                    case "Type":
                        base[reader.Name] = reader.Value;
                        break;
                    default:
                        Attributes.Add(new KeyValuePair<string, string>(reader.Name, reader.Value));
                        break;
                }
            }
        }
    }
}
