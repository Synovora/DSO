Imports System.Data.SqlClient

Public Class Antecedent

    Public Structure EnumTypeAntecedentContexte
        Const ANTECEDENT = "A"
        Const CONTEXTE = "C"
    End Structure

    Public Structure EnumStatutAffichage
        Const PUBLIE = "P"
        Const CACHE = "C"
        Const OCCULTE = "O"
    End Structure

    Property Id As Integer
    Property PatientId As Integer
    Property Type As String
    Property DrcId As Integer
    Property Description As String
    Property DateCreation As Date
    Property UserCreation As Integer
    Property DateModification As Date
    Property UserModification As Integer
    Property Diagnostic As Integer
    Property DateDebut As DateTime
    Property DateFin As Date
    Property AldId As Integer
    Property AldCim10Id As Integer
    Property AldValide As Boolean
    Property AldDateDebut As Date
    Property AldDateFin As Date
    Property AldDemandeEnCours As Boolean
    Property AldDateDemande As Date
    Property Arret As Boolean
    Property ArretCommentaire As String
    Property Nature As String
    Property Priorite As String
    Property Niveau As Integer
    Property Niveau1Id As Integer
    Property Niveau2Id As Integer
    Property Ordre1 As Integer
    Property Ordre2 As Integer
    Property Ordre3 As Integer
    Property StatutAffichage As String
    Property StatutAffichageTransformation As String
    Property CategorieContexte As String
    Property EpisodeId As Long
    Property Inactif As Boolean
    Property ChaineEpisodeDateFin As Date

    Public Function Clone() As Antecedent
        Dim newInstance As Antecedent = DirectCast(Me.MemberwiseClone(), Antecedent)
        Return newInstance
    End Function

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("oa_antecedent_id")
        Me.PatientId = reader("oa_antecedent_patient_id")
        Me.Type = reader("oa_antecedent_type")
        Me.DrcId = reader("oa_antecedent_drc_id")
        Me.Description = Coalesce(reader("oa_antecedent_description"), Nothing)
        Me.DateCreation = Coalesce(reader("oa_antecedent_date_creation"), Nothing)
        Me.UserCreation = Coalesce(reader("oa_antecedent_utilisateur_creation"), Nothing)
        Me.DateModification = Coalesce(reader("oa_antecedent_date_modification"), Nothing)
        Me.UserModification = Coalesce(reader("oa_antecedent_utilisateur_modification"), Nothing)
        Me.Diagnostic = Coalesce(reader("oa_antecedent_diagnostic"), Nothing)
        Me.DateDebut = Coalesce(reader("oa_antecedent_date_debut"), Nothing)
        Me.DateFin = Coalesce(reader("oa_antecedent_date_fin"), Nothing)
        Me.AldId = Coalesce(reader("oa_antecedent_ald_id"), Nothing)
        Me.AldCim10Id = Coalesce(reader("oa_antecedent_ald_cim_10_id"), Nothing)
        Me.AldValide = Coalesce(reader("oa_antecedent_ald_valide"), Nothing)
        Me.AldDateDebut = Coalesce(reader("oa_antecedent_ald_date_debut"), Nothing)
        Me.AldDateFin = Coalesce(reader("oa_antecedent_ald_date_fin"), Nothing)
        Me.AldDemandeEnCours = Coalesce(reader("oa_antecedent_ald_demande_en_cours"), Nothing)
        Me.AldDateDemande = Coalesce(reader("oa_antecedent_ald_demande_date"), Nothing)
        Me.Arret = Coalesce(reader("oa_antecedent_arret"), Nothing)
        Me.ArretCommentaire = Coalesce(reader("oa_antecedent_arret_commentaire"), Nothing)
        Me.Nature = Coalesce(reader("oa_antecedent_nature"), Nothing)
        Me.Priorite = Coalesce(reader("oa_antecedent_priorite"), Nothing)
        Me.Niveau = Coalesce(reader("oa_antecedent_niveau"), Nothing)
        Me.Niveau1Id = Coalesce(reader("oa_antecedent_id_niveau1"), Nothing)
        Me.Niveau2Id = Coalesce(reader("oa_antecedent_id_niveau2"), Nothing)
        Me.Ordre1 = Coalesce(reader("oa_antecedent_ordre_affichage1"), Nothing)
        Me.Ordre2 = Coalesce(reader("oa_antecedent_ordre_affichage2"), Nothing)
        Me.Ordre3 = Coalesce(reader("oa_antecedent_ordre_affichage3"), Nothing)
        Me.StatutAffichage = Coalesce(reader("oa_antecedent_statut_affichage"), Nothing)
        Me.StatutAffichageTransformation = Coalesce(reader("oa_antecedent_statut_affichage_transformation"), Nothing)
        Me.CategorieContexte = Coalesce(reader("oa_antecedent_categorie_contexte"), Nothing)
        Me.EpisodeId = Coalesce(reader("oa_episode_id"), Nothing)
        Me.Inactif = Coalesce(reader("oa_antecedent_inactif"), Nothing)
        Me.ChaineEpisodeDateFin = Coalesce(reader("oa_chaine_episode_date_fin"), Nothing)
    End Sub

    Public Function isChaineEpisodeEnable() As Boolean
        Return If(Me.ChaineEpisodeDateFin > Date.Now(), True, False)
    End Function

End Class
