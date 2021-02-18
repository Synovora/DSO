Imports System.Data.SqlClient

Public Class Drc
    Public Enum EnumGenreItem
        Homme = 1
        Femme = 2
        HommeEtFemme = 3
    End Enum

    Public Structure EnumGenre
        Const Homme = "Homme"
        Const Femme = "Femme"
        Const HommeEtFemme = "Homme et femme"
    End Structure

    Public Enum EnumCategorieOasisCode
        Contexte = 1
        Strategie = 2
        Prevention = 3
        Objectif = 4
        ActeParamedical = 5
        GroupeParametres = 6
        ProtocoleCollaboratif = 7
        ProtocoleAigu = 8
    End Enum

    Public Structure EnumCategorieOasisItem
        Const Contexte = "Contexte et antécédent"
        Const Strategie = "Stratégie"
        Const Prevention = "Prévention"
        Const Objectif = "Objectif"
        Const ActeParamedical = "Acte paramédical"
        Const GroupeParametres = "Groupe de paramètres"
        Const ProtocoleCollaboratif = "Procédure collaborative"
        Const ProtocoleAigu = "Procédure pathologie aigüe"
    End Structure

    Property DrcId As Integer
    Property DrcLibelle As String
    Property DrcSexe As Integer       '1:Masculin, 2:Féminin, 3:Les deux
    Property DrcTypeEpisode As String 'C:Chronique, A:Aigue, NC:Non concerné
    Property DrcAgeMin As Integer
    Property DrcAgeMax As Integer
    Property CategorieOasisId As Integer    '1:Contexte et antécédent, 2:Stratégie contextuelle, 3:Mesure préventive, 4:Objectif, 5:Acte paramédical
    Property CategorieMajeure As Integer
    Property AldId As Integer
    Property AldCode As String
    Property Commentaire As String
    Property Wiki As String
    Property ReponseCommentee As String
    Property DateCreation As Date
    Property UserCreation As Long
    Property DateModification As Date
    Property UserModification As Long
    Property CodeCim As String
    Property CodeCisp As String

    Sub New()
        InitInstance()
    End Sub

    Sub New(DrcId As Integer)
        If DrcId <> 0 Then
            Dim conxn As New SqlConnection(GetConnectionString())
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
                Me.Wiki = Coalesce(DrcDataReader("oa_drc_url"), "")
                Me.ReponseCommentee = Coalesce(DrcDataReader("oa_drc_typ_epi"), "")
                Me.DateCreation = Coalesce(DrcDataReader("oa_drc_date_creation"), Nothing)
                Me.UserCreation = Coalesce(DrcDataReader("oa_drc_utilisateur_creation"), 0)
                Me.DateModification = Coalesce(DrcDataReader("oa_drc_date_modification"), Nothing)
                Me.UserModification = Coalesce(DrcDataReader("oa_drc_utilisateur_modification"), 0)
            Else
                Throw New Exception("Erreur de lecture du Drc")
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
        Me.Wiki = ""
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


