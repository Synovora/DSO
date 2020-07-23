Public Class Traitement

    Public Enum EnumMonographie
        CLASSIQUE = 0
        VIRTUEL = 1
    End Enum

    Public Structure EnumBaseCode
        Const JOURNALIER = "J"
        Const HEBDOMADAIRE = "H"
        Const MENSUEL = "M"
        Const ANNUEL = "A"
        Const CONDITIONNEL = "C"
    End Structure

    Public Structure EnumBaseItem
        Const JOURNALIER = "Journalier"
        Const HEBDOMADAIRE = "Hebdomadaire"
        Const MENSUEL = "Mensuel"
        Const ANNUEL = "Annuel"
        Const CONDITIONNEL = "Conditionnel"
    End Structure

    Public Structure EnumFraction
        Const Non = "0"
        Const Quart = "1/4"
        Const Demi = "1/2"
        Const TroisQuart = "3/4"
    End Structure

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

    Public Function Clone() As Traitement
        Dim newInstance As Traitement = DirectCast(Me.MemberwiseClone(), Traitement)
        Return newInstance
    End Function

End Class


