﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MimeDetective.Tests {
    [TestClass]
    public class MicroTests_Images : MicroTests {

        protected override string RelativeRoot() {
            return base.RelativeRoot() + @"Images\";
        }

        [TestMethod]
        public void test_bmp() {
            Test("test.bmp");
        }

        [TestMethod]
        public void test_gif() {
            Test("test.gif");
        }

        [TestMethod]
        public void test_ico() {
            Test("test.ico");
        }

        [TestMethod]
        public void test_jpg() {
            Test("test.jpg");
        }

        [TestMethod]
        public void test_png() {
            Test("test.png");
        }


    }

}
