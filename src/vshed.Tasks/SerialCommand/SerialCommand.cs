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
    public class SerialCommand : ConfigurationElement
    {
        public new const string XMLDocumentTag = "SerialCommand";

        private bool initilized = false;
        public SerialCommand() {
            this.Port = new System.IO.Ports.SerialPort();
        }
        public SerialCommand(String Name, System.IO.Ports.SerialPort Port, String Command, String SuccessRegex = null)
        {
            this.Name = Name;
            this.Port = Port;
            this.Command = Command;
            this.SuccessRegex = SuccessRegex;
        }

        [ConfigurationProperty("Port", IsKey = false, IsRequired = true)]
        public System.IO.Ports.SerialPort Port
        {
            get {
                if (initilized==true & ((System.IO.Ports.SerialPort)base["Port"]).IsOpen == false)
                {
                    //FIXME((System.IO.Ports.SerialPort)base["Port"]).PortName = Config.Variable.Do.EvaluateVariables(((System.IO.Ports.SerialPort)base["Port"]).PortName);
                }
                return base["Port"] as System.IO.Ports.SerialPort; 
            }
            set { base["Port"] = value; }
        }

        [ConfigurationProperty("Command", IsKey = false, IsRequired = true)]
        public string Command
        {
            get { return base["Command"] as string; }
            set { base["Command"] = value; }
        }

        [ConfigurationProperty("SuccessRegex", IsKey = false, IsRequired = false)]
        public string SuccessRegex
        {
            get { return base["SuccessRegex"] as string; }
            set { base["SuccessRegex"] = value; }
        }

        [System.ComponentModel.DefaultValue("")]
        public string Output { get; set; }


        public bool Successfull { get; set; }
        public string ResultMessage { get; set; }

        public bool Start()
        {
            try
            {
                this.Port.Open();
                if (this.Port.IsOpen)
                {
                    this.Port.Write(this.Command + "\r");

                    while (string.IsNullOrWhiteSpace(this.Output)) { this.Output += this.Port.ReadExisting(); }
                    this.Port.Close();
                    try {
                    if (!String.IsNullOrWhiteSpace(this.SuccessRegex))
                    {
                        List<string> status = Regex.Matches(this.Output, this.SuccessRegex)
                                                        .Cast<Match>()
                                                        .Select(m => m.Value)
                                                        .ToList();
                        if (status.Count() > 0)
                        {
                            this.ResultMessage = "SuccessRegex matched Output";
                            this.Successfull = true;
                            return true; 
                        } else {
                            this.ResultMessage = "SuccessRegex did not match Output";
                            this.Successfull = false;
                            return false; 
                        }
                    }
                    else {
                            this.ResultMessage = "Process Completed";
                            this.Successfull = true;
                            return true; 
                    };
                    }
                    catch (ArgumentException)
                    {

                        this.ResultMessage = "Argument Exception: Value of SuccessRegex is invalid";
                        this.Successfull = false;
                        return false; //Error with regular expresion
                    }
                }
                else {
                    this.ResultMessage = "Port is not open";
                    this.Successfull = false;
                    return false; 
                }
            }
            catch(Exception)
            {
                this.ResultMessage = "Process Failed";
                this.Successfull = false;
                return false;
            }
        }

        protected override void DeserializeElement(System.Xml.XmlReader reader, bool serializeCollectionKey)
        {
            //Read Attributes
            for (int i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                //string readerValue = Config.Variable.Do.EvaluateVariables(reader.Value);
                string readerValue = reader.Value;
                try { this[reader.Name] = readerValue; }
                catch (Exception)
                {
                    try {
                        try
                        {
                            string t = this.Port.GetType().GetProperty(reader.Name).PropertyType.ToString();
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        switch (this.Port.GetType().GetProperty(reader.Name).PropertyType.ToString())
                        {
                            case "System.IO.Ports.Parity":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), readerValue, true), null); 
                                break;
                            case "System.IO.Ports.StopBits":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), readerValue, true), null); 
                                break;
                            case "System.Text.Encoding":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, Encoding.GetEncoding(readerValue), null); 
                                break;
                            case "System.Boolean":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, Boolean.Parse(readerValue), null); 
                                break;
                            case "System.Int16":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, Int16.Parse(readerValue), null); 
                                break;
                            case "System.Int32":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, Int32.Parse(readerValue), null); 
                                break;
                            case "System.Int64":
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, Int64.Parse(readerValue), null); 
                                break;
                            default:
                                this.Port.GetType().GetProperty(reader.Name).SetValue(this.Port, readerValue, null); 
                                break;

                        }
                    }
                    catch (Exception e) { throw new ConfigurationErrorsException("XMLPart:SerialCommand, Attribute:" + reader.Name + ", Error:Unable to read in Value",e); }
                }
                
            }
            initilized = true;
        }
    }
}
