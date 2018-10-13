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
    public class ConfigurationElementCollection : System.Configuration.ConfigurationElementCollection
    {
        public const string XMLDocumentTag = "ConfigurationElementCollection";

        protected override System.Configuration.ConfigurationElement CreateNewElement() { return new ConfigurationElement(); }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element) { return ((ConfigurationElement)element).Name; }

        public override System.Configuration.ConfigurationElementCollectionType CollectionType { get { return System.Configuration.ConfigurationElementCollectionType.BasicMap; } }

        public override bool IsReadOnly() { return false; }

        public int IndexOf(string Name)
        {

            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Name.ToUpper() == Name.ToUpper()) return idx;
            }
            return -1;
        }

        public ConfigurationElement this[int index]
        {
            get { return (ConfigurationElement)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                    BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public void Remove(string Name) { BaseRemove(Name); }

        public void RemoveAt(int index) { BaseRemoveAt(index); }
        public void Clear() { BaseClear(); }
    }
}
