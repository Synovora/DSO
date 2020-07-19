Imports System.Data.SqlClient
Imports System.Configuration

Public Class TacheDao
    Inherits StandardDao

    Public Function GetCodeNatureTacheByItem(item As String) As String
        Dim CodeNatureTache As String

        Select Case item
            Case Tache.EnumNatureTacheItem.DEMANDE
                CodeNatureTache = Tache.EnumNatureTacheCode.DEMANDE
            Case Tache.EnumNatureTacheItem.COMPLEMENT
                CodeNatureTache = Tache.EnumNatureTacheCode.COMPLEMENT
            Case Tache.EnumNatureTacheItem.MISSION_DEMANDE
                CodeNatureTache = Tache.EnumNatureTacheCode.MISSION_DEMANDE
            Case Tache.EnumNatureTacheItem.RDV
                CodeNatureTache = Tache.EnumNatureTacheCode.RDV
            Case Tache.EnumNatureTacheItem.RDV_DEMANDE
                CodeNatureTache = Tache.EnumNatureTacheCode.RDV_DEMANDE
            Case Tache.EnumNatureTacheItem.RDV_SPECIALISTE
                CodeNatureTache = Tache.EnumNatureTacheCode.RDV_SPECIALISTE
            Case Tache.EnumNatureTacheItem.REPONSE
                CodeNatureTache = Tache.EnumNatureTacheCode.REPONSE
            Case Tache.EnumNatureTacheItem.REUNION_STAFF
                CodeNatureTache = Tache.EnumNatureTacheCode.REUNION_STAFF
            Case Else
                CodeNatureTache = "Inconnue"
        End Select

        Return CodeNatureTache
    End Function

    Public Function GetItemNatureTacheByCode(code As String) As String
        Dim ItemNatureTache As String

        Select Case code
            Case Tache.EnumNatureTacheCode.DEMANDE
                ItemNatureTache = Tache.EnumNatureTacheItem.DEMANDE
            Case Tache.EnumNatureTacheCode.COMPLEMENT
                ItemNatureTache = Tache.EnumNatureTacheItem.COMPLEMENT
            Case Tache.EnumNatureTacheCode.MISSION_DEMANDE
                ItemNatureTache = Tache.EnumNatureTacheItem.MISSION_DEMANDE
            Case Tache.EnumNatureTacheCode.RDV
                ItemNatureTache = Tache.EnumNatureTacheItem.RDV
            Case Tache.EnumNatureTacheCode.RDV_DEMANDE
                ItemNatureTache = Tache.EnumNatureTacheItem.RDV_DEMANDE
            Case Tache.EnumNatureTacheCode.RDV_SPECIALISTE
                ItemNatureTache = Tache.EnumNatureTacheItem.RDV_SPECIALISTE
            Case Tache.EnumNatureTacheCode.REPONSE
                ItemNatureTache = Tache.EnumNatureTacheItem.REPONSE
            Case Tache.EnumNatureTacheCode.REUNION_STAFF
                ItemNatureTache = Tache.EnumNatureTacheItem.REUNION_STAFF
            Case Else
                ItemNatureTache = "Inconnue"
        End Select

        Return ItemNatureTache
    End Function

    Public Function GetAllTacheATraiterChk(lstFonction As List(Of Fonction), filtreTache As FiltreTache) As Integer
        Dim SQLString As String

        'Console.WriteLine("         .. Appel allTacheAtraiter CHK")
        SQLString =
            "SELECT CHECKSUM_AGG(cast(id as int)  ) as CHK " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "WHERE etat = @etat And type<> @typeSpecialiste AND (type NOT IN (@typeRdv, @typeRdvMission) OR (type IN (@typeRdv, @typeRdvMission) AND CONVERT(date, date_rendez_vous) <= CONVERT(date, @dateRdv))) " & vbCrLf

        ' --- ajout des filtre
        SQLString += AddFiltreAllTacheATraiter(lstFonction, filtreTache)

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.EN_ATTENTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeSpecialiste", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeRdv", Tache.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeRdvMission", Tache.TypeTache.RDV_MISSION.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@dateRdv", DateTime.Now)
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
    Public Function GetAllTacheATraiter(lstFonction As List(Of Fonction), filtreTache As FiltreTache) As DataTable
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
            "     T.emetteur_fonction_id, " & vbCrLf &
            "     T.emetteur_commentaire, " & vbCrLf &
            "     T.horodate_creation, " & vbCrLf &
            "     T.priorite, " & vbCrLf &
            "	  T.ordre_affichage, " & vbCrLf &
           "	  coalesce(F.oa_r_fonction_designation,'') as emetteur_fonction, " & vbCrLf &
             "	  T.date_rendez_vous " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_r_fonction F ON F.oa_r_fonction_id = T.emetteur_fonction_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE etat = @etat And type<> @typeSpecialiste AND (type NOT IN (@typeRdv,@typeRdvMission) OR (type IN (@typeRdv,@typeRdvMission) AND CONVERT(date,date_rendez_vous) <= CONVERT(date, @daterdv))) " & vbCrLf

        ' --- ajout des filtre
        SQLString += AddFiltreAllTacheATraiter(lstFonction, filtreTache)

        SQLString += "ORDER BY priorite,ordre_affichage, COALESCE(date_rendez_vous, horodate_creation) "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.EN_ATTENTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeSpecialiste", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeRdv", Tache.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeRdvMission", Tache.TypeTache.RDV_MISSION.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@daterdv", DateTime.Now)
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

    Private Function AddFiltreAllTacheATraiter(lstFonction As List(Of Fonction), filtreTache As FiltreTache) As String
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
    Public Function GetAllTacheEnCours(isMyTache As Boolean, lstFonctionChoisie As List(Of Fonction), filtre As FiltreTache, isWithNonAttribue As Boolean, userLog As Utilisateur) As DataTable
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
           "	  coalesce(U.oa_utilisateur_nom,'') as user_traiteur_nom, " & vbCrLf &
            "	  coalesce(U.oa_utilisateur_prenom,'') as user_traiteur_prenom, " & vbCrLf &
           "	  coalesce(F.oa_r_fonction_designation,'') as emetteur_fonction, " & vbCrLf &
           "	  coalesce(F2.oa_r_fonction_designation,'') as traite_fonction, " & vbCrLf &
            "	  T.date_rendez_vous " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_utilisateur U ON U.oa_utilisateur_id = T.traite_user_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_r_fonction F ON F.oa_r_fonction_id = T.emetteur_fonction_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_r_fonction F2 ON F2.oa_r_fonction_id = T.traite_fonction_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE type<> @type " & vbCrLf

        SQLString += "AND " & If(isMyTache = False AndAlso isWithNonAttribue, " etat IN (@etat , @etat1) " & vbCrLf, " etat = @etat") & vbCrLf

        ' "	  T.ordre_affichage, " & vbCrLf &

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

        SQLString += "ORDER BY priorite,ordre_affichage, COALESCE(date_rendez_vous, horodate_creation) "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.EN_COURS.ToString)
                If (isMyTache = False AndAlso isWithNonAttribue) Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat1", Tache.EtatTache.EN_ATTENTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type", Tache.TypeTache.RDV_SPECIALISTE.ToString)
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


    'Liste de toutes les tâches attribuées (en cours)
    Public Function GetAllRendezVousEnCours() As DataTable
        Dim SQLString As String

        'Console.WriteLine("----------> GetAllTachesEnTraitement")
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
            "     T.type_demande_rendez_vous," & vbCrLf &
            "	  coalesce(U.oa_utilisateur_nom,'') as user_traiteur_nom, " & vbCrLf &
            "	  coalesce(U.oa_utilisateur_prenom,'') as user_traiteur_prenom, " & vbCrLf &
            "	  coalesce(F.oa_r_fonction_designation,'') as emetteur_fonction, " & vbCrLf &
            "	  coalesce(F2.oa_r_fonction_designation,'') as traite_fonction, " & vbCrLf &
            "	  T.date_rendez_vous " & vbCrLf &
            " FROM [oasis].[oa_tache] T " & vbCrLf &
            " LEFT JOIN  oasis.oa_utilisateur U ON U.oa_utilisateur_id = T.traite_user_id " & vbCrLf &
            " LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            " LEFT JOIN  oasis.oa_r_fonction F ON F.oa_r_fonction_id = T.emetteur_fonction_id " & vbCrLf &
            " LEFT JOIN  oasis.oa_r_fonction F2 ON F2.oa_r_fonction_id = T.traite_fonction_id " & vbCrLf &
            " LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            " WHERE etat = @etat" & vbCrLf &
            " AND (type = @type1 OR type = @type2 OR type = @type3)" &
            " ORDER BY date_rendez_vous "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.EN_COURS.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type1", Tache.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type2", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type3", Tache.TypeTache.RDV_DEMANDE.ToString)

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

    Public Function GetAgendaMyRDV(dateDebut As Date, dateFin As Date, isMyTache As Boolean, lstFonctionChoisie As List(Of Fonction), filtre As FiltreTache, isWithNonAttribue As Boolean, userLog As Utilisateur) As DataTable
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
            "WHERE [type] IN (@typeTache1 , @typeTache2, @typeTache3) " & vbCrLf &
            "AND date_rendez_vous BETWEEN @datedebut AND @datefin " & vbCrLf

        SQLString += "AND " & If(isMyTache = False AndAlso isWithNonAttribue, " etat IN (@etat , @etat1) " & vbCrLf, " etat = @etat") & vbCrLf

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
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.EN_COURS.ToString)
                If (isMyTache = False AndAlso isWithNonAttribue) Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat1", Tache.EtatTache.EN_ATTENTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache1", Tache.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache2", Tache.TypeTache.RDV_MISSION.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache3", Tache.TypeTache.REUNION_STAFF.ToString)
                'tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@typeTache4", TypeTache.RDV_SPECIALISTE.ToString)
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
    Public Function GetAllTacheEmise(isNotFinal As Boolean, userLog As Utilisateur) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT TOP 1000 " & vbCrLf &
            "	  T.id, " & vbCrLf &
            "	  T.etat, " & vbCrLf &
            "     T.priorite, " & vbCrLf &
            "     S.oa_site_description as site_description, " & vbCrLf &
            "     P.oa_patient_nom as patient_nom, " & vbCrLf &
            "     P.oa_patient_prenom as patient_prenom, " & vbCrLf &
            "     T.type , " & vbCrLf &
            "     T.nature, " & vbCrLf &
            "     T.emetteur_commentaire, " & vbCrLf &
             "	  T.date_rendez_vous, " & vbCrLf &
            "	  coalesce(F.oa_r_fonction_designation,'') as emetteur_fonction, " & vbCrLf &
           "	  coalesce(U.oa_utilisateur_nom,'') as user_traiteur_nom, " & vbCrLf &
            "	  coalesce(U.oa_utilisateur_prenom,'') as user_traiteur_prenom, " & vbCrLf &
            "	  coalesce(F2.oa_r_fonction_designation,'') as user_traiteur_fonction " & vbCrLf &
            "FROM [oasis].[oa_tache] T " & vbCrLf &
            "LEFT JOIN  oasis.oa_utilisateur U ON U.oa_utilisateur_id = T.traite_user_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_r_fonction F ON F.oa_r_fonction_id = T.emetteur_fonction_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_r_fonction F2 ON F2.oa_r_fonction_id = T.traite_fonction_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_site S ON S.oa_site_id = T.site_id " & vbCrLf &
            "LEFT JOIN  oasis.oa_patient P ON P.oa_patient_id = T.patient_id " & vbCrLf &
            "WHERE [emetteur_user_id] = @emetteur_user_id " & vbCrLf

        ' -- filtre Not Final
        If isNotFinal Then
            SQLString += "AND etat NOT IN ('" + Tache.EtatTache.ANNULEE.ToString + "','" + Tache.EtatTache.TERMINEE.ToString + "')" + vbCrLf
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

    Public Function GetWorkflowHistoByEpisode(episodeId As Long) As DataTable
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
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type", Tache.TypeTache.AVIS_EPISODE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.ANNULEE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function


    Public Function GetRDVByPatient(patientId As Long) As DataTable
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
            "	  COALESCE(RO.oa_ror_structure_nom,'') as oa_ror_structure_nom, " & vbCrLf &
            "	  COALESCE(US.oa_utilisateur_prenom,'') as oa_utilisateur_prenom, " & vbCrLf &
            "	  COALESCE(US.oa_utilisateur_nom,'') as oa_utilisateur_nom " & vbCrLf &
            "FROM oasis.oa_tache T " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_patient_parcours PA ON PA.oa_parcours_id = T.parcours_id " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_ror RO ON RO.oa_ror_id = PA.oa_parcours_ror_id " & vbCrLf &
            "LEFT OUTER JOIN  oasis.oa_utilisateur US ON US.oa_utilisateur_id = T.traite_user_id " & vbCrLf &
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
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type1", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type2", Tache.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type3", Tache.TypeTache.RDV_MISSION.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type4", Tache.TypeTache.RDV_DEMANDE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat1", Tache.EtatTache.EN_COURS.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat2", Tache.EtatTache.EN_ATTENTE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetRDVHistoriqueByPatient(patientId As Long, parcoursId As Long) As DataTable
        Dim SQLString As String

        SQLString =
            "SELECT " & vbCrLf &
            "     T.date_rendez_vous" & vbCrLf &
            " FROM oasis.oa_tache T" & vbCrLf &
            " WHERE patient_id = @patientId" & vbCrLf &
            " AND T.parcours_id = @parcoursId" &
            " AND (T.type = @type1 OR T.type = @type2)" & vbCrLf &
            " AND T.categorie = @categorie" & vbCrLf &
            " AND (T.etat = @etat)" &
            "AND T.cloture = 'True'" & vbCrLf
        ' -- filtre fonctions

        SQLString += " ORDER BY T.date_rendez_vous DESC"
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@patientId", patientId)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@parcoursId", parcoursId)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type1", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@type2", Tache.TypeTache.RDV.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", Tache.EtatTache.TERMINEE.ToString)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw New Exception(ex.Message)
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetDernierRenezVousByPatientId(patientId As Long, parcoursId As Long) As Tache
        Dim con As SqlConnection
        Dim tache As New Tache
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE patient_Id = @patientId" &
                " AND etat = @etat" &
                " AND parcours_id = @parcoursId" &
                " AND categorie = @categorie" &
                " AND ([type] = @type OR [type] = @type2)" &
                " AND (nature = @nature OR nature = @nature2)" &
                " ORDER BY date_rendez_vous DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@parcoursId", parcoursId)
                .AddWithValue("@etat", Tache.EtatTache.TERMINEE.ToString)
                .AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                .AddWithValue("@type", Tache.TypeTache.RDV.ToString)
                .AddWithValue("@type2", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                .AddWithValue("@nature", Tache.NatureTache.RDV.ToString)
                .AddWithValue("@nature2", Tache.NatureTache.RDV_SPECIALISTE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = BuildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
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
    Public Function GetTacheById(id As Integer, Optional ByVal isWithAnnule As Boolean = False) As Tache
        Dim tache As Tache
        Dim con As SqlConnection
        Dim isWhere As Boolean

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
                strRequete += " And etat <> @etat " + vbCrLf
            End If
            command.CommandText = strRequete
            command.Parameters.AddWithValue("@id", id)
            If isWithAnnule = False Then command.Parameters.AddWithValue("@etat", Tache.EtatTache.ANNULEE.ToString)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = BuildBean(reader)
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

    Public Function GetProchainRendezVousByPatientIdEtParcours(patientId As Long, parcoursId As Long) As Tache
        Dim dateRendezVous As Date = Nothing
        Dim tache As New Tache
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE patient_Id = @patientId" &
                " And (etat = @etat1 Or etat = @etat2)" &
                " And parcours_id = @parcoursId" &
                " And categorie = @categorie" &
                " And ([type] = @type1 Or [type] = @type2)" &
                " And (nature = @nature1 Or nature = @nature2)" &
                " ORDER BY date_rendez_vous DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@parcoursId", parcoursId)
                .AddWithValue("@etat1", Tache.EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@etat2", Tache.EtatTache.EN_COURS.ToString)
                .AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                .AddWithValue("@type1", Tache.TypeTache.RDV.ToString)
                .AddWithValue("@type2", Tache.TypeTache.RDV_SPECIALISTE.ToString)
                .AddWithValue("@nature1", Tache.NatureTache.RDV.ToString)
                .AddWithValue("@nature2", Tache.NatureTache.RDV_SPECIALISTE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = BuildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return tache
    End Function

    Public Function GetProchainRendezVousOasisByPatientIdEtFonctionId(patientId As Long, fonctionId As Long) As Tache
        Dim dateRendezVous As Date = Nothing
        Dim tache As New Tache
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * " &
                " FROM oasis.oasis.oa_tache" &
                " LEFT JOIN oasis.oa_r_fonction F on F.oa_r_fonction_id = destinataire_fonction_id" &
                " WHERE patient_Id = @patientId" &
                " AND etat = @etat" &
                " AND categorie = @categorie" &
                " AND [type] = @type" &
                " AND nature = @nature" &
                " AND destinataire_fonction_id = @fonctionId" &
                " ORDER BY date_rendez_vous DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@etat", Tache.EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                .AddWithValue("@type", Tache.TypeTache.RDV.ToString)
                .AddWithValue("@nature", Tache.NatureTache.RDV.ToString)
                .AddWithValue("@fonctionId", fonctionId)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = BuildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return tache
    End Function

    Public Function GetProchaineDemandeRendezVousByPatientId(patientId As Long, parcoursId As Long) As Tache
        Dim tache As New Tache
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache" &
                " WHERE patient_Id = @patientId" &
                " And (etat = @etat1 Or etat = @etat2)" &
                " And parcours_id = @parcoursId" &
                " And categorie = @categorie" &
                " And [type] = @type" &
                " And nature = @nature" &
                " ORDER BY date_rendez_vous DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@parcoursId", parcoursId)
                .AddWithValue("@etat1", Tache.EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@etat2", Tache.EtatTache.EN_COURS.ToString)
                .AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                .AddWithValue("@type", Tache.TypeTache.RDV_DEMANDE.ToString)
                .AddWithValue("@nature", Tache.NatureTache.RDV_DEMANDE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = BuildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try


        Return tache
    End Function

    Public Function AttribueTacheToUserLog(idTache As Long, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim isOK As Boolean = True
        Dim con As SqlConnection = Nothing
        Dim nbUpdate As Integer

        Try
            con = GetConnection()

            Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " etat = @etat, traite_user_id = @traiteUserId, horodate_attrib = @dateAttrib" &
            " WHERE id = @Id" &
            " AND etat = @etatWhere" &
            " AND (traite_user_id Is null OR traite_user_id = 0)"

            Dim cmd As New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@etat", Tache.EtatTache.EN_COURS.ToString)
                .AddWithValue("@traiteUserId", userLog.UtilisateurId)
                .AddWithValue("@dateAttrib", Date.Now())
                .AddWithValue("@Id", idTache)
                .AddWithValue("@etatWhere", Tache.EtatTache.EN_ATTENTE.ToString)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Tâche déjà traitée par un autre utilisateur !")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
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
    Public Function DesattribueTache(idTache As Long) As Boolean
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
                .AddWithValue("@etat", Tache.EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@traiteUserId", DBNull.Value)
                .AddWithValue("@dateAttrib", DBNull.Value)
                .AddWithValue("@Id", idTache)
                .AddWithValue("@etatWhere", Tache.EtatTache.EN_COURS.ToString)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Tâche déjà traitée par un autre utilisateur !")
            End If
        Catch ex As Exception
            'Throw New Exception(ex.Message)
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

    Private Function BuildBean(reader As SqlDataReader) As Tache
        Dim tache As New Tache With {
            .Id = reader("id"),
            .ParentId = Coalesce(reader("parent_id"), 0),
            .EmetteurUserId = Coalesce(reader("emetteur_user_id"), 0),
            .EmetteurFonctionId = Coalesce(reader("emetteur_fonction_id"), 0),
            .UniteSanitaireId = Coalesce(reader("unite_sanitaire_id"), 0),
            .SiteId = Coalesce(reader("site_id"), 0),
            .PatientId = Coalesce(reader("patient_id"), 0),
            .ParcoursId = Coalesce(reader("parcours_id"), 0),
            .EpisodeId = Coalesce(reader("episode_id"), 0),
            .SousEpisodeId = Coalesce(reader("sous_episode_id"), 0),
            .TraiteUserId = Coalesce(reader("traite_user_id"), 0),
            .TraiteFonctionId = Coalesce(reader("traite_fonction_id"), 0),
            .DestinataireFonctionId = Coalesce(reader("destinataire_fonction_id"), 0),
            .Priorite = Coalesce(reader("priorite"), 0),
            .OrdreAffichage = Coalesce(reader("ordre_affichage"), 0),
            .Categorie = Coalesce(reader("categorie"), ""),
            .Type = Coalesce(reader("type"), ""),
            .Nature = Coalesce(reader("nature"), ""),
            .Duree = Coalesce(reader("duree_mn"), 0),
            .EmetteurCommentaire = Coalesce(reader("emetteur_commentaire"), ""),
            .HorodatageCreation = Coalesce(reader("horodate_creation"), Nothing),
            .HorodatageAttribution = Coalesce(reader("horodate_attrib"), Nothing),
            .HorodatageCloture = Coalesce(reader("horodate_cloture"), Nothing),
            .Etat = Coalesce(reader("etat"), ""),
            .Cloture = Coalesce(reader("cloture"), False),
            .TypedemandeRendezVous = Coalesce(reader("type_demande_rendez_vous"), ""),
            .DateRendezVous = Coalesce(reader("date_rendez_vous"), Nothing),
            .DateTraitementDemandeRendezVous = Coalesce(reader("date_traitement_demande_rendez_vous"), Nothing)
        }
        Return tache
    End Function

    Public Function CreateTache(tache As Tache, Optional ifExist As String = "", userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            ' --- test si update tache parent todo
            If tache.ParentId <> 0 Then
                ClosTache(con, tache.ParentId, Tache.EtatTache.TERMINEE, False, transaction, userLog)
            End If

            Dim SQLstring As String = "INSERT INTO oasis.oa_tache " &
                    "(parent_id, emetteur_user_id, emetteur_fonction_id, unite_sanitaire_id, site_id, patient_id, parcours_id, episode_id," &
                    " sous_episode_id, traite_user_id, traite_fonction_id, destinataire_fonction_id, priorite, ordre_affichage, categorie, type," &
                    " nature, duree_mn, emetteur_commentaire, horodate_creation, etat, cloture, type_demande_rendez_vous, date_rendez_vous, date_traitement_demande_rendez_vous)" &
            " VALUES (@parentId, @emetteurId, @emetteurFonctionId, @uniteSanitaireId, @siteId, @patientId, @parcoursId, @episodeId," &
                    " @sousEpisodeId, @traiteUserId, @traiteFonctionId, @destinataireFonctionId, @priorite, @ordreAffichage, @categorie, @type," &
                    " @nature, @duree, @commentaire, @dateCreation, @etat, @cloture, @typedemandeRendezVous, @dateRendezVous, @dateTraitementDemandeRendezVous)"

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
                .AddWithValue("@dateTraitementDemandeRendezVous", If(tache.DateTraitementDemandeRendezVous = Nothing, DBNull.Value, tache.DateTraitementDemandeRendezVous))
            End With

            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function CreateRendezVous(tache As Tache, userLog As Utilisateur) As Boolean
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF Not EXISTS (SELECT 1 FROM oasis.oa_tache" & vbCrLf &
        " WHERE patient_id = " & tache.PatientId & vbCrLf &
        " And parcours_id = " & tache.ParcoursId & vbCrLf &
        " And (type = '" & Tache.TypeTache.RDV.ToString &
            "' OR (type = '" & Tache.TypeTache.RDV_SPECIALISTE.ToString &
            "' OR (type = '" & Tache.TypeTache.RDV_DEMANDE.ToString & "')" & vbCrLf &
        " AND (etat = '" & Tache.EtatTache.EN_COURS.ToString & "' OR etat = '" & Tache.EtatTache.EN_ATTENTE.ToString & "'))"

        'Console.WriteLine(SQLstring)

        Try
            CreateTache(tache, SQLstring, userLog)
        Catch ex As Exception
            MsgBox(ex.Message)
            codeRetour = False
        End Try

        Return codeRetour
    End Function

    Public Function CreationAutomatiqueDeDemandeRendezVous(Patient As PatientBase, parcours As Parcours, dateDebut As Date, Optional PremierRDV As Boolean = False, userLog As Utilisateur) As Boolean
        'Calcul de la période (année, mois) du rendez-vous demandé
        Dim Commentaire As String = ""
        Dim Rythme As Integer = parcours.Rythme
        Dim Base As String = parcours.Base

        'Si le rythme n'est pas renseigné dans ce cas on ne peut pas générer automatiquement la demande de rendez-vous
        If Base = ParcoursDao.EnumParcoursBaseCode.TousLes2Ans Or
                Base = ParcoursDao.EnumParcoursBaseCode.TousLes3Ans Or
                Base = ParcoursDao.EnumParcoursBaseCode.TousLes4Ans Or
                Base = ParcoursDao.EnumParcoursBaseCode.TousLes5Ans Then
        Else
            If Rythme = 0 Then
                Return False
            End If
        End If

        Dim Jour As Integer
        Select Case Base
            Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                Jour = 1
            Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                Select Case Rythme
                    Case 1
                        Jour = 7
                    Case 2
                        Jour = 3
                    Case 3
                        Jour = 2
                    Case 4
                        Jour = 2
                    Case Else
                        Jour = 2
                End Select
            Case ParcoursDao.EnumParcoursBaseCode.ParMois
                Select Case Rythme
                    Case 1
                        Jour = 30
                    Case 2
                        Jour = 15
                    Case 3
                        Jour = 10
                    Case 4
                        Jour = 8
                    Case 5
                        Jour = 6
                    Case 6
                        Jour = 5
                    Case 7
                        Jour = 4
                    Case 8
                        Jour = 4
                    Case 9
                        Jour = 3
                    Case Else
                        Jour = 3
                End Select
            Case ParcoursDao.EnumParcoursBaseCode.ParAn
                Select Case Rythme
                    Case 1
                        Jour = 365
                    Case 2
                        Jour = 182
                    Case 3
                        Jour = 121
                    Case 4
                        Jour = 91
                    Case 5
                        Jour = 73
                    Case 6
                        Jour = 61
                    Case 7
                        Jour = 52
                    Case 8
                        Jour = 45
                    Case 9
                        Jour = 40
                    Case 10
                        Jour = 36
                    Case 11
                        Jour = 33
                    Case 12
                        Jour = 30
                    Case 13
                        Jour = 30
                    Case Else
                        Jour = 30
                End Select
            Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                Jour = 730
            Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                Jour = 1095
            Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                Jour = 1460
            Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                Jour = 1825
            Case Else
                'TODO: CreateLog("Base de calcul des demandes de rendez-vous inconnue pour le parcours " & parcours.Id & " du patient " & Patient.PatientId, "CreateDemandeRendezVous", LogDao.EnumTypeLog.ERREUR.ToString)
                Throw New Exception("Base de calcul de la demande de rendez-vous inconnue pour l'intervenant, la demande de rendez-vous a été créée avec un délai par défaut de 30 jours !")
                Commentaire = "Base de calcul de la demande de rendez-vous inconnue pour l'intervenant, la demande de rendez-vous a été créée avec un délai par défaut de 30 jours !"
                Jour = 30
        End Select

        'Récupérer la spécialité pour obtenir le délai de prise en charge de la spécialité
        Dim specialite As Specialite = Table_specialite.GetSpecialiteById(parcours.SpecialiteId)
        Dim DelaiPriseEnCharge As Integer = specialite.DelaiPriseEnCharge

        'Si la date du dernier rendez-vous est < date du jour, prendre la date du dernier rendez-vous, sinon prendre la date du jour
        Dim DateRendezVousCalcul As Date
        If PremierRDV = False Then
            DateRendezVousCalcul = dateDebut.AddDays(Jour)
        Else
            DateRendezVousCalcul = Date.Now().AddDays(30)
        End If

        'Convertir la date en année et mois
        Dim DateRendezVous As New Date(DateRendezVousCalcul.Year, DateRendezVousCalcul.Month, 1, 0, 0, 0)

        'Date traitement de la demande de rendez-vous
        Dim DateTraitementDemandeRendezVous As Date
        DateTraitementDemandeRendezVous = DateRendezVous.AddDays(-DelaiPriseEnCharge)
        If DateTraitementDemandeRendezVous.Date < Date.Now.Date Then
            DateTraitementDemandeRendezVous = Date.Now
        End If

        'Récupération de l'utilisateur 'AUTO' qui sera déclaré comme utilisateur émetteur
        Dim UserAutoId As Long
        Dim UserAutoIdString As String = ConfigurationManager.AppSettings("IdUserAuto")
        If IsNumeric(UserAutoIdString) Then
            UserAutoId = CInt(UserAutoIdString)
        Else
            'TODO: CreateLog("Paramètre application 'IdUserAuto' non trouvé !", "CreateDemandeRendezVous", LogDao.EnumTypeLog.ERREUR.ToString)
            UserAutoId = 1
        End If

        'Récupération de la fonction emetteur par défaut
        Dim FonctionEmetteurAutoId As Long
        Dim FonctionEmetteurAutoIdString As String = ConfigurationManager.AppSettings("FonctionEmetteurAutoId")
        If IsNumeric(FonctionEmetteurAutoIdString) Then
            FonctionEmetteurAutoId = CInt(FonctionEmetteurAutoIdString)
        Else
            'TODO: CreateLog("Paramètre application 'FonctionEmetteurAutoId' non trouvé !", "CreateDemandeRendezVous", LogDao.EnumTypeLog.ERREUR.ToString)
            FonctionEmetteurAutoId = 14
        End If

        'Récupération de la duréee par défaut
        Dim DureeRendezVousParDefaut As Integer
        Dim DureeRendezVousParDefautString As String = ConfigurationManager.AppSettings("DureeRendezVousParDefaut")
        If IsNumeric(DureeRendezVousParDefautString) Then
            DureeRendezVousParDefaut = CInt(DureeRendezVousParDefautString)
        Else
            'TODO: CreateLog("Paramètre application 'DureeRendezVousParDefaut' non trouvé !", "CreateDemandeRendezVous", LogDao.EnumTypeLog.ERREUR.ToString)
            DureeRendezVousParDefaut = 15
        End If

        'Déterminer la fonction destinataire et la fonction qui doit traiter
        Dim DestinataireFonctionId As Long
        Dim TraiteFonctionId As Long
        Select Case parcours.SousCategorieId
            Case EnumSousCategoriePPS.medecinReferent
                DestinataireFonctionId = FonctionDao.EnumFonction.MEDECIN
                TraiteFonctionId = FonctionDao.EnumFonction.MEDECIN
            Case EnumSousCategoriePPS.IDE
                DestinataireFonctionId = FonctionDao.EnumFonction.IDE
                TraiteFonctionId = FonctionDao.EnumFonction.IDE
            Case EnumSousCategoriePPS.sageFemme
                If parcours.SpecialiteId = EnumSpecialiteOasis.sageFemmeOasis Then
                    DestinataireFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                    TraiteFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                Else
                    DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                    TraiteFonctionId = FonctionDao.EnumFonction.IDE
                End If
            Case EnumSousCategoriePPS.specialiste
                DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                TraiteFonctionId = FonctionDao.EnumFonction.IDE
            Case Else
                DestinataireFonctionId = FonctionDao.EnumFonction.INCONNU
                TraiteFonctionId = FonctionDao.EnumFonction.IDE
        End Select

        'Alimentation du bean Tache
        Dim tache As New Tache
        tache.ParentId = 0
        tache.EmetteurUserId = UserAutoId 'auto
        tache.EmetteurFonctionId = FonctionEmetteurAutoId
        tache.UniteSanitaireId = Patient.PatientUniteSanitaireId
        tache.SiteId = Patient.PatientSiteId
        tache.PatientId = Patient.PatientId
        tache.ParcoursId = parcours.Id
        tache.EpisodeId = 0
        tache.SousEpisodeId = 0
        tache.TraiteUserId = 0
        tache.TraiteFonctionId = TraiteFonctionId
        tache.DestinataireFonctionId = DestinataireFonctionId
        tache.Priorite = Tache.EnumPriorite.BASSE
        tache.OrdreAffichage = 20
        tache.Categorie = Tache.CategorieTache.SOIN.ToString
        tache.Type = Tache.TypeTache.RDV_DEMANDE.ToString()
        tache.Nature = Tache.NatureTache.RDV_DEMANDE.ToString
        tache.Duree = DureeRendezVousParDefaut
        tache.EmetteurCommentaire = Commentaire
        tache.HorodatageCreation = Date.Now()
        tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString
        tache.TypedemandeRendezVous = Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString
        tache.DateRendezVous = DateRendezVous
        tache.DateTraitementDemandeRendezVous = DateTraitementDemandeRendezVous

        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        'A un instant donné, il ne peut y avoir qu'une demande de rendez-vous ou qu'un rendez-vous en cours ou en attente, pour un patient et un intervenant donné
        '--- Contrôler qu'il n'existe pas de demande de rendez-vous ou de rendez-vous en cours ou en attente, avant de créer la nouvelle demande de rendez-vous
        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT 1 FROM oasis.oa_tache" & vbCrLf &
                " WHERE patient_id = " & tache.PatientId & vbCrLf &
                " AND parcours_id = " & tache.ParcoursId & vbCrLf &
                " AND (type = '" & Tache.TypeTache.RDV_DEMANDE.ToString & "' OR type = '" & Tache.TypeTache.RDV_SPECIALISTE.ToString & "' OR type = '" & Tache.TypeTache.RDV.ToString & "')" & vbCrLf &
                " AND (etat = '" & Tache.EtatTache.EN_COURS.ToString & "' OR etat = '" & Tache.EtatTache.EN_ATTENTE.ToString & "')"

            'Console.WriteLine(SQLstring)

            With command.Parameters
                '.AddWithValue("@patientId", Patient.patientId)
                '.AddWithValue("@parcoursId", parcoursId)
                '.AddWithValue("@etat", EtatTache.TERMINEE.ToString)
                '.AddWithValue("@categorie", CategorieTache.SOIN.ToString)
                '.AddWithValue("@type", TypeTache.RDV.ToString)
                '.AddWithValue("@type2", TypeTache.RDV_SPECIALISTE.ToString)
                '.AddWithValue("@nature", NatureTache.RDV.ToString)
                '.AddWithValue("@nature2", NatureTache.RDV_SPECIALISTE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    codeRetour = False
                Else
                    Try
                        If CreateTache(tache,, userLog) = True Then
                            codeRetour = True
                        End If
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        codeRetour = False
                    End Try
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function CreationDemandeAvis(tache As Tache) As Boolean
        Dim nbCreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_tache" &
        " WHERE patient_id = @patientId" &
        " AND episode_id = @episodeId" &
        " AND type = '" & Tache.TypeTache.AVIS_EPISODE.ToString & "'" &
        " AND (etat = '" & Tache.EtatTache.EN_COURS.ToString & "' OR etat = '" & Tache.EtatTache.EN_ATTENTE.ToString & "'))" &
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
            Throw New Exception(ex.Message)
            CreateLog(ex.ToString, "TacheDao", LogDao.EnumTypeLog.ERREUR.ToString)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function AnnulationTache(tache As Tache, userLog As Utilisateur) As Boolean
        Return AnnulationTache(tache.Id, userLog)
    End Function

    Public Function AnnulationTache(idTache As Long, userLog As Utilisateur) As Boolean
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Try
            ClosTache(con, idTache, Tache.EtatTache.ANNULEE, True,, userLog)

        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function
    Public Sub ClosTache(con As SqlConnection, idTache As Long, etatFinal As Tache.EtatTache, cloture As Boolean, Optional transaction As SqlTransaction = Nothing, userLog As Utilisateur)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim nbUpdate As Integer

        Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " etat = @etat, traite_user_id = @traiteUserId, horodate_cloture = @dateCloture, cloture = @Cloture " &
            " WHERE id = @Id AND (traite_user_id is null OR traite_user_id = @traiteUserId2) AND etat <> @etat2"

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

        Try
            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Tâche déjà traitée par un autre utilisateur !")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Sub


    Public Function ClotureTache(idTache As Long, cloture As Boolean, userLog As Utilisateur) As Boolean
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Try
            ClosTache(con, idTache, Tache.EtatTache.TERMINEE, cloture, , userLog)
            codeRetour = True
        Catch ex As Exception
            Throw New Exception(ex.Message)
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
    Public Function GetTacheBeanAssocie(tache As Tache) As TacheBeanAssocie
        Dim userDao As New UserDao
        Dim fonctionDao As New FonctionDao
        Dim uniteSanitaireDao As New UniteSanitaireDao
        Dim siteDao As New SiteDao
        Dim parcoursDao As New ParcoursDao
        Dim episodeDao As New EpisodeDao
        Dim rorDao As New RorDao
        Dim patientDao As New PatientDao

        Dim tacheBeanAssocie = New TacheBeanAssocie
        tacheBeanAssocie.UserEmetteur = userDao.getUserById(tache.EmetteurUserId)
        If tache.EmetteurFonctionId <> 0 Then tacheBeanAssocie.FonctionEmetteur = fonctionDao.GetFonctionById(tache.EmetteurFonctionId)
        tacheBeanAssocie.UniteSanitaire = uniteSanitaireDao.getUniteSanitaireById(tache.UniteSanitaireId, True)
        tacheBeanAssocie.Site = siteDao.getSiteById(tache.SiteId, True)
        tacheBeanAssocie.Patient = patientDao.GetPatientById(tache.PatientId)
        If tache.ParcoursId <> 0 Then
            tacheBeanAssocie.Parcours = parcoursDao.GetParcoursById(tache.ParcoursId)
            If tacheBeanAssocie.Parcours.SpecialiteId <> 0 Then
                tacheBeanAssocie.Specialite = Table_specialite.GetSpecialiteById(tacheBeanAssocie.Parcours.SpecialiteId)
                If tacheBeanAssocie.Specialite.Oasis = False Then
                    ' -- on recherche le nom de l'intervenant
                    Dim ror As Ror = rorDao.getRorById(tacheBeanAssocie.Parcours.RorId)
                    tacheBeanAssocie.Intervenant = ror.Nom
                End If
            End If
        End If
        If tache.TraiteFonctionId Then tacheBeanAssocie.FonctionTraiteur = fonctionDao.GetFonctionById(tache.TraiteFonctionId)
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
                .AddWithValue("@etat1", Tache.EtatTache.EN_ATTENTE.ToString)
                .AddWithValue("@etat2", Tache.EtatTache.EN_COURS.ToString)
                .AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                .AddWithValue("@type", Tache.TypeTache.AVIS_EPISODE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    tache = BuildBean(reader)
                Else
                    tache.Id = 0
                    tache.DateRendezVous = Nothing
                    tache.TypedemandeRendezVous = Tache.TypeTache.AVIS_EPISODE.ToString
                    tache.Nature = ""
                    tache.Etat = ""
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return tache
    End Function


    Public Function ExisteDemandeAvisMedicalByEpisode(episodeId As Long) As Boolean
        Dim con As SqlConnection
        Dim CodeRetour As Boolean = False
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_tache T" &
                " LEFT JOIN  oasis.oa_r_fonction E ON E.oa_r_fonction_id = T.emetteur_fonction_id " & vbCrLf &
                " LEFT JOIN  oasis.oa_r_fonction D ON D.oa_r_fonction_id = T.destinataire_fonction_id " & vbCrLf &
                " WHERE episode_id = @episodeId" &
                " AND categorie = @categorie" &
                " AND [type] = @type" &
                " AND (parent_id is Null or parent_id = 0)" &
                " AND E.oa_r_fonction_type = @typeEmetteur" &
                " AND D.oa_r_fonction_type = @typeDestinataire" &
                " AND T.etat <> @etat"

            With command.Parameters
                .AddWithValue("@episodeId", episodeId)
                .AddWithValue("@typeEmetteur", ProfilDao.EnumProfilType.PARAMEDICAL.ToString)
                .AddWithValue("@typeDestinataire", ProfilDao.EnumProfilType.MEDICAL.ToString)
                .AddWithValue("@categorie", Tache.CategorieTache.SOIN.ToString)
                .AddWithValue("@type", Tache.TypeTache.AVIS_EPISODE.ToString)
                .AddWithValue("@etat", Tache.EtatTache.ANNULEE.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    CodeRetour = True
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

    ''' <summary>
    ''' Creation de rendez : insert simple ou on termine la demande (update) et insert du rdv
    ''' </summary>
    ''' <param name="patient"></param>
    ''' <param name="parcours"></param>
    ''' <param name="tacheParent"></param>
    Public Sub CreateRendezVous(patient As PatientBase, parcours As Parcours, typeTache As Tache.TypeTache, dateRDV As Date, duree As Integer, commentaire As String, Optional tacheParent As Tache = Nothing, userLog As Utilisateur)
        Dim tache As Tache = New Tache()

        If typeTache <> typeTache.RDV_MISSION AndAlso typeTache <> typeTache.RDV AndAlso typeTache <> typeTache.RDV_SPECIALISTE Then
            Throw New Exception("Pas de rendez-vous possible sur ce type de tache !")
        End If

        ' --- parent id
        If Not IsNothing(tacheParent) Then
            If (tacheParent.Type = typeTache.MISSION_DEMANDE.ToString() AndAlso typeTache = typeTache.RDV_MISSION) _
                    OrElse (tacheParent.Type = typeTache.RDV_DEMANDE.ToString() AndAlso typeTache = typeTache.RDV) Then
                tache.ParentId = tacheParent.Id
            Else
                Throw New Exception("Pas de rendez-vous possible sur ce type de tache parent !")
            End If
        Else
            tache.ParentId = 0
        End If

        ' --- on set emetteurFonctionId, traiteFonctionId et destinataireFonctionid
        SetFonctionsIdFromUserLogAnParcours(tache, parcours, tacheParent, userLog)
        ' --- set du reste 
        tache.UniteSanitaireId = patient.PatientUniteSanitaireId
        tache.SiteId = patient.PatientSiteId
        tache.PatientId = patient.PatientId
        tache.ParcoursId = parcours.Id
        tache.EpisodeId = 0
        tache.SousEpisodeId = 0
        tache.TraiteUserId = 0
        tache.Priorite = Tache.EnumPriorite.BASSE
        tache.OrdreAffichage = 20
        tache.Categorie = Tache.CategorieTache.SOIN.ToString
        tache.Type = typeTache.ToString
        tache.Nature = tache.Type  ' nature = type en string sur ce type de tache
        tache.Duree = duree
        tache.EmetteurCommentaire = commentaire
        tache.HorodatageCreation = Date.Now()
        tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString
        'tache.TypedemandeRendezVous =     ' => pas sur ce type de tache
        tache.DateRendezVous = dateRDV

        CreateTache(tache,, userLog)

    End Sub

    Public Function ModificationDemandeRendezVous(tache As Tache) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim isOK As Boolean = True
        Dim con As SqlConnection = Nothing
        Dim nbUpdate As Integer

        Try
            con = GetConnection()

            Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " type_demande_rendez_vous = @typeDemandeRendezVous, date_rendez_vous = @dateRendezVous, emetteur_commentaire = @commentaire" &
            " WHERE id = @Id And etat = @etat"

            Dim cmd As New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@Id", tache.Id)
                .AddWithValue("@typeDemandeRendezVous", tache.TypedemandeRendezVous)
                .AddWithValue("@dateRendezVous", tache.DateRendezVous)
                .AddWithValue("@commentaire", tache.EmetteurCommentaire)
                .AddWithValue("@etat", Tache.EtatTache.EN_COURS.ToString)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Annulation de la modification, la tâche n'est plus disponible !")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
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

    Public Function ModificationRendezVous(tache As Tache, etatActuel As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim isOK As Boolean = True
        Dim con As SqlConnection = Nothing
        Dim nbUpdate As Integer

        Try
            con = GetConnection()

            Dim SQLstring As String = "UPDATE oasis.oa_tache SET" &
            " date_rendez_vous = @dateRendezVous, emetteur_commentaire = @commentaire" &
            " WHERE id = @Id AND etat = @etatActuel"

            Dim cmd As New SqlCommand(SQLstring, con)
            With cmd.Parameters
                .AddWithValue("@Id", tache.Id)
                .AddWithValue("@dateRendezVous", tache.DateRendezVous)
                .AddWithValue("@commentaire", tache.EmetteurCommentaire)
                .AddWithValue("@etat", Tache.EtatTache.EN_ATTENTE.ToString())
                .AddWithValue("@etatActuel", etatActuel)
            End With

            da.UpdateCommand = cmd
            nbUpdate = da.UpdateCommand.ExecuteNonQuery()
            If nbUpdate <= 0 Then
                Throw New Exception("Collision , Annulation de la modification, la tâche n'est plus disponible !")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
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

    Private Sub SetFonctionsIdFromUserLogAnParcours(tache As Tache, parcours As Parcours, Optional tacheParent As Tache = Nothing, userLog As Utilisateur)

        With tache
            ' -- emetteur ID
            .EmetteurUserId = userLog.UtilisateurId
            ' -- emetteur fonction
            Select Case userLog.UtilisateurProfilId.Trim()
                Case "IDE"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.IDE
                Case "IDE_REMPLACANT"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.IDE_REMPLACANT
                Case "MEDECIN"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.MEDECIN
                Case "SAGE_FEMME"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                Case "CADRE_SANTE"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.CADRE_SANTE
                Case "SECRETAIRE_MEDICALE"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.SECRETAIRE_MEDICALE
                Case "ADMINISTRATIF"
                    .EmetteurFonctionId = FonctionDao.EnumFonction.ADMINISTRATIF
                Case Else
                    .EmetteurFonctionId = FonctionDao.EnumFonction.INCONNU
            End Select

            ' --- parcours
            Select Case parcours.SousCategorieId
                Case EnumSousCategoriePPS.medecinReferent
                    .DestinataireFonctionId = FonctionDao.EnumFonction.MEDECIN
                    .TraiteFonctionId = FonctionDao.EnumFonction.MEDECIN
                Case EnumSousCategoriePPS.IDE
                    .DestinataireFonctionId = FonctionDao.EnumFonction.IDE
                    .TraiteFonctionId = FonctionDao.EnumFonction.IDE
                Case EnumSousCategoriePPS.sageFemme
                    If parcours.SpecialiteId = EnumSpecialiteOasis.sageFemmeOasis Then
                        .DestinataireFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                        .TraiteFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                    Else
                        .DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                        .TraiteFonctionId = FonctionDao.EnumFonction.IDE
                    End If
                Case EnumSousCategoriePPS.specialiste
                    .DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                    .TraiteFonctionId = FonctionDao.EnumFonction.IDE
                Case Else
                    .DestinataireFonctionId = FonctionDao.EnumFonction.INCONNU
                    .TraiteFonctionId = FonctionDao.EnumFonction.IDE
            End Select
        End With

    End Sub

    Public Function GetLastDemandeRendezVousByPatient(patientId As Long) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim LastDrcId As Integer = 0
        Dim SQLstring As String
        Dim con As SqlConnection
        con = GetConnection()

        'Récupération de l'identifiant de la dernière DRC créée
        Dim dr As SqlDataReader
        SQLstring = "SELECT MAX(id) FROM oasis.oasis.oa_tache WHERE patient_id = @patientId AND ([type] = @type)"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", patientId)
            .AddWithValue("@type", Tache.TypeTache.RDV_DEMANDE.ToString)
        End With

        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            dr.Read()
            'Récupération de la clé de l'enregistrement créé
            LastDrcId = dr(0)
            'Libération des ressources d'accès aux données
            con.Close()
            cmd.Dispose()
        End If

        Return LastDrcId
    End Function

    Public Function TestMultipleSelect() As List(Of DataTable)

        Dim requete = "SELECT TOP 100 oa_patient_nir " &
                      "FROM oasis.oa_patient; " &
                      "SELECT TOP 100 oa_utilisateur_nom, oa_utilisateur_prenom " &
                      "FROM oasis.oa_utilisateur "
        'Dim indexRequete As Integer
        Dim listTablesLues As New List(Of DataTable)

        Using con As SqlConnection = GetConnection()

            Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using dataAdapter
                dataAdapter.SelectCommand = New SqlCommand(requete, con)
                Using dataSet As DataSet = New DataSet()
                    dataAdapter.Fill(dataSet)
                    For Each dataTable As DataTable In dataSet.Tables
                        Using dataTable
                            Try
                                dataAdapter.Fill(dataTable)   ' on garnit la réponse de ce dataTable
                                listTablesLues.Add(dataTable) ' ajout de la dataTable lue à notre resultset
                            Catch ex As Exception
                                Throw ex
                            End Try
                        End Using
                    Next
                End Using
            End Using
        End Using

        Return listTablesLues
    End Function

    Public Function SetTacheEmetteurEtDestinatiareBySpecialiteEtSousCategorie(SpecialiteId As Long, SousCategorieId As Long, userLog As Utilisateur) As TacheEmetteurEtDestinataire
        Dim tacheEmetteurEtDestinataire As New TacheEmetteurEtDestinataire
        tacheEmetteurEtDestinataire.DestinataireFonctionId = 0
        tacheEmetteurEtDestinataire.EmetteurFonctionId = 0
        tacheEmetteurEtDestinataire.TraiteFonctionId = 0

        Select Case userLog.UtilisateurProfilId.Trim()
            Case "IDE"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.IDE
            Case "IDE_REMPLACANT"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.IDE_REMPLACANT
            Case "MEDECIN"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.MEDECIN
            Case "SAGE_FEMME"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
            Case "CADRE_SANTE"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.CADRE_SANTE
            Case "SECRETAIRE_MEDICALE"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.SECRETAIRE_MEDICALE
            Case "ADMINISTRATIF"
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.ADMINISTRATIF
            Case Else
                tacheEmetteurEtDestinataire.EmetteurFonctionId = FonctionDao.EnumFonction.INCONNU
        End Select

        Select Case SousCategorieId
            Case EnumSousCategoriePPS.medecinReferent
                tacheEmetteurEtDestinataire.DestinataireFonctionId = FonctionDao.EnumFonction.MEDECIN
                tacheEmetteurEtDestinataire.TraiteFonctionId = FonctionDao.EnumFonction.MEDECIN
            Case EnumSousCategoriePPS.IDE
                tacheEmetteurEtDestinataire.DestinataireFonctionId = FonctionDao.EnumFonction.IDE
                tacheEmetteurEtDestinataire.TraiteFonctionId = FonctionDao.EnumFonction.IDE
            Case EnumSousCategoriePPS.sageFemme
                If SpecialiteId = EnumSpecialiteOasis.sageFemmeOasis Then
                    tacheEmetteurEtDestinataire.DestinataireFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                    tacheEmetteurEtDestinataire.TraiteFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                Else
                    tacheEmetteurEtDestinataire.DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                    tacheEmetteurEtDestinataire.TraiteFonctionId = FonctionDao.EnumFonction.IDE
                End If
            Case EnumSousCategoriePPS.specialiste
                tacheEmetteurEtDestinataire.DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                tacheEmetteurEtDestinataire.TraiteFonctionId = FonctionDao.EnumFonction.IDE
            Case Else
                tacheEmetteurEtDestinataire.DestinataireFonctionId = FonctionDao.EnumFonction.INCONNU
                tacheEmetteurEtDestinataire.TraiteFonctionId = FonctionDao.EnumFonction.IDE
        End Select

        Return tacheEmetteurEtDestinataire
    End Function

End Class
