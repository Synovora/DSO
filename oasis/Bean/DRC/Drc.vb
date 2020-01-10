Imports System.Data.SqlClient
Imports System.Collections.Specialized
Public Class Drc
    Private privateDrcId As Integer
    Private privateDrcLibelle As String
    Private privateDrcSexe As Integer       '1:Masculin, 2:Féminin, 3:Les deux
    Private privateDrcTypeEpisode As String 'C:Chronique, A:Aigue, NC:Non concerné
    Private privateDrcAgeMin As Integer
    Private privateDrcAgeMax As Integer
    Private _categorieOasisId As Integer    '1:Contexte et antécédent, 2:Stratégie contextuelle, 3:Mesure préventive, 4:Objectif, 5:Acte paramédical
    Private _categorieMajeure As Integer
    Private _aldId As Integer
    Private _aldCode As String

    Public Property DrcId As Integer
        Get
            Return privateDrcId
        End Get
        Set(value As Integer)
            privateDrcId = value
        End Set
    End Property

    Public Property DrcLibelle As String
        Get
            Return privateDrcLibelle
        End Get
        Set(value As String)
            privateDrcLibelle = value
        End Set
    End Property

    Public Property DrcSexe As Integer
        Get
            Return privateDrcSexe
        End Get
        Set(value As Integer)
            privateDrcSexe = value
        End Set
    End Property

    Public Property DrcTypeEpisode As String
        Get
            Return privateDrcTypeEpisode
        End Get
        Set(value As String)
            privateDrcTypeEpisode = value
        End Set
    End Property

    Public Property DrcAgeMin As Integer
        Get
            Return privateDrcAgeMin
        End Get
        Set(value As Integer)
            privateDrcAgeMin = value
        End Set
    End Property

    Public Property DrcAgeMax As Integer
        Get
            Return privateDrcAgeMax
        End Get
        Set(value As Integer)
            privateDrcAgeMax = value
        End Set
    End Property

    Public Property CategorieOasisId As Integer
        Get
            Return _categorieOasisId
        End Get
        Set(value As Integer)
            _categorieOasisId = value
        End Set
    End Property

    Public Property CategorieMajeure As Integer
        Get
            Return _categorieMajeure
        End Get
        Set(value As Integer)
            _categorieMajeure = value
        End Set
    End Property

    Public Property AldId As Integer
        Get
            Return _aldId
        End Get
        Set(value As Integer)
            _aldId = value
        End Set
    End Property

    Public Property AldCode As String
        Get
            Return _aldCode
        End Get
        Set(value As String)
            _aldCode = value
        End Set
    End Property

    Sub New()
        InitInstance()
    End Sub

    Sub New(DrcId As Integer)
        If DrcId <> 0 Then
            Dim conxn As New SqlConnection(getConnectionString())
            Dim SQLString As String
            Dim DrcDataReader As SqlDataReader
            SQLString = "select * from oasis.oa_drc where oa_drc_id = " & DrcId & ";"
            Dim myCommand As New SqlCommand(SQLString, conxn)

            conxn.Open()
            DrcDataReader = myCommand.ExecuteReader()
            If DrcDataReader.Read() Then
                Me.DrcId = DrcId
                Me.DrcLibelle = Coalesce(DrcDataReader("oa_drc_libelle"), "")
                Me.DrcSexe = Coalesce(DrcDataReader("oa_drc_sexe"), 0)
                Me.DrcTypeEpisode = Coalesce(DrcDataReader("oa_drc_typ_epi"), "")
                Me.DrcAgeMin = Coalesce(DrcDataReader("oa_drc_age_min"), 0)
                Me.DrcAgeMax = Coalesce(DrcDataReader("oa_drc_age_max"), 0)
                Me.CategorieMajeure = Coalesce(DrcDataReader("oa_drc_categorie_majeure_id"), 0)
                Me.CategorieOasisId = Coalesce(DrcDataReader("oa_drc_oasis_categorie"), 0)
                Me.AldId = Coalesce(DrcDataReader("oa_drc_ald_id"), 0)
                Me.AldCode = Coalesce(DrcDataReader("oa_drc_ald_code"), "")
            Else
                MessageBox.Show("Erreur de lecture du Drc")
            End If

            conxn.Close()
            myCommand.Dispose()
        Else
            InitInstance()
        End If
    End Sub
    Private Sub InitInstance()
        Me.DrcId = 0
        Me.DrcLibelle = ""
        Me.DrcSexe = 0
        Me.DrcTypeEpisode = ""
        Me.DrcAgeMin = 0
        Me.DrcAgeMax = 0
        Me.CategorieMajeure = 0
        Me.CategorieOasisId = 0
        Me.AldId = 0
        Me.AldCode = ""
    End Sub

    Public Function Clone() As Drc
        Dim newInstance As Drc = DirectCast(Me.MemberwiseClone(), Drc)
        Return newInstance
    End Function

End Class


