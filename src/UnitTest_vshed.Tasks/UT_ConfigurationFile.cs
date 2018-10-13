/* 
 *Copyright (C) 2018 Peter Varney - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the MIT license, 
 *
 * You should have received a copy of the MIT license with
 * this file. If not, visit : https://github.com/fatalwall/vshed.Tasks
 */

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace UnitTest_vshed.Tasks
{
    [TestClass]
    public class UT_ConfigurationFile
    {
        [TestMethod]
        public void ConfigurationFile_Load()
        {
            try
            {
                vshed.Tasks.Tasks Settings = ConfigurationManager.GetSection("Tasks") as vshed.Tasks.Tasks;
                Assert.IsTrue(Settings.Processes.Count> 0 && Settings.SerialCommands.Count > 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
