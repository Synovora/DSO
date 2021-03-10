Public Class Ror

    Public Structure EnumIntervenant
        Const Intervenants = "Intervenant"
        Const Structures = "Structure"
    End Structure

    Property Id As Long
    Property SpecialiteId As Long
    Property Nom As String
    Property Oasis As Boolean
    Property Type As String
    Property StructureId As Long
    Property StructureNom As String
    Property Adresse1 As String
    Property Adresse2 As String
    Property CodePostal As String
    Property Ville As String
    Property Code As String
    Property Telephone As String
    Property Email As String
    Property Commentaire As String
    Property Rpps As Long
    Property Finess As Long
    Property Adeli As Long
    Property Inactif As Boolean
    Property UserCreation As Long
    Property DateCreation As Date
    Property UserModification As Long
    Property DateModification As Date
    Property ExtractionAnnuaire As Boolean
    Property IdentifiantNational As String
    Property IdentifiantStructure As String
    Property CodeModeExercice_r23 As String
    Property CodeProfessionSante_g15 As Integer
    Property CodeTypeSavoirFaire_r04 As String
    Property CodeSavoirFaire As String
    Property CleReferenceAnnuaire As Long


    Public Function Clone() As Ror
        Dim newInstance As Ror = DirectCast(Me.MemberwiseClone(), Ror)
        Return newInstance
    End Function

End Class
