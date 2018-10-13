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
using System.Text.RegularExpressions;
using System.Configuration;


namespace vshed.Tasks
{
    public class Process: ConfigurationElement
    {
        public new const string XMLDocumentTag = "Process"; 

        #region Configuration Parts
        [ConfigurationProperty("WorkingDirectory", IsKey = false, IsRequired = false)]
        public string WorkingDirectory
        {
            get { return (string)base["WorkingDirectory"]; }
            set { this["WorkingDirectory"] = value; }
        }

        [ConfigurationProperty("FileName", IsKey = false, IsRequired = true)]
        public string FileName
        {
            get { return (string)base["FileName"]; }
            set { this["FileName"] = value; }
        }

        [ConfigurationProperty("Arguments", IsKey = false, IsRequired = false)]
        public string Arguments
        {
            get { return (string)base["Arguments"]; }
            set { this["Arguments"] = value; }
        }

        [ConfigurationProperty("UserName", IsKey = false, IsRequired = false)]
        public string UserName
        {
            get { return (string)base["UserName"]; }
            set { this["UserName"] = value; }
        }

        [ConfigurationProperty("Password", IsKey = false, IsRequired = false)]
        public string Password
        {
            get { return (string)base["Password"]; }
            set { this["Password"] = value; }
        }

        [ConfigurationProperty("SuccessRegex", IsKey = false, IsRequired = false)]
        public string SuccessRegex
        {
            get { return (string)base["SuccessRegex"]; }
            set { this["SuccessRegex"] = value; }
        }
        #endregion

        public string StandardOutput { get; set; }
        public int ExitCode { get; set; }
        public bool Successfull { get; set; }
        public string ResultMessage { get; set; }

        public bool Start()
        {
            System.Diagnostics.Process app = new System.Diagnostics.Process();
            app.StartInfo.FileName = this.FileName;
            app.StartInfo.WorkingDirectory = this.WorkingDirectory;
            app.StartInfo.Arguments = this.Arguments;
            app.StartInfo.UserName = this.UserName;
            app.StartInfo.Password = new System.Security.SecureString();
            if (!String.IsNullOrWhiteSpace(this.Password)) this.Password.ToList().ForEach(app.StartInfo.Password.AppendChar);
            app.StartInfo.UseShellExecute = false;
            app.StartInfo.RedirectStandardOutput = true;
            app.StartInfo.CreateNoWindow = true;
            app.Start();

            this.StandardOutput = app.StandardOutput.ReadToEnd();
            app.WaitForExit();
            this.ExitCode = app.ExitCode;
            if (!String.IsNullOrWhiteSpace(this.SuccessRegex))
            {
                try
                {
                    List<string> status = Regex.Matches(this.StandardOutput, this.SuccessRegex.Replace("\\R", "\r\n"))
                                                        .Cast<Match>()
                                                        .Select(m => m.Value)
                                                        .ToList();
                    if (status.Count() > 0)
                    {
                        this.ResultMessage="SuccessRegex matched StandardOutput";
                        this.Successfull = true;
                        return true;
                    }
                    else
                    {
                        this.ResultMessage = "SuccessRegex did not match StandardOutput";
                        this.Successfull = false;
                        return false;
                    }
                }
                catch (ArgumentException)
                {
                    
                    this.ResultMessage = "Argument Exception: Value of SuccessRegex is invalid";
                    this.Successfull = false;
                    return false; //Error with regular expresion
                }
            }
            else
            {
                this.ResultMessage = "Process Completed";
                this.Successfull = true;
                return true;
            }
        }
    }
}
