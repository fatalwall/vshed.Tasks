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
    public class UT_ExpandVariables
    {
        [TestMethod]
        public void ExpandPropertyWithValue()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Settings.Processes["ipconfig Success"].Start();
                Assert.AreEqual(Settings.ExpandVariables("ipconfig Exit Code is: ${ipconfig Success:Property->ExitCode}"), "ipconfig Exit Code is: 0");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ExpandPropertyWithOutValue()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Assert.AreEqual(Settings.ExpandVariables("ipconfig Exit Code is: ${ipconfig Not Run:Property->ExitCode}"), "ipconfig Exit Code is: ");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ExpandVarWithValue()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Settings.Processes["ipconfig Success"].Start();
                Assert.AreEqual(Settings.ExpandVariables("ipconfig IP is: ${ipconfig Success:Var->IP}"), "ipconfig IP is: 172.16.20.206");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ExpandVarWithValueArray()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Settings.Processes["ipconfig Success Multi Match"].Start();
                Assert.AreEqual(Settings.ExpandVariables("ipconfig IP is: ${ipconfig Success Multi Match:Var->IP[0]}"), "ipconfig IP is: 172.16.20.206");
                Assert.AreEqual(Settings.ExpandVariables("ipconfig IP is: ${ipconfig Success Multi Match:Var->IP[1]}"), "ipconfig IP is: 255.255.252.0");

            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ExpandVarWithValueDataset()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Assert.Fail(); //FIXME Test for Datasets - Requires new Task type 
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ExpandVarWithOutValue()
        {
            try
            {
                vshed.Tasks.Tasks Settings = vshed.Tasks.Tasks.getCurrentInstance;
                Assert.AreEqual(Settings.ExpandVariables("ipconfig IP is: ${ipconfig Not Run:Var->IP}"), "ipconfig IP is: ");
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }
    }
}
