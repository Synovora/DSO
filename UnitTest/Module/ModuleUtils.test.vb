Imports Oasis_Common

<TestClass()> Public Class TestModuleOutils
    <TestMethod()> Public Sub Coalesce()
        Assert.AreEqual(False, ModuleUtilsBase.Coalesce(False, True))
        Assert.AreEqual(True, ModuleUtilsBase.Coalesce(Nothing, True))
        Assert.AreEqual(0, ModuleUtilsBase.Coalesce(Nothing, 0))
    End Sub

    <TestMethod()> Public Sub EncryptStringDecryptString()
        Assert.AreEqual("This Is A Test", ModuleUtilsBase.DecryptString(ModuleUtilsBase.EncryptString("This Is A Test")))
    End Sub

    <TestMethod()> Public Sub IsValidEmail()
        Assert.AreEqual(True, ModuleUtilsBase.IsValidEmail("test@test.com"))
        Assert.AreEqual(False, ModuleUtilsBase.IsValidEmail("test.test.com"))
        Assert.AreEqual(False, ModuleUtilsBase.IsValidEmail("test@testcom"))
    End Sub

    <TestMethod()> Public Sub isValidePassword()
        Assert.AreEqual(False, ModuleUtilsBase.isValidePassword("test"))
        Assert.AreEqual(False, ModuleUtilsBase.isValidePassword("qwertqwer"))
        Assert.AreEqual(False, ModuleUtilsBase.isValidePassword("Qwertqwer"))
        Assert.AreEqual(False, ModuleUtilsBase.isValidePassword("Qwertqwer1"))
        Assert.AreEqual(True, ModuleUtilsBase.isValidePassword("Qwertqwer1@"))
    End Sub
End Class
