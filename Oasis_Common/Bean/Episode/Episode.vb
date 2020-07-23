Public Class Episode

    Public Enum EnumTypeConclusionParamedicale
        ROLE_PROPRE = 0
        SUR_PROTOCOLE = 1
        DEMANDE_AVIS = 2
    End Enum

    Public Structure EnumTypeActiviteEpisodeItem
        Const PATHOLOGIE_AIGUE = "Pathologie Aiguë"
        Const PREVENTION_AUTRE = "Autre prévention"
        Const PREVENTION_ENFANT_PRE_SCOLAIRE = "Prévention de l'enfant en âge pré-scolaire (0 à 40 mois)"
        Const PREVENTION_ENFANT_SCOLAIRE = "Prévention de l'enfant en âge scolaire (à partir de 3 ans)"
        Const PREVENTION_SUIVI_GROSSESSE = "Suivi grossesse"
        Const PREVENTION_SUIVI_GYNECOLOGIQUE = "Suivi gynécologique"
        Const SOCIAL = "Social"
        Const SUIVI_CHRONIQUE = "Suivi chronique"
    End Structure

    Public Structure EnumTypeActiviteEpisodeCode
        Const PATHOLOGIE_AIGUE = "PATHOLOGIE_AIGUE"
        Const PREVENTION_AUTRE = "PREVENTION_AUTRE"
        Const PREVENTION_ENFANT_PRE_SCOLAIRE = "PREVENTION_ENFANT_PRE_SCOLAIRE"
        Const PREVENTION_ENFANT_SCOLAIRE = "PREVENTION_ENFANT_SCOLAIRE"
        Const PREVENTION_SUIVI_GROSSESSE = "PREVENTION_SUIVI_GROSSESSE"
        Const PREVENTION_SUIVI_GYNECOLOGIQUE = "PREVENTION_SUIVI_GYNECOLOGIQUE"
        Const SOCIAL = "SOCIAL"
        Const SUIVI_CHRONIQUE = "SUIVI_CHRONIQUE"
    End Structure

    Public Enum EnumEtatEpisode
        EN_COURS = 0
        CLOTURE = 1
        ANNULE = 2
    End Enum

    Public Enum EnumTypeEpisode
        CONSULTATION = 0
        VIRTUEL = 1
        PARAMETRE = 2
    End Enum

    Property Id As Long
    Property PatientId As Long
    Property Type As String         'C: Consultation, V: Virteul
    Property TypeActivite As String 'Suivi pathologie chronique, pathologie aigue, prévention protection infantile, prévention suivi gynécologique, prévention suivi grossesse, prévention autre, social
    Property DescriptionActivite As String
    Property TypeProfil As String 'MEDICAL, PARAMEDICAL
    Property Commentaire As String
    Property ObservationMedical As String
    Property ObservationParamedical As String
    Property ConclusionIdeType As String
    Property ConclusionMedConsigneDrcId As Long
    Property ConclusionMedConsigneDenomination As String
    Property ConclusionMedContexte1DrcId As Long
    Property ConclusionMedContexte1AntecedentId As Long
    Property ConclusionMedContexte2DrcId As Long
    Property ConclusionMedContexte2AntecedentId As Long
    Property ConclusionMedContexte3DrcId As Long
    Property ConclusionMedContexte3AntecedentId As Long
    Property Decision As String
    Property UserCreation As Long
    Property DateCreation As Date
    Property UserModification As Long
    Property DateModification As Date
    Property Etat As String         'En cours, En attente, Cloturé
    Property Inactif As Boolean
End Class
