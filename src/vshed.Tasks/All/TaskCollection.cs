/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/vshed.Tasks
 */

using System;

namespace vshed.Tasks
{

    public class TaskCollection: ConfigurationElementCollection
    {

        public new const string XMLDocumentTag = "Tasks";

        public TaskCollection()
        {
            Task e = (Task)CreateNewElement();
            if (e.Name != "") { Add(e); }

        }

        protected override System.Configuration.ConfigurationElement CreateNewElement() { return new Task(); }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element) { return ((Task)element).Name; }

        public new Task this[int index]
        {
            get { return (Task)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public Task this[string Name] { get { return (Task)BaseGet(Name); } }

        public int IndexOf(Task e) { return BaseIndexOf(e); }

        public void Add(Task e) { BaseAdd(e); }

        public void Remove(Task e) { if (BaseIndexOf(e) >= 0) BaseRemove(e.Name); }

        protected override string ElementName { get { return "Task"; } }

        #region Cast
        public static implicit operator ProcessCollection(TaskCollection tc)
        {
            ProcessCollection pc = new ProcessCollection();
            foreach (Task t in tc)
            {
                if (t.Type == "Process")
                {
                    pc.Add(new Process()
                    {
                        Name = t.Name,
                        FileName = t.Attribute("FileName"),
                        Arguments = t.Attribute("Arguments"),
                        WorkingDirectory = t.Attribute("WorkingDirectory"),
                        UserName = t.Attribute("UserName"),
                        Password = t.Attribute("Password"),
                        SuccessRegex = t.Attribute("SuccessRegex")
                    });
                }
            }
            return pc;
        }

        public static implicit operator SerialCommandCollection(TaskCollection tc)
        {
            SerialCommandCollection sc = new SerialCommandCollection();
            foreach (Task t in tc)
            {
                if (t.Type == SerialCommand.XMLDocumentTag)
                {
                    sc.Add(new SerialCommand()
                    {
                        Name = t.Name,
                        Command = t.Attribute("Command"),
                        Port = new System.IO.Ports.SerialPort()
                        {
                            PortName = t.Attribute("PortName"),
                            BaudRate = int.Parse(t.Attribute("BaudRate")),
                            DataBits = int.Parse(t.Attribute("DataBits")),
                            Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), t.Attribute("Parity"), true),
                            StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), t.Attribute("StopBits"), true),
                            Encoding = System.Text.Encoding.GetEncoding(t.Attribute("Encoding")),
                            DtrEnable = Boolean.Parse(t.Attribute("DtrEnable")),
                            RtsEnable = Boolean.Parse(t.Attribute("RtsEnable"))
                        },
                        SuccessRegex = t.Attribute("SuccessRegex")
                    });
                }
            }
            return sc;
        }
        #endregion
    }
}

