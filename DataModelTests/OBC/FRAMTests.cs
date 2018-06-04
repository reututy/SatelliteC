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
        public void WriteSuccessTest()
        {
            FRAM ram = new FRAM(20);
            byte[] data = { 0x12, 0x34, 0x56, 0x78 };
            Assert.AreNotEqual(0x12, ram.Memory[0]);
            Assert.AreEqual(0, ram.Write(data, 0, 4));
            Assert.AreEqual(0x12, ram.Memory[0]);
            Assert.AreEqual(0x34, ram.Memory[1]);
            Assert.AreEqual(0x56, ram.Memory[2]);
            Assert.AreEqual(0x78, ram.Memory[3]);

            Assert.AreEqual(0, ram.Write(data, 10, 2));
            Assert.AreEqual(0x12, ram.Memory[10]);
            Assert.AreEqual(0x34, ram.Memory[11]);
            Assert.AreNotEqual(0x56, ram.Memory[12]);
            Assert.AreNotEqual(0x78, ram.Memory[13]);

        }

        [TestMethod()]
        public void WriteFailedOutOfRangeTest()
        {
            FRAM ram = new FRAM(10);
            byte[] data = { 0x12, 0x34, 0x56, 0x78 };
            Assert.AreEqual(-2, ram.Write(data, 10, 4));
        }

        [TestMethod()]
        public void ReadSuccessTest()
        {
            FRAM ram = new FRAM(10);
            ram.Memory[0] = 0x12;
            ram.Memory[1] = 0x34;
            ram.Memory[2] = 0x56;
            ram.Memory[3] = 0x78;
            ram.Memory[4] = 0x21;
            ram.Memory[5] = 0x43;
            ram.Memory[6] = 0x65;
            int size = 3;
            byte[] data = new byte[size];
            Assert.AreEqual(0, ram.Read(data, 4, size));
            Assert.AreEqual(0x21, data[0]);
            Assert.AreEqual(0x43, data[1]);
            Assert.AreEqual(0x65, data[2]);
        }

        [TestMethod()]
        public void ReadFailedOutOfRangeTest()
        {
            FRAM ram = new FRAM(10);
            ram.Memory[0] = 0x12;
            ram.Memory[1] = 0x34;
            ram.Memory[2] = 0x56;
            ram.Memory[3] = 0x78;
            ram.Memory[4] = 0x21;
            ram.Memory[5] = 0x43;
            ram.Memory[6] = 0x65;
            int size = 3;
            byte[] data = new byte[size];
            Assert.AreEqual(-2, ram.Read(data, 10, size));
        }

    }
}