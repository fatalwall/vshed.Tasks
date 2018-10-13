/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/vshed.Tasks
 */

using System;
using System.Configuration;

namespace vshed.Tasks
{
    public class SerialCommandCollection : ConfigurationElementCollection
    {
        public new const string XMLDocumentTag = "SerialCommands";

        public new SerialCommand this[int index]
        {
            get { return (SerialCommand)BaseGet(index); }
        }

        public new SerialCommand this[String Name]
        {
            get
            {
                if (IndexOf(Name) < 0) return null;
                return (SerialCommand)BaseGet(Name);
            }
        }

        public void Add(SerialCommand Command) { BaseAdd(Command); }

        public void Add(SerialCommandCollection e)
        {
            foreach (SerialCommand es in e)
                BaseAdd(es);
        }

        public void Remove(SerialCommand Command) { if (BaseIndexOf(Command) >= 0) BaseRemove(Command.Name); }


        public SerialCommandCollection()
        {
            SerialCommand e = (SerialCommand)CreateNewElement();
            if (e.Name != "") { Add(e); }

        }

        protected override System.Configuration.ConfigurationElement CreateNewElement() { return new SerialCommand(); }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element) { return ((SerialCommand)element).Name; }

        protected override String ElementName { get { return SerialCommand.XMLDocumentTag; } }


    }
}
