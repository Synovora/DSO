Public Class TraitementBase
    Property TraitementId As Integer
    Property PatientId As Integer
    Property MedicamentId As Integer
    Property MedicamentMonographie As Boolean
    Property MedicamentDci As String
    Property DenominationLongue As String
    Property ClasseAtc As String
    Property UserCreation As Integer
    Property DateCreation As Date
    Property UserModification As Integer
    Property DateModification As Date
    Property DateDebut As Date
    Property DateFin As Date
    Property OrdreAffichage As Integer
    Property PosologieBase As String
    Property PosologieRythme As Integer
    Property PosologieMatin As Integer
    Property PosologieMidi As Integer
    Property PosologieApresMidi As Integer
    Property PosologieSoir As Integer
    Property FractionMatin As String
    Property FractionMidi As String
    Property FractionApresMidi As String
    Property FractionSoir As String
    Property PosologieCommentaire As String
    Property Fenetre As Boolean
    Property FenetreDateDebut As Date
    Property FenetreDateFin As Date
    Property FenetreCommentaire As String
    Property Commentaire As String
    Property Arret As String
    Property ArretCommentaire As String
    Property Allergie As Boolean
    Property DeclaratifHorsTraitement As Boolean
    Property ContreIndication As Boolean
    Property Annulation As String
    Property AnnulationCommentaire As String

    Public Function Clone() As TraitementBase
        Dim newInstance As TraitementBase = DirectCast(Me.MemberwiseClone(), TraitementBase)
        Return newInstance
    End Function

End Class


