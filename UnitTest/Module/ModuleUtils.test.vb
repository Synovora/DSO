Imports Oasis_Common

<TestClass()> Public Class TestModuleOutils
    <TestMethod()> Public Sub Coalesce()
        Assert.AreEqual(False, ModuleUtils.Coalesce(False, True))
        Assert.AreEqual(True, ModuleUtils.Coalesce(Nothing, True))
        Assert.AreEqual(0, ModuleUtils.Coalesce(Nothing, 0))
    End Sub

    <TestMethod()> Public Sub EncryptStringDecryptString()
        Assert.AreEqual("This Is A Test", ModuleUtils.DecryptString(ModuleUtils.EncryptString("This Is A Test")))
    End Sub

    <TestMethod()> Public Sub IsValidEmail()
        Assert.AreEqual(True, ModuleUtils.IsValidEmail("test@test.com"))
        Assert.AreEqual(False, ModuleUtils.IsValidEmail("test.test.com"))
        Assert.AreEqual(False, ModuleUtils.IsValidEmail("test@testcom"))
    End Sub

    <TestMethod()> Public Sub isValidePassword()
        Assert.AreEqual(False, ModuleUtils.isValidePassword("test"))
        Assert.AreEqual(False, ModuleUtils.isValidePassword("qwertqwer"))
        Assert.AreEqual(False, ModuleUtils.isValidePassword("Qwertqwer"))
        Assert.AreEqual(False, ModuleUtils.isValidePassword("Qwertqwer1"))
        Assert.AreEqual(True, ModuleUtils.isValidePassword("Qwertqwer1@"))
    End Sub
End Class
