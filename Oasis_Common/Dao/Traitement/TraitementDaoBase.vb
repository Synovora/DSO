Imports System.Data.SqlClient

Public Class TraitementDaoBase
    Inherits StandardDao

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

    Public Function GetBaseCodeByItem(Item As String) As String
        Dim Code As String
        Select Case Item
            Case EnumBaseItem.JOURNALIER
                Code = EnumBaseCode.JOURNALIER
            Case EnumBaseItem.HEBDOMADAIRE
                Code = EnumBaseCode.HEBDOMADAIRE
            Case EnumBaseItem.MENSUEL
                Code = EnumBaseCode.MENSUEL
            Case EnumBaseItem.ANNUEL
                Code = EnumBaseCode.ANNUEL
            Case EnumBaseItem.CONDITIONNEL
                Code = EnumBaseCode.CONDITIONNEL
            Case Else
                Code = ""
        End Select

        Return Code
    End Function

    Public Function GetBseItemByCode(Code As String) As String
        Dim Item As String
        Select Case Code
            Case EnumBaseCode.JOURNALIER
                Item = EnumBaseItem.JOURNALIER
            Case EnumBaseCode.HEBDOMADAIRE
                Item = EnumBaseItem.HEBDOMADAIRE
            Case EnumBaseCode.MENSUEL
                Item = EnumBaseItem.MENSUEL
            Case EnumBaseCode.ANNUEL
                Item = EnumBaseItem.ANNUEL
            Case EnumBaseCode.CONDITIONNEL
                Item = EnumBaseItem.CONDITIONNEL
            Case Else
                Item = ""
        End Select

        Return Item
    End Function

    Public Function GetTraitementById(traitementId As Integer) As TraitementBase
        Dim traitement As TraitementBase
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_traitement where oa_traitement_id = @id"
            command.Parameters.AddWithValue("@id", traitementId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    traitement = BuildBean(reader)
                Else
                    Throw New ArgumentException("Traitement inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return traitement
    End Function

    Private Function BuildBean(reader As SqlDataReader) As TraitementBase
        Dim traitement As New TraitementBase With {
            .TraitementId = reader("oa_traitement_id"),
            .PatientId = Coalesce(reader("oa_traitement_patient_id"), ""),
            .MedicamentId = Coalesce(reader("oa_traitement_medicament_cis"), ""),
            .MedicamentDci = Coalesce(reader("oa_traitement_medicament_dci"), ""),
            .ClasseAtc = Coalesce(reader("oa_traitement_classe_atc"), ""),
            .DenominationLongue = Coalesce(reader("oa_traitement_denomination_longue"), ""),
            .UserCreation = Coalesce(reader("oa_traitement_identifiant_creation"), 0),
            .DateCreation = Coalesce(reader("oa_traitement_date_creation"), Nothing),
            .UserModification = Coalesce(reader("oa_traitement_identifiant_modification"), 0),
            .DateModification = Coalesce(reader("oa_traitement_date_modification"), Nothing),
            .DateDebut = Coalesce(reader("oa_traitement_date_debut"), Nothing),
            .DateFin = Coalesce(reader("oa_traitement_date_fin"), Nothing),
            .OrdreAffichage = Coalesce(reader("oa_traitement_ordre_affichage"), 0),
            .PosologieBase = Coalesce(reader("oa_traitement_posologie_base"), ""),
            .PosologieRythme = Coalesce(reader("oa_traitement_posologie_rythme"), 0),
            .PosologieMatin = Coalesce(reader("oa_traitement_posologie_matin"), 0),
            .PosologieMidi = Coalesce(reader("oa_traitement_posologie_midi"), 0),
            .PosologieApresMidi = Coalesce(reader("oa_traitement_posologie_apres_midi"), 0),
            .PosologieSoir = Coalesce(reader("oa_traitement_posologie_soir"), 0),
            .FractionMatin = Coalesce(reader("oa_traitement_fraction_matin"), ""),
            .FractionMidi = Coalesce(reader("oa_traitement_fraction_midi"), ""),
            .FractionApresMidi = Coalesce(reader("oa_traitement_fraction_apres_midi"), ""),
            .FractionSoir = Coalesce(reader("oa_traitement_fraction_soir"), ""),
            .PosologieCommentaire = Coalesce(reader("oa_traitement_posologie_commentaire"), ""),
            .Fenetre = Coalesce(reader("oa_traitement_fenetre"), False),
            .FenetreDateDebut = Coalesce(reader("oa_traitement_fenetre_date_debut"), Nothing),
            .FenetreDateFin = Coalesce(reader("oa_traitement_fenetre_date_fin"), Nothing),
            .FenetreCommentaire = Coalesce(reader("oa_traitement_fenetre_commentaire"), ""),
            .Commentaire = Coalesce(reader("oa_traitement_commentaire"), ""),
            .Arret = Coalesce(reader("oa_traitement_arret"), ""),
            .ArretCommentaire = Coalesce(reader("oa_traitement_arret_commentaire"), ""),
            .Allergie = Coalesce(reader("oa_traitement_allergie"), False),
            .ContreIndication = Coalesce(reader("oa_traitement_contre_indication"), False),
            .DeclaratifHorsTraitement = Coalesce(reader("oa_traitement_declaratif_hors_traitement"), False),
            .Annulation = Coalesce(reader("oa_traitement_annulation"), ""),
            .AnnulationCommentaire = Coalesce(reader("oa_traitement_annulation_commentaire"), "")
        }
        Return traitement
    End Function

    Public Function GetBaseDescription(base As String) As String
        Dim baseString As String
        Select Case base
            Case TraitementDaoBase.EnumBaseCode.CONDITIONNEL
                baseString = "Conditionnel : "
            Case TraitementDaoBase.EnumBaseCode.HEBDOMADAIRE
                baseString = "Hebdo : "
            Case TraitementDaoBase.EnumBaseCode.MENSUEL
                baseString = "Mensuel : "
            Case TraitementDaoBase.EnumBaseCode.ANNUEL
                baseString = "Annuel : "
            Case Else
                baseString = "Base inconnue ! "
        End Select

        Return baseString
    End Function

End Class
