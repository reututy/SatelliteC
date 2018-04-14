using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataModel.OBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.OBC.Tests
{
    [TestClass()]
    public class FRAMTests
    {
        [TestMethod()]
        public void FRAMTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WriteTest()
        {
            FRAM ram = new FRAM();
            byte[] data = { 0x12, 0x34, 0x56, 0x78 };
            Assert.AreNotEqual(0x12, ram.memory[0]);
            ram.Write(data, 0, 4);
            Assert.AreEqual(0x12, ram.memory[0]);
            Assert.AreEqual(0x34, ram.memory[1]);
            Assert.AreEqual(0x56, ram.memory[2]);
            Assert.AreEqual(0x78, ram.memory[3]);

            ram.Write(data, 10, 2);
            Assert.AreEqual(0x12, ram.memory[10]);
            Assert.AreEqual(0x34, ram.memory[11]);
            Assert.AreNotEqual(0x56, ram.memory[12]);
            Assert.AreNotEqual(0x78, ram.memory[13]);

        }

        [TestMethod()]
        public void ReadTest()
        {
            FRAM ram = new FRAM();
            ram.memory[0] = 0x12;
            ram.memory[1] = 0x34;
            ram.memory[2] = 0x56;
            ram.memory[3] = 0x78;
            ram.memory[4] = 0x21;
            ram.memory[5] = 0x43;
            ram.memory[6] = 0x65;
            byte[] data = ram.Read(4, 3);
            Assert.AreEqual(0x21, data[0]);
            Assert.AreEqual(0x43, data[1]);
            Assert.AreEqual(0x65, data[2]);
        }
    }
}