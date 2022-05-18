Public Class Tache

    Public Enum EtatTache
        EN_ATTENTE
        EN_COURS
        TERMINEE
        ANNULEE
    End Enum

    Public Enum EnumPriorite
        HAUTE = 100
        MOYENNE = 200
        BASSE = 300
    End Enum

    Public Enum CategorieTache
        SOIN
        LOGISTIQUE
        RH
    End Enum

    Public Enum TypeTache      ' - le chiffre sert dans l'agenda pour la couleur du RDV, ne pas changer SVP
        RDV_DEMANDE = 100
        RDV = 2
        MISSION_DEMANDE = 200
        RDV_MISSION = 4
        REUNION_STAFF = 3
        RDV_SPECIALISTE = 11
        AVIS_EPISODE = 300
        AVIS_SOUS_EPISODE = 400
    End Enum

    Public Enum PrioriteByTypeTache
        AVIS_EPISODE_URGENT = 100       'Correspond à priorité Haute
        AVIS_EPISODE_SYNCHRONE = 200    'correspond à priorité Moyenne
        RDV = 230
        RDV_MISSION = 260
        AVIS_EPISODE_ASYNCHRONE = 300   'Correspond à priorité Basse
        RDV_DEMANDE = 400
    End Enum

    Public Enum NatureTache
        DEMANDE                 '    nature des TypeTache AVIS (les 2)
        REPONSE                 '    nature des TypeTache AVIS (les 2)
        COMPLEMENT              '    nature de TypeTache AVIS_EPISODE
        RDV_DEMANDE             '   = TypeTache
        RDV                     '   = TypeTache
        MISSION_DEMANDE         '   = TypeTache
        REUNION_STAFF           '   = TypeTache
        RDV_SPECIALISTE         '   = TypeTache
    End Enum

    Public Structure EnumNatureTacheItem
        Const DEMANDE = "Demande d'avis"
        Const REPONSE = "Réponse à demande d'avis"
        Const COMPLEMENT = "Demande de complément d'information"
        Const RDV_DEMANDE = "Demande de rendez-vous"
        Const RDV = "Rendez-vous Oasis"
        Const RDV_SPECIALISTE = "Rendez-vous spécialiste"
        Const MISSION_DEMANDE = "Rendez-vous mission"
        Const REUNION_STAFF = "Réunion staff"
    End Structure

    Public Structure EnumNatureTacheCode
        Const DEMANDE = "DEMANDE"
        Const REPONSE = "REPONSE"
        Const COMPLEMENT = "COMPLEMENT"
        Const RDV_DEMANDE = "RDV_DEMANDE"
        Const RDV = "RDV"
        Const RDV_SPECIALISTE = "RDV_SPECIALISTE"
        Const MISSION_DEMANDE = "MISSION_DEMANDE"
        Const REUNION_STAFF = "REUNION_STAFF"
    End Structure

    Public Enum EnumDemandeRendezVous
        ANNEEMOIS
        ANNEE
    End Enum

    Public Enum EnumOptionWorkflow
        REPONSE_AVIS
        DEMANDE_PRECISION
        VALIDATION_AVIS
        RELANCE_AVIS
        NULL
    End Enum

    Property Id As Long
    Property ParentId As Long
    Property EmetteurUserId As Long
    Property EmetteurFonctionId As Long
    Property UniteSanitaireId As Long
    Property SiteId As Long
    Property PatientId As Long
    Property ParcoursId As Long
    Property EpisodeId As Long
    Property SousEpisodeId As Long
    Property TraiteUserId As Long
    Property TraiteFonctionId As Long
    Property DestinataireFonctionId As Long
    Property Priorite As Integer
    Property OrdreAffichage As Integer
    Property Categorie As String
    Property Type As String
    Property Nature As String
    Property Duree As Integer
    Property EmetteurCommentaire As String
    Property HorodatageCreation As DateTime
    Property HorodatageAttribution As DateTime
    Property HorodatageCloture As DateTime
    Property Etat As String
    Property Cloture As Boolean
    Property TypedemandeRendezVous As String
    Property DateTraitementDemandeRendezVous As DateTime
    Property DateRendezVous As Date

    Public Shared Function GetTypeTacheIndex(inputString As String) As Integer
        Dim a As TypeTache
        Try
            a = TypeTache.Parse(GetType(TypeTache), inputString)
        Catch
            Return 0
        End Try
        Return CInt(a)
    End Function

    Public Function GetLibelleTacheNature() As String
        Return Tache.getLibelleTacheNature(Me.Type, Me.Nature)
    End Function

    Public Shared Function getLibelleTacheNature(type As String, nature As String) As String
        Dim libel As String = ""
        Select Case type
            Case TypeTache.RDV_DEMANDE.ToString
                libel = "Demande de rendez-vous"
            Case TypeTache.RDV.ToString
                libel = "Rendez-vous"
            Case TypeTache.AVIS_EPISODE.ToString
                libel = StrConv(nature, VbStrConv.ProperCase) + " d'avis sur épisode"
            Case TypeTache.AVIS_SOUS_EPISODE.ToString
                libel = StrConv(nature, VbStrConv.ProperCase) + " d'avis sur sous-épisode"
            Case TypeTache.MISSION_DEMANDE.ToString
                libel = "Demande de Rendez-vous mission"
            Case TypeTache.RDV_MISSION.ToString
                libel = "Rendez-vous mission"
            Case TypeTache.REUNION_STAFF.ToString
                libel = "Réunion Staff"
            Case TypeTache.RDV_SPECIALISTE.ToString
                libel = "Rendez-vous hors Oasis"
        End Select

        Return libel
    End Function

    Public Function IsAttribue() As Boolean
        Return (Coalesce(TraiteUserId, 0) <> 0)
    End Function

    Public Function IsMyTacheEmetteur(userLog) As Boolean
        Return (userLog.UtilisateurId = EmetteurUserId)
    End Function

    Public Function IsMyTacheATraiter(userLog) As Boolean
        Return (userLog.UtilisateurId = TraiteUserId)
    End Function

    Public Function IsAnnulable(userLog) As Boolean
        Return ((IsMyTacheEmetteur(userLog) AndAlso IsAttribue() = False) OrElse (IsMyTacheATraiter(userLog))) AndAlso IsStatutFinal() = False
    End Function

    Public Function IsRendezVousAFixer(userLog) As Boolean
        Return (IsMyTacheATraiter(userLog) _
                AndAlso Etat = EtatTache.EN_COURS.ToString _
                AndAlso (Type = TypeTache.RDV_DEMANDE.ToString OrElse Type = TypeTache.MISSION_DEMANDE.ToString)
               )
    End Function

    Public Function IsAttribuable(userLog) As Boolean
        Return IsAttribue() = False AndAlso userLog.IsFonctionIdPossible(TraiteFonctionId) AndAlso IsStatutFinal() = False
    End Function

    Public Function IsDesattribuable(userLog) As Boolean
        Return IsAttribue() _
            AndAlso (IsMyTacheATraiter(userLog) OrElse userLog.UtilisateurAdmin) _
            AndAlso (IsStatutFinal() = False)
    End Function

    Public Function IsStatutFinal() As Boolean
        Return Etat = EtatTache.ANNULEE.ToString OrElse Etat = EtatTache.TERMINEE.ToString
    End Function

    Public Function IsUnRdv() As Boolean
        Return Type = TypeTache.RDV.ToString OrElse Type = TypeTache.RDV_MISSION.ToString OrElse Type = TypeTache.RDV_SPECIALISTE.ToString OrElse Type = TypeTache.REUNION_STAFF.ToString
    End Function

    Public Function GetTypeRdvFromDemande() As TypeTache
        Select Case Type
            Case TypeTache.RDV_DEMANDE.ToString
                Return TypeTache.RDV
            Case TypeTache.MISSION_DEMANDE.ToString
                Return TypeTache.RDV_MISSION
        End Select
        Return Nothing
    End Function


    Public Function IsFonctionPossiblePourUser(user As Utilisateur) As Boolean
        Return user.IsFonctionIdPossible(TraiteFonctionId)
    End Function

End Class
