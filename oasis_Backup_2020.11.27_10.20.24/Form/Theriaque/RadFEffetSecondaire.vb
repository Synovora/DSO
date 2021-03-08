Imports Oasis_Common

Public Class RadFEffetSecondaire
    Private _medicamentId1 As Integer

    Public Property MedicamentId As Integer
        Get
            Return _medicamentId1
        End Get
        Set(value As Integer)
            _medicamentId1 = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao

    Private Sub RadFEffetSecondaire_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Thériaque - Effets indésirables", userLog)
        ChargementText()
        RadBtnAbandon.Select()
    End Sub

    Private Sub ChargementText()
        Dim dt As DataTable
        dt = theriaqueDao.GetSpecialiteByArgument(MedicamentId, TheriaqueDao.EnumGetSpecialite.ID_THERIAQUE, TheriaqueDao.EnumMonoVir.NULL)
        TextBoxMedicament.Text = dt.Rows(0)("SP_NOMLONG")

        TextBoxClinique.Text = theriaqueDao.GetEffetIndesirableBySpecialite(MedicamentId, TheriaqueDao.EnumTypeEffetIndesirable.CLINIQUE)
        TextBoxParaclinique.Text = theriaqueDao.GetEffetIndesirableBySpecialite(MedicamentId, TheriaqueDao.EnumTypeEffetIndesirable.PARA_CLINIQUE)
        TextBoxCliniqueSurdosage.Text = theriaqueDao.GetEffetIndesirableBySpecialite(MedicamentId, TheriaqueDao.EnumTypeEffetIndesirable.CLINIQUE_SURDOSAGE)
        TextBoxParacliniqueSurdosage.Text = theriaqueDao.GetEffetIndesirableBySpecialite(MedicamentId, TheriaqueDao.EnumTypeEffetIndesirable.PARA_CLINIQUE_SURDOSAGE)
    End Sub

End Class
