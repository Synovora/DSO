Imports Oasis_Common

''' <summary>
''' AppointmentDao n'a ni appelant ni requête : ses deux méthodes lèvent une
''' exception « Undeclared Method » sans toucher à la base. Les tests épinglent ce
''' comportement, pour qu'une implémentation future arrive avec ses propres tests.
''' </summary>
<TestClass()> Public Class AppointmentDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AppointmentDao

    <TestMethod()> Public Sub CreateAppointment_LeveUndeclaredMethod()
        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.CreateAppointment(Nothing))
        Assert.AreEqual("Undeclared Method", erreur.Message)
    End Sub

    <TestMethod()> Public Sub DeleteAppointment_LeveUndeclaredMethod()
        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.DeleteAppointment(Nothing))
        Assert.AreEqual("Undeclared Method", erreur.Message)
    End Sub

End Class
