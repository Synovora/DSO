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

    Public Function Clone() As Antecedent
        Dim newInstance As Antecedent = DirectCast(Me.MemberwiseClone(), Antecedent)
        Return newInstance
    End Function

End Class
