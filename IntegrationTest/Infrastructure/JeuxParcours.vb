Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' Jeux de données du parcours de soins : intervenants du ROR, parcours d'un
''' patient, consignes, rendez-vous rattachés à un parcours, contextes datés.
'''
''' Chaque table qu'un DAO écrit passe par ce DAO : RorDao.CreationRor,
''' ParcoursDao.CreateIntervenantParcours, ParcoursConsigneDao.CreateParcoursConsigne,
''' TacheDao.CreateTache, ContexteDao.CreationContexte. Le SQL brut se limite à ce
''' qu'aucun DAO n'écrit (retrait d'un intervenant de l'annuaire, recalage d'une
''' identité, lectures sous le compte Admin).
'''
''' Les spécialités et les intervenants Oasis 1 et 2 viennent de
''' Schema/28-reference-parcours.sql : le singleton Table_specialite lit les
''' premières une fois pour tout le processus, CreateIntervenantOasisByPatient pose
''' les seconds en dur.
''' </summary>
Public Module JeuxParcours

    ' --- Référentiel posé par 28-reference-parcours.sql ---------------------------

    ''' <summary>Spécialité Oasis médecin référent (EnumSpecialiteOasis.medecinReferent), délai 60 jours.</summary>
    Public Const SpecialiteMedecinReferentOasis As Integer = 1
    ''' <summary>Spécialité Oasis IDE (EnumSpecialiteOasis.IDE), délai 60 jours.</summary>
    Public Const SpecialiteIdeOasis As Integer = 2
    ''' <summary>Spécialité hors Oasis active, délai 45 jours.</summary>
    Public Const SpecialiteParcoursTest As Integer = 9801
    Public Const SpecialiteParcoursTestLibelle As String = "Cardiologie de test"
    ''' <summary>Seconde spécialité hors Oasis active, délai 20 jours.</summary>
    Public Const SpecialiteParcoursAutre As Integer = 9802
    Public Const SpecialiteParcoursAutreLibelle As String = "Dermatologie de test"
    ''' <summary>Spécialité inactive : absente de Table_specialite, lisible par SpecialiteDao.</summary>
    Public Const SpecialiteParcoursInactive As Integer = 9803

    ''' <summary>Intervenant Oasis que CreateIntervenantOasisByPatient donne au médecin référent.</summary>
    Public Const RorMedecinReferentOasis As Long = 1
    ''' <summary>Intervenant Oasis que CreateIntervenantOasisByPatient donne à l'IDE.</summary>
    Public Const RorIdeOasis As Long = 2

    ' Catégorie et sous-catégories de PPS (EnumCategoriePPS, EnumSousCategoriePPS).
    Public Const CategorieParcoursSuivi As Integer = 3
    Public Const SousCategorieParcoursIde As Integer = 3
    Public Const SousCategorieParcoursMedecinReferent As Integer = 4
    Public Const SousCategorieParcoursSpecialiste As Integer = 6

    ' --- ROR ------------------------------------------------------------------------

    ''' <summary>
    ''' Intervenant du ROR, par RorDao.CreationRor sous le compte courant, comme la
    ''' fiche RadFRorDetailEdit. Le DAO écrit la date de création par
    ''' ToString("yyyy-MM-dd HH:mm:ss"), dont le séparateur horaire dépend de la
    ''' culture : celle du poste, fr-FR. Renvoie l'id.
    ''' </summary>
    Function CreerRorParcours(nom As String,
                              Optional specialiteId As Integer = SpecialiteParcoursTest,
                              Optional typeRor As String = "Intervenant",
                              Optional ville As String = "Mamoudzou",
                              Optional codePostal As String = "97600",
                              Optional email As String = "",
                              Optional structureNom As String = "Cabinet de test",
                              Optional inactif As Boolean = False,
                              Optional rpps As Long = 0,
                              Optional extractionAnnuaire As Boolean = False,
                              Optional identifiantNational As String = "",
                              Optional identifiantStructure As String = "",
                              Optional modeExercice As String = "",
                              Optional professionId As Integer = 0,
                              Optional typeSavoirFaire As String = "",
                              Optional codeSavoirFaire As String = "",
                              Optional utilisateurId As Long = 0) As Long
        Dim fiche As New Ror With {
            .SpecialiteId = specialiteId,
            .Nom = nom,
            .Type = typeRor,
            .StructureId = 0,
            .StructureNom = structureNom,
            .Adresse1 = "1 rue du Test",
            .Adresse2 = "Batiment B",
            .CodePostal = codePostal,
            .Ville = ville,
            .Code = "ITC",
            .Telephone = "0269123456",
            .Email = email,
            .Commentaire = "Intervenant de test",
            .Rpps = rpps,
            .Finess = 0,
            .Adeli = 0,
            .Inactif = inactif,
            .ExtractionAnnuaire = extractionAnnuaire,
            .IdentifiantNational = identifiantNational,
            .IdentifiantStructure = identifiantStructure,
            .CodeModeExercice_r23 = modeExercice,
            .CodeProfessionSante_g15 = professionId,
            .CodeTypeSavoirFaire_r04 = typeSavoirFaire,
            .CodeSavoirFaire = codeSavoirFaire,
            .CleReferenceAnnuaire = 0
        }
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Dim daoRor As New RorDao
            Return daoRor.CreationRor(fiche, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    ''' <summary>
    ''' Retire un intervenant de l'annuaire (oa_ror_annuaire_inactif = 1), comme le
    ''' laisse l'import de l'annuaire. Aucun DAO n'écrit cette colonne : SQL brut.
    ''' </summary>
    Sub RetirerRorAnnuaire(rorId As Long)
        Executer("UPDATE oasis.oa_ror SET oa_ror_annuaire_inactif = 1 WHERE oa_ror_id = @p0", rorId)
    End Sub

    ' --- Parcours -------------------------------------------------------------------

    ''' <summary>
    ''' Intervenant du parcours de soins d'un patient, par
    ''' ParcoursDao.CreateIntervenantParcours, comme RadFParcoursDetailEdit : l'INSERT,
    ''' l'historique (état Création) et la date de synthèse du patient. L'historique
    ''' est écrit en [oasis].[oasis] : le test appelant commence par
    ''' BaseDeTest.ExigerBaseOasis(). Renvoie l'id.
    ''' </summary>
    Function CreerParcoursPatient(patientId As Long, rorId As Long,
                                  Optional specialiteId As Integer = SpecialiteParcoursTest,
                                  Optional sousCategorieId As Integer = SousCategorieParcoursSpecialiste,
                                  Optional intervenantOasis As Boolean = False,
                                  Optional categorieId As Integer = CategorieParcoursSuivi,
                                  Optional baseCalcul As String = "PAR_AN",
                                  Optional rythme As Integer = 1,
                                  Optional commentaire As String = "Parcours de test",
                                  Optional cacher As Boolean = False,
                                  Optional utilisateurId As Long = 0) As Long
        Dim nouveau As New Parcours With {
            .PatientId = CInt(patientId),
            .SpecialiteId = specialiteId,
            .CategorieId = categorieId,
            .SousCategorieId = sousCategorieId,
            .IntervenantOasis = intervenantOasis,
            .RorId = CInt(rorId),
            .Commentaire = commentaire,
            .Base = baseCalcul,
            .Rythme = rythme,
            .Cacher = cacher,
            .Inactif = False,
            .UserCreation = CInt(utilisateurId),
            .DateCreation = Date.Now
        }
        Dim daoParcours As New ParcoursDao
        Return daoParcours.CreateIntervenantParcours(nouveau, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Function

    ''' <summary>Lignes d'historique d'un parcours, de la plus ancienne à la plus récente, sous le compte Admin.</summary>
    Function HistoriqueParcours(parcoursId As Long) As DataTable
        Return LireTableParcours("SELECT * FROM oasis.oa_patient_parcours_histo WHERE oa_parcours_id = @p0 ORDER BY oa_parcours_histo_id",
                                 parcoursId)
    End Function

    ''' <summary>Parcours d'un patient, par id croissant, sous le compte Admin.</summary>
    Function ParcoursDuPatient(patientId As Long) As DataTable
        Return LireTableParcours("SELECT * FROM oasis.oa_patient_parcours WHERE oa_parcours_patient_id = @p0 ORDER BY oa_parcours_id",
                                 patientId)
    End Function

    ''' <summary>Tâches rattachées à un parcours, par id croissant, sous le compte Admin.</summary>
    Function TachesDuParcours(parcoursId As Long) As DataTable
        Return LireTableParcours("SELECT * FROM oasis.oa_tache WHERE parcours_id = @p0 ORDER BY id", parcoursId)
    End Function

    ''' <summary>
    ''' Rendez-vous ou demande de rendez-vous rattaché à un parcours, par
    ''' TacheDao.CreateTache sous le compte courant, avec les valeurs que pose
    ''' CreationAutomatiqueDeDemandeRendezVous (catégorie SOIN, fonctions IDE, durée
    ''' 15 minutes, nature égale au type). Renvoie l'id.
    ''' </summary>
    Function CreerTacheRendezVousParcours(patientId As Long, parcoursId As Long, emetteurId As Long,
                                          typeDeTache As Oasis_Common.Tache.TypeTache,
                                          etatDeTache As Oasis_Common.Tache.EtatTache,
                                          dateRendezVous As Date) As Long
        Dim nouvelle As New Oasis_Common.Tache With {
            .ParentId = 0,
            .EmetteurUserId = emetteurId,
            .EmetteurFonctionId = FonctionDao.EnumFonction.IDE,
            .UniteSanitaireId = 0,
            .SiteId = 0,
            .PatientId = patientId,
            .ParcoursId = parcoursId,
            .EpisodeId = 0,
            .SousEpisodeId = 0,
            .TraiteUserId = 0,
            .TraiteFonctionId = FonctionDao.EnumFonction.IDE,
            .DestinataireFonctionId = FonctionDao.EnumFonction.IDE,
            .Priorite = Oasis_Common.Tache.EnumPriorite.BASSE,
            .OrdreAffichage = 20,
            .Categorie = Oasis_Common.Tache.CategorieTache.SOIN.ToString(),
            .Type = typeDeTache.ToString(),
            .Nature = typeDeTache.ToString(),
            .Duree = 15,
            .EmetteurCommentaire = "",
            .HorodatageCreation = Date.Now,
            .Etat = etatDeTache.ToString(),
            .Cloture = (etatDeTache = Oasis_Common.Tache.EtatTache.TERMINEE),
            .TypedemandeRendezVous = If(typeDeTache = Oasis_Common.Tache.TypeTache.RDV_DEMANDE,
                                        Oasis_Common.Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString(), ""),
            .DateRendezVous = dateRendezVous
        }
        Dim daoTache As New TacheDao
        daoTache.CreateTache(nouvelle, New Utilisateur With {.UtilisateurId = CInt(emetteurId)})
        Return CLng(Scalaire("SELECT MAX(id) FROM oasis.oa_tache WHERE parcours_id = @p0", parcoursId))
    End Function

    ' --- Consignes ------------------------------------------------------------------

    ''' <summary>
    ''' Consigne de parcours, par ParcoursConsigneDao.CreateParcoursConsigne, comme
    ''' RadFParcoursConsigneDetailEdit. Les dates absentes valent une fenêtre ouverte
    ''' (2000 à 2999), le DAO n'acceptant pas Date.MinValue. CreateParcoursConsigne ne
    ''' renvoie pas l'id : c'est le plus grand du patient. Renvoie l'id.
    ''' </summary>
    Function CreerConsigneParcours(parcoursId As Long, patientId As Long, drcId As Long,
                                   Optional typeActivite As String = "PATHOLOGIE_AIGUE",
                                   Optional ordre As Integer = 1,
                                   Optional commentaire As String = "Consigne de test",
                                   Optional dateDebut As Date? = Nothing,
                                   Optional dateFin As Date? = Nothing,
                                   Optional inactif As Boolean = False,
                                   Optional ageMin As Integer = 0,
                                   Optional ageMax As Integer = 0,
                                   Optional ageUnite As String = "A") As Long
        Dim daoConsigne As New ParcoursConsigneDao
        daoConsigne.CreateParcoursConsigne(New ParcoursConsigne With {
            .ParcoursId = parcoursId,
            .PatientId = patientId,
            .DrcId = drcId,
            .TypeEpisode = typeActivite,
            .Commentaire = commentaire,
            .Ordre = ordre,
            .AgeMin = ageMin,
            .AgeMax = ageMax,
            .AgeUnite = ageUnite,
            .DateDebut = If(dateDebut, New Date(2000, 1, 1)),
            .DateFin = If(dateFin, New Date(2999, 12, 31)),
            .Inactif = inactif
        })
        Return CLng(Scalaire("SELECT MAX(oa_parcours_consigne_id) FROM oasis.oa_patient_parcours_consigne" &
                             " WHERE oa_parcours_consigne_patient_id = @p0", patientId))
    End Function

    ' --- Contextes -----------------------------------------------------------------

    ''' <summary>
    ''' Contexte patient (type C) par ContexteDao.CreationContexte, hors conclusion
    ''' d'épisode, avec une date de fin au choix (31/12/2999 par défaut, comme les
    ''' écrans). Le DAO horodate par ToString("yyyy-MM-dd HH:mm:ss") : culture du
    ''' poste, fr-FR. Renvoie l'id.
    ''' </summary>
    Function CreerContexteParcours(patientId As Long, utilisateurId As Long, drcId As Long,
                                   Optional description As String = "Contexte de parcours",
                                   Optional dateFin As Date? = Nothing,
                                   Optional statutAffichage As String = "P",
                                   Optional categorie As String = "M",
                                   Optional diagnostic As Integer = 1,
                                   Optional episodeId As Long = 0) As Long
        Dim nouveau As New Antecedent With {
            .PatientId = CInt(patientId),
            .Type = "C",
            .DrcId = CInt(drcId),
            .Description = description,
            .DateDebut = DateDebutAntecedentDeTest,
            .DateFin = If(dateFin, FinContexteDeTest),
            .Niveau = 1,
            .Nature = "Patient",
            .StatutAffichage = statutAffichage,
            .StatutAffichageTransformation = "P",
            .CategorieContexte = categorie,
            .Ordre1 = 0,
            .Diagnostic = diagnostic,
            .EpisodeId = episodeId,
            .Inactif = False
        }
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Dim daoContexte As New ContexteDao
            Return daoContexte.CreationContexte(nouveau, New AntecedentHisto, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    ' --- Chaînes d'épisodes --------------------------------------------------------

    ''' <summary>
    ''' Fait partir les prochains ids de oa_chaine_episode au-delà du million, loin
    ''' de tout id d'antécédent : ChaineEpisodeDao.GetRelationListByPatient joint
    ''' l'id de la chaîne à celui de l'antécédent, et un test ne doit pas dépendre
    ''' d'une coïncidence de numérotation. Réservé au compte Admin (DBCC).
    ''' </summary>
    Sub DecalerIdentiteChaineEpisode()
        Executer("DBCC CHECKIDENT ('oasis.oa_chaine_episode', RESEED, 1000000) WITH NO_INFOMSGS")
    End Sub

    Private Function LireTableParcours(sql As String, identifiant As Long) As DataTable
        Using connexion As New SqlConnection(ChaineConnexion(Compte.Admin))
            connexion.Open()
            Using commande As New SqlCommand(sql, connexion)
                commande.Parameters.AddWithValue("@p0", identifiant)
                Dim resultat As New DataTable
                Using adaptateur As New SqlDataAdapter(commande)
                    adaptateur.Fill(resultat)
                End Using
                Return resultat
            End Using
        End Using
    End Function

End Module
