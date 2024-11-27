using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MimeDetective.Tests;

[TestClass]
public class MicroTestsAssemblies : MicroTests {
    protected override string RelativeRoot() {
        return base.RelativeRoot() + @"Assemblies\";
    }

    [TestMethod]
    public void ManagedDLL_dll() {
        Test("ManagedDLL.dll");
    }

    [TestMethod]
    public void ManagedExe_exe() {
        Test("ManagedExe.exe");
    }

    [TestMethod]
    public void MixedDLL_dll() {
        Test("MixedDLL.dll");
    }

    [TestMethod]
    public void MixedExe_exe() {
        Test("MixedExe.exe");
    }

    [TestMethod]
    public void NativeDLL_dll() {
        Test("NativeDLL.dll");
    }

    [TestMethod]
    public void NativeExe_exe() {
        Test("NativeExe.exe");
    }

}