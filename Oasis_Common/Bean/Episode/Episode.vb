Public Class Episode
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
