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

    public class Tasks : ConfigurationSection
    {


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
