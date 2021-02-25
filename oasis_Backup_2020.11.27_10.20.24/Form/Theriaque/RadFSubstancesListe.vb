Imports System.Collections.Specialized
Imports Oasis_Common

Public Class RadFSubstancesListe
    Private _SelectedSpecialite As Integer

    Public Property SelectedSpecialite As Integer
        Get
            Return _SelectedSpecialite
        End Get
        Set(value As Integer)
            _SelectedSpecialite = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao

    Private Sub RadFSubstancesListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim dt As DataTable = theriaqueDao.GetSpecialiteByArgument(SelectedSpecialite, TheriaqueDao.EnumGetSpecialite.ID_THERIAQUE, TheriaqueDao.EnumMonoVir.NULL)
        TextBoxSpecialite.Text = dt.Rows(0)("SP_NOM")

        Dim SubstanceListe As List(Of Integer)
        Dim iGrid As Integer = -1
        SubstanceListe = theriaqueDao.GetSubstanceCodeListBySpecialite(SelectedSpecialite)
        Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
        RadGridViewSubstance.Rows.Clear()
        While EnumeratorSubstanceListe.MoveNext()
            Dim CodeSubstance As Integer = EnumeratorSubstanceListe.Current
            Dim SubstanceDenomination As String = theriaqueDao.GetSubstanceDenominationById(CodeSubstance)
            iGrid += 1
            RadGridViewSubstance.Rows.Add(iGrid)
            RadGridViewSubstance.Rows(iGrid).Cells("SAC_NOM").Value = SubstanceDenomination
        End While
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
