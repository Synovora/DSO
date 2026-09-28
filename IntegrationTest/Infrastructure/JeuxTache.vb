Imports Oasis_Common

''' <summary>
''' Tâches de test (oa_tache) et ce dont elles ont besoin autour : parcours de
''' soins, filtres par unité sanitaire et site. Les intervenants du ROR viennent de
''' JeuxParcours.CreerRorParcours.
'''
''' Les tâches passent par TacheDao.CreateTache, l'INSERT que le client lourd
''' exécute pour toutes les tâches (demande d'avis, réponse, rendez-vous). Les
''' changements d'état passent par les méthodes du DAO dans les tests eux-mêmes :
''' ce sont elles qu'on éprouve. Les unités sanitaires et les sites viennent de
''' JeuxStructure (CreerUniteSanitaire, CreerSite).
'''
''' Les spécialités 9701 et 9702 viennent de Schema/27-reference-tache.sql : le
''' singleton Table_specialite les lit une fois pour tout le processus.
''' </summary>
Public Module JeuxTache

    ' --- Référentiel posé par 27-reference-tache.sql -------------------------------

    ''' <summary>Spécialité hors Oasis, délai de prise en charge de 400 jours.</summary>
    Public Const SpecialiteTacheNonOasis As Integer = 9701
    ''' <summary>Délai de prise en charge de la spécialité 9701, en jours.</summary>
    Public Const DelaiSpecialiteTacheNonOasis As Integer = 400
    ''' <summary>
    ''' Spécialité Oasis, délai à 0 : Table_specialite lui applique alors le délai
    ''' par défaut (appSetting SpecialiteDelaiPriseEnCharge, 30 jours en son absence).
    ''' </summary>
    Public Const SpecialiteTacheOasis As Integer = 9702
    ''' <summary>Spécialité absente du référentiel : GetSpecialiteById rend un délai de 0.</summary>
    Public Const SpecialiteTacheAbsente As Integer = 9799

    ' --- Tâches ----------------------------------------------------------------------

    ''' <summary>
    ''' Tâche prête pour CreateTache, rien n'est écrit. Par défaut une demande d'avis
    ''' sur épisode (AVIS_EPISODE, nature DEMANDE), en attente, catégorie SOIN,
    ''' priorité basse, horodatée à l'instant. natureDeTache à Nothing reprend le type
    ''' pour les tâches de rendez-vous (comme CreateRendezVous) et DEMANDE sinon.
    ''' </summary>
    Function TacheDeTest(patientId As Long, emetteurId As Long,
                         Optional typeDeTache As Tache.TypeTache = Tache.TypeTache.AVIS_EPISODE,
                         Optional natureDeTache As Tache.NatureTache? = Nothing,
                         Optional etatDeTache As Tache.EtatTache = Tache.EtatTache.EN_ATTENTE,
                         Optional traiteFonctionId As Long = 0,
                         Optional emetteurFonctionId As Long = 0,
                         Optional destinataireFonctionId As Long = 0,
                         Optional traiteUserId As Long = 0,
                         Optional priorite As Integer = Tache.EnumPriorite.BASSE,
                         Optional ordreAffichage As Integer = 10,
                         Optional categorie As Tache.CategorieTache = Tache.CategorieTache.SOIN,
                         Optional episodeId As Long = 0,
                         Optional parcoursId As Long = 0,
                         Optional uniteSanitaireId As Long = 0,
                         Optional siteId As Long = 0,
                         Optional parentId As Long = 0,
                         Optional dateRendezVous As Date? = Nothing,
                         Optional horodatage As Date? = Nothing,
                         Optional commentaire As String = "Tache de test",
                         Optional duree As Integer = 0,
                         Optional typeDemandeRendezVous As String = "") As Tache
        Dim natureRetenue As String
        If natureDeTache.HasValue Then
            natureRetenue = natureDeTache.Value.ToString()
        ElseIf typeDeTache = Tache.TypeTache.AVIS_EPISODE OrElse typeDeTache = Tache.TypeTache.AVIS_SOUS_EPISODE Then
            natureRetenue = Tache.NatureTache.DEMANDE.ToString()
        Else
            natureRetenue = typeDeTache.ToString()
        End If
        Return New Tache With {
            .ParentId = parentId,
            .EmetteurUserId = emetteurId,
            .EmetteurFonctionId = emetteurFonctionId,
            .UniteSanitaireId = uniteSanitaireId,
            .SiteId = siteId,
            .PatientId = patientId,
            .ParcoursId = parcoursId,
            .EpisodeId = episodeId,
            .SousEpisodeId = 0,
            .TraiteUserId = traiteUserId,
            .TraiteFonctionId = traiteFonctionId,
            .DestinataireFonctionId = destinataireFonctionId,
            .Priorite = priorite,
            .OrdreAffichage = ordreAffichage,
            .Categorie = categorie.ToString(),
            .Type = typeDeTache.ToString(),
            .Nature = natureRetenue,
            .Duree = duree,
            .EmetteurCommentaire = commentaire,
            .HorodatageCreation = If(horodatage, Date.Now),
            .Etat = etatDeTache.ToString(),
            .Cloture = False,
            .TypedemandeRendezVous = typeDemandeRendezVous,
            .DateRendezVous = If(dateRendezVous.HasValue, dateRendezVous.Value, Nothing)
        }
    End Function

    ''' <summary>
    ''' Enregistre la tâche par TacheDao.CreateTache sous le compte courant et renvoie
    ''' l'id attribué (le plus grand : les tests s'exécutent l'un après l'autre).
    ''' auteurId est l'utilisateur connecté ; il ne compte que pour la clôture de la
    ''' tâche parente. 0 reprend l'émetteur.
    ''' </summary>
    Function EnregistrerTache(nouvelle As Tache, Optional auteurId As Long = 0) As Long
        Dim daoTache As New TacheDao
        Dim auteur = UtilisateurPourTache(If(auteurId = 0, nouvelle.EmetteurUserId, auteurId))
        daoTache.CreateTache(nouvelle, auteur)
        Return DerniereTache()
    End Function

    ''' <summary>Plus grand id de oa_tache, 0 si la table est vide.</summary>
    Function DerniereTache() As Long
        Dim valeur = Scalaire("SELECT MAX(id) FROM oasis.oa_tache")
        If valeur Is Nothing OrElse valeur Is DBNull.Value Then Return 0
        Return CLng(valeur)
    End Function

    ''' <summary>Nombre de lignes de oa_tache, sous le compte Admin.</summary>
    Function NombreDeTaches(Optional patientId As Long = 0) As Integer
        If patientId = 0 Then Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_tache"))
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_tache WHERE patient_id = @p0", patientId))
    End Function

    ''' <summary>Valeur brute d'une colonne de oa_tache, sous le compte Admin (DBNull.Value si NULL).</summary>
    Function ColonneTache(tacheId As Long, colonne As String) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_tache WHERE id = @p0", tacheId)
    End Function

    ''' <summary>
    ''' Utilisateur connecté tel que TacheDao le lit : id et profil (le profil donne
    ''' la fonction émettrice des rendez-vous). Rien n'est lu en base.
    ''' </summary>
    Function UtilisateurPourTache(utilisateurId As Long, Optional profilId As String = "IDE") As Utilisateur
        Return New Utilisateur With {
            .UtilisateurId = CInt(utilisateurId),
            .UtilisateurProfilId = profilId,
            .LstFonction = New List(Of Fonction)
        }
    End Function

    ''' <summary>
    ''' Change le nom et le prénom d'un utilisateur de test : CreerUtilisateur les pose
    ''' tous à TEST Utilisateur, et les listes de tâches joignent ces colonnes.
    ''' </summary>
    Sub RenommerUtilisateurPourTache(utilisateurId As Long, nom As String, prenom As String)
        Executer("UPDATE oasis.oa_utilisateur SET oa_utilisateur_nom = @p0, oa_utilisateur_prenom = @p1" &
                 " WHERE oa_utilisateur_id = @p2", nom, prenom, utilisateurId)
    End Sub

    ' --- Filtres ---------------------------------------------------------------------

    ''' <summary>
    ''' Unité sanitaire pour FiltreTache, avec les sites retenus. La liste des sites
    ''' est toujours créée : FiltreTache.GetListAllSite parcourt LstSite sans tester
    ''' Nothing, comme le fait le client lourd après UniteSanitaireDao.
    ''' </summary>
    Function UnitePourFiltreTache(uniteSanitaireId As Long, ParamArray siteIds() As Long) As UniteSanitaire
        Dim unite As New UniteSanitaire With {
            .Oa_unite_sanitaire_id = CInt(uniteSanitaireId),
            .Oa_unite_sanitaire_description = "Unite " & uniteSanitaireId,
            .LstSite = New List(Of Site)
        }
        For Each idSite In siteIds
            unite.AddSite(New Site With {.Oa_site_id = idSite, .Oa_site_description = "Site " & idSite})
        Next
        Return unite
    End Function

    ''' <summary>Filtre de tâches composé de ces unités (aucune : pas de filtre de structure).</summary>
    Function FiltreTacheDe(ParamArray unites() As UniteSanitaire) As FiltreTache
        Dim filtre As New FiltreTache
        For Each unite In unites
            filtre.AddUniteSanitaire(unite)
        Next
        Return filtre
    End Function

    ''' <summary>Liste de fonctions (seul l'id compte pour les clauses IN).</summary>
    Function FonctionsTache(ParamArray ids() As Long) As List(Of Fonction)
        Return ids.Select(Function(i) New Fonction With {.Id = i}).ToList()
    End Function

    ''' <summary>Colonne id d'une liste de tâches, dans l'ordre des lignes.</summary>
    Function IdsTaches(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("id"))).ToArray()
    End Function

    ''' <summary>Ligne d'une liste de tâches par id, Nothing si absente.</summary>
    Function LigneTache(table As DataTable, tacheId As Long) As DataRow
        Return table.Rows.Cast(Of DataRow)().FirstOrDefault(Function(r) CLng(r("id")) = tacheId)
    End Function

    ' --- Parcours -------------------------------------------------------------------

    ''' <summary>
    ''' Intervenant du parcours de soins d'un patient. Mêmes colonnes et mêmes valeurs
    ''' que l'INSERT de ParcoursDao.CreateIntervenantParcours, en SQL brut. Ce DAO (et
    ''' JeuxParcours.CreerParcoursPatient, qui passe par lui) enchaîne l'historique du
    ''' parcours, écrit en [oasis].[oasis] : chaque test de tâche qui a besoin d'un
    ''' parcours deviendrait Inconclusive hors CI. Renvoie l'id.
    ''' </summary>
    Function CreerParcoursPourTache(patientId As Long, utilisateurId As Long,
                                    Optional specialiteId As Integer = SpecialiteTacheOasis,
                                    Optional sousCategorieId As Integer = EnumSousCategoriePPS.IDE,
                                    Optional base As String = ParcoursDao.EnumParcoursBaseCode.ParAn,
                                    Optional rythme As Integer = 1,
                                    Optional rorId As Long = 0,
                                    Optional intervenantOasis As Boolean = True) As Long
        ' Catégorie 4 : Stratégie (EnvironnementBase.EnumCategoriePPS).
        Return InsererLignePourTache("oasis.oa_patient_parcours", "oa_parcours_id",
            "oa_parcours_patient_id, oa_parcours_specialite, oa_parcours_categorie_id, oa_parcours_sous_categorie_id," &
            " oa_parcours_intervenant_oasis, oa_parcours_ror_id, oa_parcours_commentaire, oa_parcours_base, oa_parcours_rythme," &
            " oa_parcours_cacher, oa_parcours_inactif, oa_parcours_utilisateur_creation, oa_parcours_date_creation",
            patientId, specialiteId, 4, sousCategorieId,
            intervenantOasis, rorId, "Parcours de test", base, rythme,
            False, False, utilisateurId, Date.Today)
    End Function

    ''' <summary>Parcours tel que CreationAutomatiqueDeDemandeRendezVous le reçoit, relu de la table.</summary>
    Function LireParcoursPourTache(parcoursId As Long) As Parcours
        Dim daoParcours As New ParcoursDao
        Return daoParcours.GetParcoursById(CInt(parcoursId))
    End Function

    ''' <summary>
    ''' INSERT qui fonctionne que la clé soit une identité ou non. Les valeurs
    ''' deviennent @p0, @p1... dans l'ordre des colonnes. Renvoie l'id créé.
    ''' </summary>
    Private Function InsererLignePourTache(table As String, colonneId As String, colonnes As String,
                                           ParamArray valeurs() As Object) As Long
        Dim marques = String.Join(", ", Enumerable.Range(0, valeurs.Length).Select(Function(i) "@p" & i))
        Dim sql =
            "DECLARE @id BIGINT;" & vbCrLf &
            "IF COLUMNPROPERTY(OBJECT_ID('" & table & "'), '" & colonneId & "', 'IsIdentity') = 1" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonnes & ") VALUES (" & marques & ");" & vbCrLf &
            "    SET @id = SCOPE_IDENTITY();" & vbCrLf &
            "END" & vbCrLf &
            "ELSE" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    SELECT @id = COALESCE(MAX(" & colonneId & "), 0) + 1 FROM " & table & ";" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonneId & ", " & colonnes & ") VALUES (@id, " & marques & ");" & vbCrLf &
            "END" & vbCrLf &
            "SELECT @id;"
        Return CLng(Scalaire(sql, valeurs))
    End Function

End Module
