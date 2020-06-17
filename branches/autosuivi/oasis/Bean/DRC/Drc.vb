Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports Oasis_Common
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
    Private _commentaire As String
    Private _reponseCommentee As String
    Private _dateCreation As Date
    Private _userCreation As Long
    Private _dateModification As Date
    Private _userModification As Long
    Private _CodeCim As String
    Private _CodeCisp As String

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

    Public Property Commentaire As String
        Get
            Return _commentaire
        End Get
        Set(value As String)
            _commentaire = value
        End Set
    End Property

    Public Property DateCreation As Date
        Get
            Return _dateCreation
        End Get
        Set(value As Date)
            _dateCreation = value
        End Set
    End Property

    Public Property UserCreation As Long
        Get
            Return _userCreation
        End Get
        Set(value As Long)
            _userCreation = value
        End Set
    End Property

    Public Property DateModification As Date
        Get
            Return _dateModification
        End Get
        Set(value As Date)
            _dateModification = value
        End Set
    End Property

    Public Property UserModification As Long
        Get
            Return _userModification
        End Get
        Set(value As Long)
            _userModification = value
        End Set
    End Property

    Public Property CodeCim As String
        Get
            Return _CodeCim
        End Get
        Set(value As String)
            _CodeCim = value
        End Set
    End Property

    Public Property CodeCisp As String
        Get
            Return _CodeCisp
        End Get
        Set(value As String)
            _CodeCisp = value
        End Set
    End Property

    Public Property ReponseCommentee As String
        Get
            Return _reponseCommentee
        End Get
        Set(value As String)
            _reponseCommentee = value
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
                Me.CodeCim = Coalesce(DrcDataReader("oa_drc_code_cim_defaut"), "")
                Me.CodeCisp = Coalesce(DrcDataReader("oa_drc_code_cisp_defaut"), "")
                Me.AldId = Coalesce(DrcDataReader("oa_drc_ald_id"), 0)
                Me.AldCode = Coalesce(DrcDataReader("oa_drc_ald_code"), "")
                Me.Commentaire = Coalesce(DrcDataReader("oa_drc_dur_prob_epis"), "")
                Me.ReponseCommentee = Coalesce(DrcDataReader("oa_drc_typ_epi"), "")
                Me.DateCreation = Coalesce(DrcDataReader("oa_drc_date_creation"), Nothing)
                Me.UserCreation = Coalesce(DrcDataReader("oa_drc_utilisateur_creation"), 0)
                Me.DateModification = Coalesce(DrcDataReader("oa_drc_date_modification"), Nothing)
                Me.UserModification = Coalesce(DrcDataReader("oa_drc_utilisateur_modification"), 0)
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
        Me.CodeCim = ""
        Me.CodeCisp = ""
        Me.AldId = 0
        Me.AldCode = ""
        Me.Commentaire = ""
        Me.ReponseCommentee = ""
        Me.DateCreation = Nothing
        Me.UserCreation = 0
        Me.DateModification = Nothing
        Me.UserModification = 0
    End Sub

    Public Function Clone() As Drc
        Dim newInstance As Drc = DirectCast(Me.MemberwiseClone(), Drc)
        Return newInstance
    End Function

End Class


