Imports System.Data.SqlClient
Imports Oasis_WF

Public Class TacheDao
    Inherits StandardDao

    Public Enum EtatTache
        EN_ATTENTE
        EN_COURS
        TERMINEE
        ANNULEE
    End Enum

    Public Enum Priorite
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

    Public Enum typeDemandeRendezVous
        ANNEEMOIS
        ANNEE
    End Enum

    Friend Function getCodeNatureTacheByItem(item As String) As String
        Dim CodeNatureTache As String

        Select Case item
            Case EnumNatureTacheItem.DEMANDE
                CodeNatureTache = EnumNatureTacheCode.DEMANDE
            Case EnumNatureTacheItem.COMPLEMENT
                CodeNatureTache = EnumNatureTacheCode.COMPLEMENT
            Case EnumNatureTacheItem.MISSION_DEMANDE
                CodeNatureTache = EnumNatureTacheCode.MISSION_DEMANDE
            Case EnumNatureTacheItem.RDV
                CodeNatureTache = EnumNatureTacheCode.RDV
            Case EnumNatureTacheItem.RDV_DEMANDE
                CodeNatureTache = EnumNatureTacheCode.RDV_DEMANDE
            Case EnumNatureTacheItem.RDV_SPECIALISTE
                CodeNatureTache = EnumNatureTacheCode.RDV_SPECIALISTE
            Case EnumNatureTacheItem.REPONSE
                CodeNatureTache = EnumNatureTacheCode.REPONSE
            Case EnumNatureTacheItem.REUNION_STAFF
                CodeNatureTache = EnumNatureTacheCode.REUNION_STAFF
            Case Else
                CodeNatureTache = "Inconnue"
        End Select

        Return CodeNatureTache
    End Function

    Friend Function getItemNatureTacheByCode(code As String) As String
        Dim ItemNatureTache As String

        Select Case code
            Case EnumNatureTacheCode.DEMANDE
                ItemNatureTache = EnumNatureTacheItem.DEMANDE
            Case EnumNatureTacheCode.COMPLEMENT
                ItemNatureTache = EnumNatureTacheItem.COMPLEMENT
            Case EnumNatureTacheCode.MISSION_DEMANDE
                ItemNatureTache = EnumNatureTacheItem.MISSION_DEMANDE
            Case EnumNatureTacheCode.RDV
                ItemNatureTache = EnumNatureTacheItem.RDV
            Case EnumNatureTacheCode.RDV_DEMANDE
                ItemNatureTache = EnumNatureTacheItem.RDV_DEMANDE
            Case EnumNatureTacheCode.RDV_SPECIALISTE
                ItemNatureTache = EnumNatureTacheItem.RDV_SPECIALISTE
            Case EnumNatureTacheCode.REPONSE
                ItemNatureTache = EnumNatureTacheItem.REPONSE
            Case EnumNatureTacheCode.REUNION_STAFF
                ItemNatureTache = EnumNatureTacheItem.REUNION_STAFF
            Case Else
                ItemNatureTache = "Inconnue"
        End Select

        Return ItemNatureTache
    End Function

    Public Shared Function getTypeTacheIndex(inputString As String) As Integer
        Dim a As TypeTache
        Try
            a = TypeTache.Parse(GetType(TypeTache), inputString)
        Catch
            Return 0
        End Try
        Return CInt(a)
    End Function

    Public Function getAllTacheATraiterChk(lstFonction As List(Of Fonction), filtreTache As FiltreTache) As Integer
        Dim SQLString As String

        'Console.WriteLine("         .. Appel allTacheAtraiter CHK")
        SQLString =
            "SELECT CHECKSUM_AGG(cast(id as int)  ) as CHK " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "WHERE etat = @etat " & vbCrLf

        ' --- ajout des filtre
        SQLString += addFiltreAllTacheATraiter(lstFonction, filtreTache)

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", EtatTache.EN_ATTENTE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return Coalesce(tacheDataTable.Rows(0)("CHK"), 0)
                End Using
            End Using
        End Using

    End Function

    ''' <summary>
    ''' Liste des tache a traiter 
    ''' Attention : au moins une fonction dans la liste des fonction sinon pas de resultat
    ''' </summary>
    ''' <param name="lstFonction"></param>
    ''' <param name="filtreTache"></param>
    ''' <returns></returns>
    Public Function getAllTacheATraiter(lstFonction As List(Of Fonction), filtreTache As FiltreTache) As DataTable
        Dim SQLString As String
        'Console.WriteLine("         .. Appel allTacheAtraiter data")

        SQLString =
            "SELECT " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     S.oa_site_description as site_description, " & vbCrLf &
            "     P.oa_patient_nom as patient_nom, " & vbCrLf &
            "     P.oa_patient_prenom as patient_prenom, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
            "     T.emetteur_commentaire, " & vbCrLf &
            "     T.horodate_creation, " & vbCrLf &
            "     T.priorite, " & vbCrLf &
            "	  T.ordre_affichage " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE etat = @etat " & vbCrLf

        ' --- ajout des filtre
        SQLString += addFiltreAllTacheATraiter(lstFonction, filtreTache)

        SQLString += "ORDER BY priorite,ordre_affichage,horodate_creation "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", EtatTache.EN_ATTENTE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Private Function addFiltreAllTacheATraiter(lstFonction As List(Of Fonction), filtreTache As FiltreTache) As String
        Dim SQLString As String = ""
        ' -- filtre fonctions
        If (lstFonction.Count < 1) Then
            SQLString += "AND 0=1 " & vbCrLf  ' si pas de fonction => pas de resultats (sécurité)
        Else
            SQLString += "AND traite_fonction_id " & Fonction.getQueryInForIds(lstFonction) & vbCrLf
        End If
        ' --- filtre unités sanitaire
        If filtreTache.LstUniteSanitaire.Count > 0 Then
            SQLString += "AND unite_sanitaire_id " & UniteSanitaire.getQueryInForIds(filtreTache.LstUniteSanitaire) & vbCrLf
        End If
        ' --- filtre sites
        Dim lstAllSite As List(Of Site)
        lstAllSite = filtreTache.getListAllSite()
        If lstAllSite.Count > 0 Then
            SQLString += "AND site_id " & Site.getQueryInForIds(lstAllSite) & vbCrLf
        End If
        Return SQLString
    End Function

    ''' <summary>
    ''' donne les tache en cours pour un user ou pour les fonctions du profil d'un user
    ''' </summary>
    ''' <param name="isMyTache"></param>
    ''' <returns></returns>
    Friend Function getAllTacheEnCours(isMyTache As Boolean, lstFonctionChoisie As List(Of Fonction), filtre As FiltreTache) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getAllTacheEnCours")
        SQLString =
            "SELECT " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     S.oa_site_description as site_description, " & vbCrLf &
            "     P.oa_patient_nom as patient_nom, " & vbCrLf &
            "     P.oa_patient_prenom as patient_prenom, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
            "     T.emetteur_commentaire, " & vbCrLf &
            "     T.horodate_creation, " & vbCrLf &
            "     T.priorite, " & vbCrLf &
            "	  T.ordre_affichage, " & vbCrLf &
            "	  T.date_rendez_vous " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE etat = @etat" & vbCrLf

        ' --- condition si pour moi ou pour toutes les fonctions de mon profil
        If isMyTache Then
            SQLString += "AND traite_user_id = @traite_user_id" & vbCrLf
        Else
            If IsNothing(lstFonctionChoisie) OrElse lstFonctionChoisie.Count = 0 OrElse IsNothing(filtre) Then
                SQLString += "AND 0=1 " & vbCrLf  ' pas de fonction ou pas de filtre => pas de tache
            Else
                ' --- filtre fonction
                SQLString += "AND traite_fonction_id " & Fonction.getQueryInForIds(lstFonctionChoisie) & vbCrLf
                ' --- filtre unités sanitaire
                If filtre.LstUniteSanitaire.Count > 0 Then
                    SQLString += "AND unite_sanitaire_id " & UniteSanitaire.getQueryInForIds(filtre.LstUniteSanitaire) & vbCrLf
                End If
                ' --- filtre sites
                If filtre.getListAllSite().Count > 0 Then
                    SQLString += "AND site_id " & Site.getQueryInForIds(filtre.getListAllSite()) & vbCrLf
                End If
            End If
        End If

        SQLString += "ORDER BY priorite,ordre_affichage,horodate_creation "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", EtatTache.EN_COURS.ToString)
                If isMyTache Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@traite_user_id", userLog.UtilisateurId)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function


    Friend Function getAgendaMyRDV(dateDebut As Date, dateFin As Date, isMyTache As Boolean, lstFonctionChoisie As List(Of Fonction), filtre As FiltreTache) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     S.oa_site_description as site_description, " & vbCrLf &
            "     P.oa_patient_nom as patient_nom, " & vbCrLf &
            "     P.oa_patient_prenom as patient_prenom, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
            "     T.emetteur_commentaire, " & vbCrLf &
            "     T.horodate_creation, " & vbCrLf &
            "     T.priorite, " & vbCrLf &
            "	  T.ordre_affichage, " & vbCrLf &
            "	  T.date_rendez_vous, " & vbCrLf &
            "	  T.duree_mn " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE etat = @etat" & vbCrLf &
            "AND [type] IN (@typeTache1 , @typeTache2, @typeTache3, @typeTache4) " & vbCrLf &
            "AND date_rendez_vous BETWEEN @datedebut and @datefin " & vbCrLf

        ' --- condition si pour moi ou pour toutes les fonctions de mon profil
        If isMyTache Then
            SQLString += "AND traite_user_id = @traite_user_id" & vbCrLf
        Else
            If IsNothing(lstFonctionChoisie) OrElse lstFonctionChoisie.Count = 0 OrElse IsNothing(filtre) Then
                SQLString += "AND 0=1 " & vbCrLf  ' pas de fonction ou pas de filtre => pas de tache
            Else
                ' --- filtre fonction
                SQLString += "AND traite_fonction_id " & Fonction.getQueryInForIds(lstFonctionChoisie) & vbCrLf
                ' --- filtre unités sanitaire
                If filtre.LstUniteSanitaire.Count > 0 Then
                    SQLString += "AND unite_sanitaire_id " & UniteSanitaire.getQueryInForIds(filtre.LstUniteSanitaire) & vbCrLf
                End If
                ' --- filtre sites
                If filtre.getListAllSite().Count > 0 Then
                    SQLString += "AND site_id " & Site.getQueryInForIds(filtre.getListAllSite()) & vbCrLf
                End If
            End If
        End If


        ' -- filtre fonctions

        SQLString += "ORDER BY date_rendez_vous "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@traite_user_id", userLog.UtilisateurId)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", EtatTache.EN_COURS.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache1", TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache2", TypeTache.RDV_MISSION.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache3", TypeTache.REUNION_STAFF.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache4", TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@datedebut", dateDebut)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@datefin", dateFin.AddDays(1))   ' <= jour suivant à minuit
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' tache emises (max 1000)
    ''' </summary>
    ''' <param name="isNotFinal"></param>
    ''' <returns></returns>
    Friend Function getAllTacheEmise(isNotFinal As Boolean) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT TOP 1000 " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     S.oa_site_description as site_description, " & vbCrLf &
            "     P.oa_patient_nom as patient_nom, " & vbCrLf &
            "     P.oa_patient_prenom as patient_prenom, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
             "	  T.date_rendez_vous, " & vbCrLf &
           "	  coalesce(U.oa_utilisateur_nom,'') as user_traiteur_nom, " & vbCrLf &
            "	  coalesce(U.oa_utilisateur_prenom,'') as user_traiteur_prenom, " & vbCrLf &
            "	  coalesce(F.oa_r_fonction_designation,'') as user_traiteur_fonction " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_utilisateur U ON U.oa_utilisateur_id = T.traite_user_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_r_fonction F ON F.oa_r_fonction_id = T.traite_fonction_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE [emetteur_user_id] = @emetteur_user_id " & vbCrLf

        ' -- filtre Not Final
        If isNotFinal Then
            SQLString += "AND etat NOT IN ('" + TacheDao.EtatTache.ANNULEE.ToString + "','" + TacheDao.EtatTache.TERMINEE.ToString + "')" + vbCrLf
        End If

        SQLString += "ORDER BY priorite,ordre_affichage,horodate_creation "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@emetteur_user_id", userLog.UtilisateurId)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function GetWorkflowHistoByEpisode(episodeId As Long) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.emetteur_user_id, " & vbCrLf &
            "	  T.emetteur_fonction_id, " & vbCrLf &
            "	  T.emetteur_commentaire, " & vbCrLf &
            "	  T.destinataire_fonction_id, " & vbCrLf &
            "	  T.traite_user_id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
            "     T.horodate_creation, " & vbCrLf &
            "     T.horodate_cloture, " & vbCrLf &
            "     T.cloture, " & vbCrLf &
            "	  COALESCE(UE.oa_utilisateur_nom,'') as user_emetteur_nom, " & vbCrLf &
            "	  COALESCE(UE.oa_utilisateur_prenom,'') as user_emetteur_prenom, " & vbCrLf &
            "	  COALESCE(UE.oa_utilisateur_profil_id,'') as user_emetteur_profil, " & vbCrLf &
            "	  COALESCE(UT.oa_utilisateur_nom,'') as user_traite_nom, " & vbCrLf &
            "	  COALESCE(UT.oa_utilisateur_prenom,'') as user_traite_prenom, " & vbCrLf &
            "	  COALESCE(UT.oa_utilisateur_profil_id,'') as user_traite_profil, " & vbCrLf &
            "	  COALESCE(F.oa_r_fonction_designation,'') as user_destinataire_fonction " & vbCrLf &
            "FROM oasis.oa_tache T " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_utilisateur UE ON UE.oa_utilisateur_id = T.emetteur_user_id " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_utilisateur UT ON UT.oa_utilisateur_id = T.traite_user_id " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_r_fonction F ON F.oa_r_fonction_id = T.destinataire_fonction_id " & vbCrLf &
            "WHERE T.episode_id = @episodeId " & vbCrLf &
            "AND T.type = @type " & vbCrLf &
            "AND T.categorie = @categorie " & vbCrLf &
            "AND T.etat <> @etat " & vbCrLf

        ' -- filtre fonctions

        SQLString += "ORDER BY T.id "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@episodeId", episodeId)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type", TacheDao.TypeTache.AVIS_EPISODE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@categorie", TacheDao.CategorieTache.SOIN.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", TacheDao.EtatTache.ANNULEE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function


    Friend Function GetRDVByPatient(patientId As Long) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.parcours_id, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
            "	  T.traite_user_id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     T.date_rendez_vous, " & vbCrLf &
            "     T.type_demande_rendez_vous, " & vbCrLf &
            "     T.horodate_cloture, " & vbCrLf &
            "     T.cloture, " & vbCrLf &
            "	  COALESCE(PA.oa_parcours_ror_id,0) as ror_id, " & vbCrLf &
            "	  COALESCE(RO.oa_ror_nom,'') as oa_ror_nom, " & vbCrLf &
            "     COALESCE(RO.oa_ror_specialite_id,0) as oa_ror_specialite_id, " & vbCrLf &
            "	  COALESCE(RO.oa_ror_structure_nom,'') as oa_ror_structure_nom " & vbCrLf &
            "FROM oasis.oa_tache T " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_patient_parcours PA ON PA.oa_parcours_id = T.parcours_id " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_ror RO ON RO.oa_ror_id = PA.oa_parcours_ror_id " & vbCrLf &
            "WHERE T.patient_id = @patientId " & vbCrLf &
            "AND (T.type = @type1 OR T.type = @type2 OR T.type = @type3 OR T.type = @type4) " & vbCrLf &
            "AND T.categorie = @categorie " & vbCrLf &
            "AND (T.etat = @etat1 OR T.etat = @etat2) " & vbCrLf &
            "AND (T.cloture = 'False' OR T.cloture is Null)" & vbCrLf

        ' -- filtre fonctions

        SQLString += "ORDER BY T.date_rendez_vous DESC"
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@patientId", patientId)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type1", TacheDao.TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type2", TacheDao.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type3", TacheDao.TypeTache.RDV_MISSION.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type4", TacheDao.TypeTache.RDV_DEMANDE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@categorie", TacheDao.CategorieTache.SOIN.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat1", TacheDao.EtatTache.EN_COURS.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat2", TacheDao.EtatTache.EN_ATTENTE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function GetDernierRenezVousByPatientId(patientId As Long, parcoursId As Long) As Tache
        Dim con As SqlConnection
        Dim tache As New Tache
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE patient_Id = @patientId AND etat = @etat AND parcours_id = @parcoursId AND categorie = @categorie AND [type] = @type AND nature = @nature" &
                " ORDER BY date_rendez_vous"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@parcoursId", parcoursId)
                .AddWithValue("@etat", EtatTache.TERMINEE.ToString)
                .AddWithValue("@categorie", CategorieTache.SOIN.ToString)
                .AddWithValue("@type", TypeTache.RDV.ToString)
                .AddWithValue("@nature", NatureTache.RDV.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = buildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return tache
    End Function

    ''' <summary>
    ''' retrouve une tache par son id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="isWithAnnule"></param>
    ''' <returns></returns>
    Public Function getTacheById(id As Integer, Optional ByVal isWithAnnule As Boolean = False) As Tache
        Dim tache As Tache
        Dim con As SqlConnection
        Dim isWhere As Boolean = False

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()
            Dim strRequete
            strRequete =
               "select * " &
               "from oasis.oa_tache " &
               "where id = @id"
            If isWithAnnule = False Then
                isWhere = True
                strRequete += " AND etat <> @etat " + vbCrLf
            End If
            command.CommandText = strRequete
            command.Parameters.AddWithValue("@id", id)
            If isWithAnnule = False Then command.Parameters.AddWithValue("@etat", EtatTache.ANNULEE.ToString)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = buildBean(reader)
                Else
                    Throw New ArgumentException("Tâche non retrouvée !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return tache

    End Function

    Friend Function GetProchainRendezVousByPatientId(patientId As Long, parcoursId As Long) As Tache
        Dim dateRendezVous As Date = Nothing
        Dim tache As New Tache
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE patient_Id = @patientId" &
                " AND (etat = @etat1 OR etat = @etat2)" &
                " AND parcours_id = @parcoursId" &
                " AND categorie = @categorie" &
                " AND ([type] = @type1 OR [type] = @type2)" &
                " AND (nature = @nature1 OR nature = @nature2)" &
                " ORDER BY date_rendez_vous DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@parcoursId", parcoursId)
                .AddWithValue("@etat1", EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@etat2", EtatTache.EN_COURS.ToString)
                .AddWithValue("@categorie", CategorieTache.SOIN.ToString)
                .AddWithValue("@type1", TypeTache.RDV.ToString)
                .AddWithValue("@type2", TypeTache.RDV_SPECIALISTE.ToString)
                .AddWithValue("@nature1", NatureTache.RDV.ToString)
                .AddWithValue("@nature2", NatureTache.RDV_SPECIALISTE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = buildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return tache
    End Function

    Friend Function GetProchaineDemandeRendezVousByPatientId(patientId As Long, parcoursId As Long) As Tache
        Dim tache As New Tache
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE patient_Id = @patientId" &
                " AND (etat = @etat1 OR etat = @etat2)" &
                " AND parcours_id = @parcoursId" &
                " AND categorie = @categorie" &
                " AND [type] = @type" &
                " AND nature = @nature" &
                " ORDER BY date_rendez_vous DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@parcoursId", parcoursId)
                .AddWithValue("@etat1", EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@etat2", EtatTache.EN_COURS.ToString)
                .AddWithValue("@categorie", CategorieTache.SOIN.ToString)
                .AddWithValue("@type", TypeTache.RDV_DEMANDE.ToString)
                .AddWithValue("@nature", NatureTache.RDV_DEMANDE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = buildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return tache
    End Function

    Friend Function attribueTacheToUserLog(idTache As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim isOK As Boolean = True
        Dim con As SqlConnection = Nothing
        Dim nbUpdate As Integer

        Try
            con = GetConnection()

            Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " etat = @etat, traite_user_id = @traiteUserId, horodate_attrib = @dateAttrib" &
            " WHERE id = @Id" &
            " And etat = @etatWhere" &
            " And traite_user_id Is null"

            Dim cmd As New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@etat", EtatTache.EN_COURS.ToString)
                .AddWithValue("@traiteUserId", userLog.UtilisateurId)
                .AddWithValue("@dateAttrib", Date.Now())
                .AddWithValue("@Id", idTache)
                .AddWithValue("@etatWhere", EtatTache.EN_ATTENTE.ToString)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Tâche déjà traitée par un autre utilisateur !")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            isOK = False
        Finally
            If IsNothing(con) = False Then
                Try
                    con.Close()
                Catch
                End Try
            End If
        End Try
        Return isOK

    End Function
    Friend Function desattribueTache(idTache As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim isOK As Boolean = True
        Dim con As SqlConnection = Nothing
        Dim nbUpdate As Integer

        Try
            con = GetConnection()

            Dim SQLstring As String = "update oasis.oa_tache set" &
            " etat = @etat, traite_user_id = @traiteUserId, horodate_attrib = @dateAttrib" &
            " where id = @Id And etat = @etatWhere And traite_user_id Is Not null"

            Dim cmd As New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@etat", EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@traiteUserId", DBNull.Value)
                .AddWithValue("@dateAttrib", DBNull.Value)
                .AddWithValue("@Id", idTache)
                .AddWithValue("@etatWhere", EtatTache.EN_COURS.ToString)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Tâche déjà traitée par un autre utilisateur !")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            isOK = False
        Finally
            If IsNothing(con) = False Then
                Try
                    con.Close()
                Catch
                End Try
            End If
        End Try
        Return isOK

    End Function

    Private Function buildBean(reader As SqlDataReader) As Tache
        Dim tache As New Tache
        tache.Id = reader("id")
        tache.ParentId = Coalesce(reader("parent_id"), 0)
        tache.EmetteurUserId = Coalesce(reader("emetteur_user_id"), 0)
        tache.EmetteurFonctionId = Coalesce(reader("emetteur_fonction_id"), 0)
        tache.UniteSanitaireId = Coalesce(reader("unite_sanitaire_id"), 0)
        tache.SiteId = Coalesce(reader("site_id"), 0)
        tache.PatientId = Coalesce(reader("patient_id"), 0)
        tache.ParcoursId = Coalesce(reader("parcours_id"), 0)
        tache.EpisodeId = Coalesce(reader("episode_id"), 0)
        tache.SousEpisodeId = Coalesce(reader("sous_episode_id"), 0)
        tache.TraiteUserId = Coalesce(reader("traite_user_id"), 0)
        tache.TraiteFonctionId = Coalesce(reader("traite_fonction_id"), 0)
        tache.DestinataireFonctionId = Coalesce(reader("destinataire_fonction_id"), 0)
        tache.Priorite = Coalesce(reader("priorite"), 0)
        tache.OrdreAffichage = Coalesce(reader("ordre_affichage"), 0)
        tache.Categorie = Coalesce(reader("categorie"), "")
        tache.Type = Coalesce(reader("type"), "")
        tache.Nature = Coalesce(reader("nature"), "")
        tache.Duree = Coalesce(reader("duree_mn"), 0)
        tache.EmetteurCommentaire = Coalesce(reader("emetteur_commentaire"), "")
        tache.HorodatageCreation = Coalesce(reader("horodate_creation"), Nothing)
        tache.HorodatageAttribution = Coalesce(reader("horodate_attrib"), Nothing)
        tache.HorodatageCloture = Coalesce(reader("horodate_cloture"), Nothing)
        tache.Etat = Coalesce(reader("etat"), "")
        tache.Cloture = Coalesce(reader("cloture"), False)
        tache.TypedemandeRendezVous = Coalesce(reader("type_demande_rendez_vous"), "")
        tache.DateRendezVous = Coalesce(reader("date_rendez_vous"), Nothing)
        Return tache
    End Function

    Friend Function CreateTache(tache As Tache, Optional ifExist As String = "") As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            ' --- test si update tache parent todo
            If tache.ParentId <> 0 Then
                closTache(con, tache.ParentId, EtatTache.TERMINEE, False, transaction)
            End If

            Dim SQLstring As String = "INSERT INTO oasis.oa_tache " &
                    "(parent_id, emetteur_user_id, emetteur_fonction_id, unite_sanitaire_id, site_id, patient_id, parcours_id, episode_id," &
                    " sous_episode_id, traite_user_id, traite_fonction_id, destinataire_fonction_id, priorite, ordre_affichage, categorie, type," &
                    " nature, duree_mn, emetteur_commentaire, horodate_creation, etat, cloture, type_demande_rendez_vous, date_rendez_vous)" &
            " VALUES (@parentId, @emetteurId, @emetteurFonctionId, @uniteSanitaireId, @siteId, @patientId, @parcoursId, @episodeId," &
                    " @sousEpisodeId, @traiteUserId, @traiteFonctionId, @destinataireFonctionId, @priorite, @ordreAffichage, @categorie, @type," &
                    " @nature, @duree, @commentaire, @dateCreation, @etat, @cloture, @typedemandeRendezVous, @dateRendezVous)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@parentId", If(tache.ParentId = 0, DBNull.Value, tache.ParentId))
                .AddWithValue("@emetteurId", tache.EmetteurUserId)
                .AddWithValue("@emetteurFonctionId", tache.EmetteurFonctionId)
                .AddWithValue("@uniteSanitaireId", If(tache.UniteSanitaireId = 0, DBNull.Value, tache.UniteSanitaireId))
                .AddWithValue("@siteId", If(tache.SiteId = 0, DBNull.Value, tache.SiteId))
                .AddWithValue("@patientId", tache.PatientId)
                .AddWithValue("@parcoursId", If(tache.ParcoursId = 0, DBNull.Value, tache.ParcoursId))
                .AddWithValue("@episodeId", If(tache.EpisodeId = 0, DBNull.Value, tache.EpisodeId))
                .AddWithValue("@sousEpisodeId", If(tache.SousEpisodeId = 0, DBNull.Value, tache.SousEpisodeId))
                .AddWithValue("@traiteUserId", If(tache.TraiteUserId = 0, DBNull.Value, tache.TraiteUserId))
                .AddWithValue("@traiteFonctionId", tache.TraiteFonctionId)
                .AddWithValue("@destinataireFonctionId", If(tache.DestinataireFonctionId = 0, DBNull.Value, tache.DestinataireFonctionId))
                .AddWithValue("@priorite", tache.Priorite)
                .AddWithValue("@ordreAffichage", tache.OrdreAffichage)
                .AddWithValue("@categorie", tache.Categorie)
                .AddWithValue("@type", tache.Type)
                .AddWithValue("@nature", tache.Nature)
                .AddWithValue("@duree", If(tache.Duree = 0, DBNull.Value, tache.Duree))
                .AddWithValue("@commentaire", tache.EmetteurCommentaire)
                .AddWithValue("@dateCreation", tache.HorodatageCreation)
                .AddWithValue("@etat", tache.Etat)
                .AddWithValue("@cloture", tache.Cloture)
                .AddWithValue("@typeDemandeRendezVous", If(tache.TypedemandeRendezVous = "", DBNull.Value, tache.TypedemandeRendezVous))
                .AddWithValue("@dateRendezVous", If(tache.DateRendezVous = Nothing, DBNull.Value, tache.DateRendezVous))
            End With

            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function CreateRendezVous(tache As Tache) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF Not EXISTS (SELECT 1 FROM oasis.oa_tache" & vbCrLf &
        " WHERE patient_id = " & tache.PatientId & vbCrLf &
        " And parcours_id = " & tache.ParcoursId & vbCrLf &
        " And (type = '" & TypeTache.RDV.ToString &
            "' OR (type = '" & TypeTache.RDV_SPECIALISTE.ToString &
            "' OR (type = '" & TypeTache.RDV_DEMANDE.ToString & "')" & vbCrLf &
        " AND (etat = '" & EtatTache.EN_COURS.ToString & "' OR etat = '" & EtatTache.EN_ATTENTE.ToString & "'))"

        'Console.WriteLine(SQLstring)

        Try
            CreateTache(tache, SQLstring)
        Catch ex As Exception
            MsgBox(ex.Message)
            codeRetour = False
        End Try

        Return codeRetour
    End Function

    Friend Function CreationDemandeAvis(tache As Tache) As Boolean
        Dim nbCreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_tache" &
        " WHERE patient_id = @patientId" &
        " AND episode_id = @episodeId" &
        " AND type = '" & TypeTache.AVIS_EPISODE.ToString & "'" &
        " AND (etat = '" & EtatTache.EN_COURS.ToString & "' OR etat = '" & EtatTache.EN_ATTENTE.ToString & "'))" &
        "INSERT INTO oasis.oa_tache " &
                "(parent_id, emetteur_user_id, emetteur_fonction_id, unite_sanitaire_id, site_id, patient_id, parcours_id, episode_id," &
                " sous_episode_id, traite_user_id, traite_fonction_id, destinataire_fonction_id, priorite, ordre_affichage, categorie, type," &
                " nature, duree_mn, emetteur_commentaire, horodate_creation, etat, cloture, type_demande_rendez_vous, date_rendez_vous)" &
        " VALUES (@parentId, @emetteurId, @emetteurFonctionId, @uniteSanitaireId, @siteId, @patientId, @parcoursId, @episodeId," &
                " @sousEpisodeId, @traiteUserId, @traiteFonctionId, @destinataireFonctionId, @priorite, @ordreAffichage, @categorie, @type," &
                " @nature, @duree, @commentaire, @dateCreation, @etat, @cloture, @typedemandeRendezVous, @dateRendezVous)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@parentId", If(tache.ParentId = 0, DBNull.Value, tache.ParentId))
            .AddWithValue("@emetteurId", tache.EmetteurUserId)
            .AddWithValue("@emetteurFonctionId", tache.EmetteurFonctionId)
            .AddWithValue("@uniteSanitaireId", If(tache.UniteSanitaireId = 0, DBNull.Value, tache.UniteSanitaireId))
            .AddWithValue("@siteId", If(tache.SiteId = 0, DBNull.Value, tache.SiteId))
            .AddWithValue("@patientId", tache.PatientId)
            .AddWithValue("@parcoursId", If(tache.ParcoursId = 0, DBNull.Value, tache.ParcoursId))
            .AddWithValue("@episodeId", If(tache.EpisodeId = 0, DBNull.Value, tache.EpisodeId))
            .AddWithValue("@sousEpisodeId", If(tache.SousEpisodeId = 0, DBNull.Value, tache.SousEpisodeId))
            .AddWithValue("@traiteUserId", If(tache.TraiteUserId = 0, DBNull.Value, tache.TraiteUserId))
            .AddWithValue("@traiteFonctionId", tache.TraiteFonctionId)
            .AddWithValue("@destinataireFonctionId", If(tache.DestinataireFonctionId = 0, DBNull.Value, tache.DestinataireFonctionId))
            .AddWithValue("@priorite", tache.Priorite)
            .AddWithValue("@ordreAffichage", tache.OrdreAffichage)
            .AddWithValue("@categorie", tache.Categorie)
            .AddWithValue("@type", tache.Type)
            .AddWithValue("@nature", tache.Nature)
            .AddWithValue("@duree", If(tache.Duree = 0, DBNull.Value, tache.Duree))
            .AddWithValue("@commentaire", tache.EmetteurCommentaire)
            .AddWithValue("@dateCreation", tache.HorodatageCreation)
            .AddWithValue("@etat", tache.Etat)
            .AddWithValue("@cloture", tache.Cloture)
            .AddWithValue("@typeDemandeRendezVous", If(tache.TypedemandeRendezVous = "", DBNull.Value, tache.TypedemandeRendezVous))
            .AddWithValue("@dateRendezVous", If(tache.DateRendezVous = Nothing, DBNull.Value, tache.DateRendezVous))
        End With

        Try
            da.InsertCommand = cmd
            nbCreate = da.InsertCommand.ExecuteNonQuery()
            If nbCreate <= 0 Then
                Throw New Exception("Collision: Une demande d'avis en cours ou en attente de traitement existe déjà pour ce patient et cet épisode")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CreateLog(ex.ToString, "TacheDao", LogDao.EnumTypeLog.ERREUR.ToString)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function AnnulationTache(tache As Tache) As Boolean
        Return AnnulationTache(tache.Id)
    End Function

    Friend Function AnnulationTache(idTache As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Try
            closTache(con, idTache, EtatTache.ANNULEE, True)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function
    Friend Sub closTache(con As SqlConnection, idTache As Long, etatFinal As EtatTache, cloture As Boolean, Optional transaction As SqlTransaction = Nothing)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim nbUpdate As Integer

        Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " etat = @etat, traite_user_id = @traiteUserId, horodate_cloture = @dateCloture, cloture = @Cloture " &
            " WHERE id = @Id AND (traite_user_id is null OR traite_user_id = @traiteUserId2) AND etat<> @etat2"

        Dim cmd As SqlCommand
        If transaction Is Nothing Then
            cmd = New SqlCommand(SQLstring, con)
        Else
            cmd = New SqlCommand(SQLstring, con, transaction)
        End If
        With cmd.Parameters
            .AddWithValue("@etat", etatFinal.ToString)
            .AddWithValue("@traiteUserId", userLog.UtilisateurId)
            .AddWithValue("@dateCloture", Date.Now())
            .AddWithValue("@Cloture", cloture)
            .AddWithValue("@Id", idTache)
            .AddWithValue("@traiteUserId2", userLog.UtilisateurId)
            .AddWithValue("@etat2", etatFinal.ToString)
        End With

        da.UpdateCommand = cmd
        nbUpdate = da.UpdateCommand.ExecuteNonQuery()
        If nbUpdate <= 0 Then
            Throw New Exception("Collision , Tâche déjà traitée par un autre utilisateur !")
        End If
    End Sub


    Friend Function ClotureTache(idTache As Long, cloture As Boolean) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Try
            closTache(con, idTache, EtatTache.TERMINEE, cloture)
            codeRetour = True
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function



    ''' <summary>
    ''' recherche tous les bean associés à une tache (user , fonction, unité sanitaire  ...etc)
    ''' </summary>
    ''' <param name="tache"></param>
    ''' <returns></returns>
    Public Function getTacheBeanAssocie(tache As Tache) As TacheBeanAssocie
        Dim userDao As New UserDao
        Dim fonctionDao As New FonctionDao
        Dim uniteSanitaireDao As New UniteSanitaireDao
        Dim siteDao As New SiteDao
        Dim parcoursDao As New ParcoursDao
        Dim episodeDao As New EpisodeDao
        Dim rorDao As New RorDao

        Dim tacheBeanAssocie = New TacheBeanAssocie
        tacheBeanAssocie.UserEmetteur = userDao.getUserById(tache.EmetteurUserId)
        If tache.EmetteurFonctionId <> 0 Then tacheBeanAssocie.FonctionEmetteur = fonctionDao.getFonctionById(tache.EmetteurFonctionId)
        tacheBeanAssocie.UniteSanitaire = uniteSanitaireDao.getUniteSanitaireById(tache.UniteSanitaireId, True)
        tacheBeanAssocie.Site = siteDao.getSiteById(tache.SiteId, True)
        tacheBeanAssocie.Patient = PatientDao.getPatientById(tache.PatientId)
        If tache.ParcoursId <> 0 Then
            tacheBeanAssocie.Parcours = parcoursDao.getParcoursById(tache.ParcoursId)
            If tacheBeanAssocie.Parcours.SpecialiteId <> 0 Then
                tacheBeanAssocie.Specialite = Environnement.Table_specialite.GetSpecialiteById(tacheBeanAssocie.Parcours.SpecialiteId)
                If tacheBeanAssocie.Specialite.Oasis = False Then
                    ' -- on recherche le nom de l'intervenant
                    Dim ror As Ror = rorDao.getRorById(tacheBeanAssocie.Parcours.RorId)
                    tacheBeanAssocie.Intervenant = ror.Nom
                End If
            End If
        End If
        If tache.TraiteFonctionId Then tacheBeanAssocie.FonctionTraiteur = fonctionDao.getFonctionById(tache.TraiteFonctionId)
        If tache.TraiteUserId <> 0 Then
            tacheBeanAssocie.UserTraiteur = userDao.getUserById(tache.TraiteUserId)
        End If
        Return tacheBeanAssocie

    End Function

    Public Function GetDemandeEnCoursByEpisode(episodeId As Long) As Tache
        Dim con As SqlConnection
        Dim tache As New Tache
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE (etat = @etat1 OR etat = @etat2) AND episode_id = @episodeId AND categorie = @categorie AND [type] = @type"

            With command.Parameters
                .AddWithValue("@episodeId", episodeId)
                .AddWithValue("@etat1", EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@etat2", EtatTache.EN_COURS.ToString)
                .AddWithValue("@categorie", CategorieTache.SOIN.ToString)
                .AddWithValue("@type", TypeTache.AVIS_EPISODE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = buildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = TypeTache.AVIS_EPISODE.ToString
                    tache.Nature = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return tache
    End Function

    ''' <summary>
    ''' Creation de rendez : insert simple ou on termine la demande (update) et insert du rdv
    ''' </summary>
    ''' <param name="patient"></param>
    ''' <param name="parcours"></param>
    ''' <param name="tacheParent"></param>
    Public Sub createRendezVous(patient As Patient, parcours As Parcours, typeTache As TypeTache, dateRDV As Date, duree As Integer, commentaire As String, Optional tacheParent As Tache = Nothing)
        Dim tache As Tache = New Tache()

        If typeTache <> TypeTache.RDV_MISSION AndAlso typeTache <> TypeTache.RDV Then
            Throw New Exception("Pas de rendez-vous possible sur ce type de tache !")
        End If

        ' --- parent id
        If Not IsNothing(tacheParent) Then
            If (tacheParent.Type = TypeTache.MISSION_DEMANDE.ToString() AndAlso typeTache = TypeTache.RDV_MISSION) _
                    OrElse (tacheParent.Type = TypeTache.RDV_DEMANDE.ToString() AndAlso typeTache = TypeTache.RDV) Then
                tache.ParentId = tacheParent.Id
            Else
                Throw New Exception("Pas de rendez-vous possible sur ce type de tache parent !")
            End If
        Else
                tache.ParentId = 0
        End If

        ' --- on set emetteurFonctionId, traiteFonctionId et destinataireFonctionid
        SetFonctionsIdFromUserLogAnParcours(tache, parcours, tacheParent)
        ' --- set du reste 
        tache.UniteSanitaireId = patient.PatientUniteSanitaireId
        tache.SiteId = patient.PatientSiteId
        tache.PatientId = patient.patientId
        tache.ParcoursId = parcours.Id
        tache.EpisodeId = 0
        tache.SousEpisodeId = 0
        tache.TraiteUserId = 0
        tache.Priorite = TacheDao.Priorite.BASSE
        tache.OrdreAffichage = 20
        tache.Categorie = TacheDao.CategorieTache.SOIN.ToString
        tache.Type = typeTache.ToString
        tache.Nature = tache.Type  ' nature = type en string sur ce type de tache
        tache.Duree = duree
        tache.EmetteurCommentaire = commentaire
        tache.HorodatageCreation = Date.Now()
        tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString
        'tache.TypedemandeRendezVous =     ' => pas sur ce type de tache
        tache.DateRendezVous = dateRDV

        CreateTache(tache)

    End Sub

    Friend Function ModificationRendezVous(tache As Tache) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim isOK As Boolean = True
        Dim con As SqlConnection = Nothing
        Dim nbUpdate As Integer

        Try
            con = GetConnection()

            Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " type_demande_rendez_vous = @typeDemandeRendezVous, date_rendez_vous = @dateRendezVous, emetteur_commentaire = @commentaire" &
            " WHERE id = @Id AND etat = @etat"

            Dim cmd As New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@Id", tache.Id)
                .AddWithValue("@typeDemandeRendezVous", tache.TypedemandeRendezVous)
                .AddWithValue("@dateRendezVous", tache.DateRendezVous)
                .AddWithValue("@commentaire", tache.EmetteurCommentaire)
                .AddWithValue("@etat", TacheDao.EtatTache.EN_ATTENTE.ToString)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Annulation de la modification, la tâche n'est plus disponible !")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            isOK = False
        Finally
            If IsNothing(con) = False Then
                Try
                    con.Close()
                Catch
                End Try
            End If
        End Try
        Return isOK

    End Function

    Private Sub SetFonctionsIdFromUserLogAnParcours(tache As Tache, parcours As Parcours, Optional tacheParent As Tache = Nothing)

        With tache
            ' -- emetteur ID
            .EmetteurUserId = userLog.UtilisateurId
            ' -- emetteur fonction
            Select Case userLog.UtilisateurProfilId.Trim()
                Case "IDE"
                    .EmetteurFonctionId = FonctionDao.enumFonction.IDE
                Case "IDE_REMPLACANT"
                    .EmetteurFonctionId = FonctionDao.enumFonction.IDE_REMPLACANT
                Case "MEDECIN"
                    .EmetteurFonctionId = FonctionDao.enumFonction.MEDECIN
                Case "SAGE_FEMME"
                    .EmetteurFonctionId = FonctionDao.enumFonction.SAGE_FEMME
                Case "CADRE_SANTE"
                    .EmetteurFonctionId = FonctionDao.enumFonction.CADRE_SANTE
                Case "SECRETAIRE_MEDICALE"
                    .EmetteurFonctionId = FonctionDao.enumFonction.SECRETAIRE_MEDICALE
                Case "ADMINISTRATIF"
                    .EmetteurFonctionId = FonctionDao.enumFonction.ADMINISTRATIF
                Case Else
                    .EmetteurFonctionId = FonctionDao.enumFonction.INCONNU
            End Select

            ' --- parcours
            Select Case parcours.SousCategorieId
                Case EnumSousCategoriePPS.medecinReferent
                    .DestinataireFonctionId = FonctionDao.enumFonction.MEDECIN
                    .TraiteFonctionId = FonctionDao.enumFonction.MEDECIN
                Case EnumSousCategoriePPS.IDE
                    .DestinataireFonctionId = FonctionDao.enumFonction.IDE
                    .TraiteFonctionId = FonctionDao.enumFonction.IDE
                Case EnumSousCategoriePPS.sageFemme
                    If parcours.SpecialiteId = EnumSpecialiteOasis.sageFemmeOasis Then
                        .DestinataireFonctionId = FonctionDao.enumFonction.SAGE_FEMME
                        .TraiteFonctionId = FonctionDao.enumFonction.SAGE_FEMME
                    Else
                        .DestinataireFonctionId = FonctionDao.enumFonction.SPECIALISTE_NON_OASIS
                        .TraiteFonctionId = FonctionDao.enumFonction.IDE
                    End If
                Case EnumSousCategoriePPS.specialiste
                    .DestinataireFonctionId = FonctionDao.enumFonction.SPECIALISTE_NON_OASIS
                    .TraiteFonctionId = FonctionDao.enumFonction.IDE
                Case Else
                    .DestinataireFonctionId = FonctionDao.enumFonction.INCONNU
                    .TraiteFonctionId = FonctionDao.enumFonction.IDE
            End Select
        End With

    End Sub


End Class
