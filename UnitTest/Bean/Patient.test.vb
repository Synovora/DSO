Imports Oasis_Common

''' <summary>
''' Clé de contrôle du NIR (numéro de sécurité sociale). L'exemple est celui de
''' la documentation INSEE : 2 69 05 49 588 157, clé 80.
''' </summary>
<TestClass()> Public Class TestPatientNir

    <TestMethod()> Public Sub LaCleDeLExempleOfficielEstRetrouvee()
        Assert.AreEqual(80, Patient.CalculModuloNIR(2690549588157L))
    End Sub

    <TestMethod()> Public Sub UnNirCompletValideEstAccepte()
        Assert.IsTrue(Patient.IsValidNIR(269054958815780L))
    End Sub

    <TestMethod()> Public Sub UneCleFausseEstRefusee()
        Assert.IsFalse(Patient.IsValidNIR(269054958815781L))
        Assert.IsFalse(Patient.IsValidNIR(269054958815763L), "63 est la clé calculée à tort sur quinze chiffres")
    End Sub

    <TestMethod()> Public Sub UnChiffreModifieDansLeCorpsEstDetecte()
        Assert.IsFalse(Patient.IsValidNIR(269054958825780L))
        Assert.IsFalse(Patient.IsValidNIR(169054958815780L))
    End Sub

    <TestMethod()> Public Sub LaCleVautNonanteSeptQuandLeResteEstNul()
        ' 97 divise 194 : la clé est alors 97, pas 00.
        Assert.AreEqual(97, Patient.CalculModuloNIR(194))
        Assert.IsTrue(Patient.IsValidNIR(19497))
    End Sub

    <TestMethod()> Public Sub UnNombreTropCourtEstRefuse()
        Assert.IsFalse(Patient.IsValidNIR(0))
        Assert.IsFalse(Patient.IsValidNIR(42))
    End Sub

End Class
