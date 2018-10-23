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
    public class UT_Process
    {
        [TestMethod]
        public void Process_Start_WithRegEx_Fail()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Settings.Processes["ipconfig Fail"].Start();
                Assert.IsFalse(Settings.Processes["ipconfig Fail"].Successfull);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Process_Start_WithRegEx_Success()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Settings.Processes["ipconfig Success"].Start();
                Assert.IsTrue(Settings.Processes["ipconfig Success"].Successfull);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Process_Start()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Settings.Processes["Pause"].Start();
                Assert.IsTrue(Settings.Processes["Pause"].Successfull && Settings.Processes["Pause"].ExitCode == 0);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
