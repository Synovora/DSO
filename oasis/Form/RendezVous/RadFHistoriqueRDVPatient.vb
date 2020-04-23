Imports Oasis_Common
Public Class RadFHistoriqueRDVPatient
    Private _SelectedPatient As Patient
    Private _SelectedParcoursId As Integer

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property SelectedParcoursId As Integer
        Get
            Return _SelectedParcoursId
        End Get
        Set(value As Integer)
            _SelectedParcoursId = value
        End Set
    End Property

    Dim tacheDao As New TacheDao

    Private Sub RadFHistoriqueRDVPatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementRDV()
    End Sub

    Private Sub ChargementRDV()
        RadGridView.Rows.Clear()
        Dim dt As DataTable
        dt = tacheDao.GetRDVHistoriqueByPatient(SelectedPatient.patientId, SelectedParcoursId)
        Dim iGrid As Integer = -1
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridView.Rows.Add(iGrid)
            Dim dateRendezVous As Date = dt.Rows(i)("date_rendez_vous")
            RadGridView.Rows(iGrid).Cells("date_rendez_vous").Value = dateRendezVous.ToString("dd.MM.yyyy")
        Next

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
