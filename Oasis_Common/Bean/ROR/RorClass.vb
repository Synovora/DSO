Imports System.Data.SqlClient

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

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)

        Me.Id = reader("oa_ror_id")
        Me.SpecialiteId = Coalesce(reader("oa_ror_specialite_id"), 0)
        Me.Nom = Coalesce(reader("oa_ror_nom"), "")
        Me.Oasis = Coalesce(reader("oa_ror_oasis"), False)
        Me.Type = Coalesce(reader("oa_ror_type"), "")
        Me.StructureId = Coalesce(reader("oa_ror_structure_id"), 0)
        Me.StructureNom = Coalesce(reader("oa_ror_structure_nom"), "")
        Me.Adresse1 = Coalesce(reader("oa_ror_adresse1"), "")
        Me.Adresse2 = Coalesce(reader("oa_ror_adresse2"), "")
        Me.CodePostal = Coalesce(reader("oa_ror_code_postal"), "")
        Me.Ville = Coalesce(reader("oa_ror_ville"), "")
        Me.Code = Coalesce(reader("oa_ror_code"), "")
        Me.Telephone = Coalesce(reader("oa_ror_telephone"), "")
        Me.Email = Coalesce(reader("oa_ror_email"), "")
        Me.Commentaire = Coalesce(reader("oa_ror_commentaire"), "")
        Me.Rpps = Coalesce(reader("oa_ror_rpps"), 0)
        Me.Finess = Coalesce(reader("oa_ror_finess"), 0)
        Me.Adeli = Coalesce(reader("oa_ror_adeli"), 0)
        Me.Inactif = Coalesce(reader("oa_ror_inactif"), False)
        Me.UserCreation = Coalesce(reader("oa_ror_user_creation"), 0)
        Me.DateCreation = Coalesce(reader("oa_ror_date_creation"), Nothing)
        Me.UserModification = Coalesce(reader("oa_ror_user_modification"), 0)
        Me.DateModification = Coalesce(reader("oa_ror_date_modification"), Nothing)
        Me.ExtractionAnnuaire = Coalesce(reader("oa_ror_extraction_annuaire"), False)
        Me.IdentifiantNational = Coalesce(reader("oa_ror_identifiant_national_pp"), "")
        Me.IdentifiantStructure = Coalesce(reader("oa_ror_identifiant_technique_structure"), "")
        Me.CodeModeExercice_r23 = Coalesce(reader("oa_ror_code_nos_r23_mode_exercice"), "")
        Me.CodeProfessionSante_g15 = Coalesce(reader("oa_ror_code_nos_g15_profession_sante"), 0)
        Me.CodeTypeSavoirFaire_r04 = Coalesce(reader("oa_ror_code_nos_r04_type_savoir_faire"), "")
        Me.CodeSavoirFaire = Coalesce(reader("oa_ror_code_savoir_faire"), "")
        Me.CleReferenceAnnuaire = Coalesce(reader("oa_ror_cle_reference"), 0)
    End Sub

End Class
