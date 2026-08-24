Imports System.Data
Imports Oasis_Common

''' <summary>
''' Lecture d'une spécialité Theriaque par TheriaqueDao.BuildBean, qui reçoit
''' un DataTable entier et lit sa première ligne.
''' </summary>
<TestClass()> Public Class TestTheriaqueDaoLecture

    Private Shared Function Table(code As Object, nom As Object, nomLong As Object) As DataTable
        Dim t As New DataTable()
        For Each colonne In {"SP_CODE_SQ_PK", "SP_NOM", "SP_NOMLONG"}
            t.Columns.Add(colonne, GetType(Object))
        Next
        t.Rows.Add(code, nom, nomLong)
        Return t
    End Function

    <TestMethod()> Public Sub UneLigneCompleteEstLue()
        Dim s = TheriaqueDao.BuildBean(Table(61234567, "PARACETAMOL", "PARACETAMOL 1 g comprimé"))
        Assert.AreEqual(61234567, s.Id)
        Assert.AreEqual("PARACETAMOL", s.Dci)
        Assert.AreEqual("PARACETAMOL 1 g comprimé", s.DciLongue)
        ' CodeAtc est lu sur la colonne du code de spécialité, pas sur un code
        ' ATC. Le test fige ce que fait le code ; à revoir avec la table source.
        Assert.AreEqual("61234567", s.CodeAtc)
    End Sub

    <TestMethod()> Public Sub LeMarqueurDeSectionEstRetireDuNom()
        Dim s = TheriaqueDao.BuildBean(Table(1, "PARA§CETAMOL", "x"))
        Assert.AreEqual("PARACETAMOL", s.Dci)
    End Sub

    <TestMethod()> Public Sub UnNomAbsentDonneUneChaineVide()
        ' Replace était appelé sur la valeur brute : sur DBNull, c'est une
        ' MissingMemberException et la spécialité ne se chargeait pas.
        Dim s = TheriaqueDao.BuildBean(Table(1, DBNull.Value, DBNull.Value))
        Assert.AreEqual("", s.Dci)
        Assert.AreEqual("", s.DciLongue)
    End Sub

End Class
