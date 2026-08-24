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

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("oa_antecedent_id")
        Me.PatientId = record("oa_antecedent_patient_id")
        Me.Type = record("oa_antecedent_type")
        Me.DrcId = record("oa_antecedent_drc_id")
        Me.Description = Coalesce(record("oa_antecedent_description"), Nothing)
        Me.DateCreation = Coalesce(record("oa_antecedent_date_creation"), Nothing)
        Me.UserCreation = Coalesce(record("oa_antecedent_utilisateur_creation"), Nothing)
        Me.DateModification = Coalesce(record("oa_antecedent_date_modification"), Nothing)
        Me.UserModification = Coalesce(record("oa_antecedent_utilisateur_modification"), Nothing)
        Me.Diagnostic = Coalesce(record("oa_antecedent_diagnostic"), Nothing)
        Me.DateDebut = Coalesce(record("oa_antecedent_date_debut"), Nothing)
        Me.DateFin = Coalesce(record("oa_antecedent_date_fin"), Nothing)
        Me.AldId = Coalesce(record("oa_antecedent_ald_id"), Nothing)
        Me.AldCim10Id = Coalesce(record("oa_antecedent_ald_cim_10_id"), Nothing)
        Me.AldValide = Coalesce(record("oa_antecedent_ald_valide"), Nothing)
        Me.AldDateDebut = Coalesce(record("oa_antecedent_ald_date_debut"), Nothing)
        Me.AldDateFin = Coalesce(record("oa_antecedent_ald_date_fin"), Nothing)
        Me.AldDemandeEnCours = Coalesce(record("oa_antecedent_ald_demande_en_cours"), Nothing)
        Me.AldDateDemande = Coalesce(record("oa_antecedent_ald_demande_date"), Nothing)
        Me.Arret = Coalesce(record("oa_antecedent_arret"), Nothing)
        Me.ArretCommentaire = Coalesce(record("oa_antecedent_arret_commentaire"), Nothing)
        Me.Nature = Coalesce(record("oa_antecedent_nature"), Nothing)
        Me.Priorite = Coalesce(record("oa_antecedent_priorite"), Nothing)
        Me.Niveau = Coalesce(record("oa_antecedent_niveau"), Nothing)
        Me.Niveau1Id = Coalesce(record("oa_antecedent_id_niveau1"), Nothing)
        Me.Niveau2Id = Coalesce(record("oa_antecedent_id_niveau2"), Nothing)
        Me.Ordre1 = Coalesce(record("oa_antecedent_ordre_affichage1"), Nothing)
        Me.Ordre2 = Coalesce(record("oa_antecedent_ordre_affichage2"), Nothing)
        Me.Ordre3 = Coalesce(record("oa_antecedent_ordre_affichage3"), Nothing)
        Me.StatutAffichage = Coalesce(record("oa_antecedent_statut_affichage"), Nothing)
        Me.StatutAffichageTransformation = Coalesce(record("oa_antecedent_statut_affichage_transformation"), Nothing)
        Me.CategorieContexte = Coalesce(record("oa_antecedent_categorie_contexte"), Nothing)
        Me.EpisodeId = Coalesce(record("oa_episode_id"), Nothing)
        Me.Inactif = Coalesce(record("oa_antecedent_inactif"), Nothing)
        Me.ChaineEpisodeDateFin = Coalesce(record("oa_chaine_episode_date_fin"), Nothing)
    End Sub

    Public Function isChaineEpisodeEnable() As Boolean
        Return If(Me.ChaineEpisodeDateFin > Date.Now(), True, False)
    End Function

End Class
