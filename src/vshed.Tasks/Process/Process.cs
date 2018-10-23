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

        public string Output { get; set; }
        public int? ExitCode { get; set; }
        public bool Successfull { get; set; }
        public string ResultMessage { get; set; }

        public bool Start()
        {
            this.Variables.Clear();
            System.Diagnostics.Process app = new System.Diagnostics.Process();
            app.StartInfo.FileName = vshed.Tasks.Tasks.getCurrentInstance.ExpandVariables(this.FileName);
            app.StartInfo.WorkingDirectory = vshed.Tasks.Tasks.getCurrentInstance.ExpandVariables(this.WorkingDirectory);
            app.StartInfo.Arguments = vshed.Tasks.Tasks.getCurrentInstance.ExpandVariables(this.Arguments);
            app.StartInfo.UserName = vshed.Tasks.Tasks.getCurrentInstance.ExpandVariables(this.UserName);
            app.StartInfo.Password = new System.Security.SecureString();
            if (!String.IsNullOrWhiteSpace(vshed.Tasks.Tasks.getCurrentInstance.ExpandVariables(this.Password))) this.Password.ToList().ForEach(app.StartInfo.Password.AppendChar);
            app.StartInfo.UseShellExecute = false;
            app.StartInfo.RedirectStandardOutput = true;
            app.StartInfo.CreateNoWindow = true;
            app.Start();

            this.Output = app.StandardOutput.ReadToEnd();
            app.WaitForExit();
            this.ExitCode = app.ExitCode;
            if (!String.IsNullOrWhiteSpace(this.SuccessRegex))
            {
                try
                {
                    Regex regex = new Regex(this.SuccessRegex.Replace("\\R", "\r\n"), RegexOptions.None);

                    MatchCollection matches = regex.Matches(this.Output);
                    if (matches.Count > 0)
                    {
                        foreach (string group in regex.GetGroupNames())
                        {
                            Dictionary<string, dynamic> d = new Dictionary<string, dynamic>();
                            foreach (Match m in matches)
                            {
                                if (matches.Count == 1) { this.Variables.Add(group, m.Groups[group].Value); }
                                else { d.Add(d.Count.ToString(), m.Groups[group].Value); }
                            }
                            if (matches.Count > 1) { this.Variables.Add(group, d); }
                        }

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
