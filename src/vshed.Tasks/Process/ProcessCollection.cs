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
using System.Configuration;


namespace vshed.Tasks
{
    public class ProcessCollection : ConfigurationElementCollection
    {
        public new const string XMLDocumentTag = "Processes";

        public ProcessCollection()
        {
            Process e = (Process)CreateNewElement();
            if (e.Name != "") { Add(e); }

        }
        protected override System.Configuration.ConfigurationElement CreateNewElement() { return new Process(); }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element) { return ((Process)element).Name; }

        public new Process this[int index]
        {
            get { return (Process)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        new public Process this[string Name] { get { return (Process)BaseGet(Name); } }

        public int IndexOf(Process e) { return BaseIndexOf(e); }

        public void Add(Process e) { BaseAdd(e); }

        public void Add(ProcessCollection e)
        {
            foreach (Process ep in e)
                if (IndexOf(ep.Name) ==-1) BaseAdd(ep);
        }

        public void Remove(Process e) { if (BaseIndexOf(e) >= 0) BaseRemove(e.Name); }

        public override bool IsReadOnly()
        {
            return false;
        }

        public static ProcessCollection operator +(ProcessCollection a, ProcessCollection b)
        {
            a.Add(b);
            return a;
        }

        protected override String ElementName { get { return Process.XMLDocumentTag; } }

    }
}
