Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' Antécédents, contextes et PPS de test. Les antécédents passent par
''' AntecedentDao.CreationAntecedent, les contextes par ContexteDao.CreationContexte,
''' les PPS par PpsDao.CreationPPS : leurs INSERT sont ceux que le client lourd
''' exécute en production, historique et date de synthèse compris. La hiérarchie
''' (niveau, pères, ordres d'affichage) est posée par
''' AntecedentAffectationDao.UpdateAntecedentaAffecter, l'UPDATE que l'écran de
''' synthèse emploie pour déplacer un antécédent.
'''
''' Le SQL brut se limite à ce qu'aucun DAO n'écrit : l'arrêt d'un contexte, le
''' retrait de la date de fin ou de l'affichage en synthèse d'un PPS, les
''' sous-catégories de PPS (oa_r_pps_sous_categorie, qu'aucun singleton ne met en
''' cache).
''' </summary>
Public Module JeuxAntecedent

    ''' <summary>Date de début des antécédents et contextes de test, à minuit.</summary>
    Public ReadOnly DateDebutAntecedentDeTest As New Date(2020, 3, 15)

    ''' <summary>Date de fin que les écrans donnent à un contexte sans fin (« 31/12/2999 »).</summary>
    Public ReadOnly FinContexteDeTest As New Date(2999, 12, 31)

    ''' <summary>Date de fin des PPS de test quand le test n'en donne pas.</summary>
    Public ReadOnly FinPpsDeTest As New Date(2999, 12, 31)

    ''' <summary>
    ''' Crée un antécédent (type A) comme la fenêtre RadFAntecedentDetailEdit : sans
    ''' ALD, fin de chaîne d'épisodes à six mois. L'antécédent arrive en niveau 1,
    ''' ordre 980. drcId à 0 : une DRC est créée pour lui. Renvoie son id.
    ''' </summary>
    Function CreerAntecedent(patientId As Long, utilisateurId As Long,
                             Optional drcId As Long = 0,
                             Optional description As String = "Antécédent de test",
                             Optional statutAffichage As String = "P",
                             Optional diagnostic As Integer = 1,
                             Optional dateDebut As Date? = Nothing) As Long
        Dim drcRetenue = If(drcId = 0, CreerDrc(), drcId)
        Dim nouveau As New Antecedent With {
            .PatientId = CInt(patientId),
            .DrcId = CInt(drcRetenue),
            .Description = description,
            .DateDebut = If(dateDebut, DateDebutAntecedentDeTest),
            .StatutAffichage = statutAffichage,
            .Diagnostic = diagnostic,
            .AldId = 0,
            .ChaineEpisodeDateFin = Date.Today.AddMonths(6)
        }
        Dim dao As New AntecedentDao
        Return dao.CreationAntecedent(nouveau, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Function

    ''' <summary>
    ''' Place un antécédent dans la hiérarchie : niveau, pères de niveau 1 et 2, trois
    ''' ordres d'affichage, par AntecedentAffectationDao.UpdateAntecedentaAffecter.
    ''' </summary>
    Sub PlacerAntecedent(antecedentId As Long, niveau As Integer, idNiveau1 As Long, idNiveau2 As Long,
                         ordre1 As Integer, ordre2 As Integer, ordre3 As Integer)
        Dim dao As New AntecedentAffectationDao
        dao.UpdateAntecedentaAffecter(CInt(antecedentId), niveau, CInt(idNiveau1), CInt(idNiveau2), ordre1, ordre2, ordre3)
    End Sub

    ''' <summary>
    ''' Position d'un antécédent relue sous le compte Admin, sous la forme
    ''' « niveau/idNiveau1/idNiveau2/ordre1/ordre2/ordre3 » (NULL donne une chaîne vide).
    ''' Une seule chaîne à comparer rend les écarts lisibles dans le message d'échec.
    ''' </summary>
    Function PositionAntecedent(antecedentId As Long) As String
        Return CStr(Scalaire(
            "SELECT CONCAT(oa_antecedent_niveau, '/', oa_antecedent_id_niveau1, '/', oa_antecedent_id_niveau2, '/'," &
            " oa_antecedent_ordre_affichage1, '/', oa_antecedent_ordre_affichage2, '/', oa_antecedent_ordre_affichage3)" &
            " FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", antecedentId))
    End Function

    ''' <summary>Statut d'affichage d'un antécédent (P, C ou O), relu sous le compte Admin.</summary>
    Function StatutAntecedent(antecedentId As Long) As String
        Return CStr(Scalaire("SELECT oa_antecedent_statut_affichage FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0",
                             antecedentId))
    End Function

    ''' <summary>
    ''' Lignes d'historique d'un antécédent (oa_antecedent_histo), de la plus ancienne
    ''' à la plus récente, lues sous le compte Admin.
    ''' </summary>
    Function HistoriqueAntecedent(antecedentId As Long) As DataTable
        Return TableAdmin("SELECT * FROM oasis.oa_antecedent_histo WHERE oa_antecedent_id = @p0 ORDER BY oa_antecedent_histo_id",
                          antecedentId)
    End Function

    ''' <summary>
    ''' Lignes d'historique d'un PPS (oa_patient_pps_histo), de la plus ancienne à la
    ''' plus récente, lues sous le compte Admin.
    ''' </summary>
    Function HistoriquePps(ppsId As Long) As DataTable
        Return TableAdmin("SELECT * FROM oasis.oa_patient_pps_histo WHERE oa_pps_id = @p0 ORDER BY oa_pps_histo_id", ppsId)
    End Function

    Private Function TableAdmin(sql As String, identifiant As Long) As DataTable
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

    ''' <summary>
    ''' Crée un contexte (type C) comme RadFContextedetailEdit : niveau 1, sans fin
    ''' (31/12/2999), hors conclusion d'épisode. drcId à 0 : une DRC est créée pour
    ''' lui. Renvoie son id.
    ''' </summary>
    Function CreerContexteMedical(patientId As Long, utilisateurId As Long,
                                  Optional drcId As Long = 0,
                                  Optional description As String = "Contexte de test",
                                  Optional statutAffichage As String = "P",
                                  Optional categorie As String = "M",
                                  Optional ordre1 As Integer = 0) As Long
        Dim drcRetenue = If(drcId = 0, CreerDrc(), drcId)
        Dim nouveau As New Antecedent With {
            .PatientId = CInt(patientId),
            .Type = "C",
            .DrcId = CInt(drcRetenue),
            .Description = description,
            .DateDebut = DateDebutAntecedentDeTest,
            .DateFin = FinContexteDeTest,
            .Niveau = 1,
            .Nature = "Patient",
            .StatutAffichage = statutAffichage,
            .StatutAffichageTransformation = "P",
            .CategorieContexte = categorie,
            .Ordre1 = ordre1,
            .Diagnostic = 1,
            .EpisodeId = 0,
            .Inactif = False
        }
        Dim dao As New ContexteDao
        Return dao.CreationContexte(nouveau, New AntecedentHisto, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Function

    ''' <summary>
    ''' Arrête un contexte. Aucun DAO ne pose oa_antecedent_arret : SQL brut, comme
    ''' l'état que les écrans de contexte laissent derrière eux.
    ''' </summary>
    Sub ArreterContexteMedical(contexteId As Long)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_arret = 1 WHERE oa_antecedent_id = @p0", contexteId)
    End Sub

    ''' <summary>
    ''' Crée un PPS par PpsDao.CreationPPS, comme RadFPPSDetailEdit, avec une date de
    ''' fin (CreationPPS n'accepte pas de PPS sans date de fin, voir PpsDaoTest).
    ''' drcId à 0 : une DRC est créée pour lui. CreationPPS ne renvoie pas l'id : il
    ''' est relu en base. Renvoie son id.
    ''' </summary>
    Function CreerPps(patientId As Long, utilisateurId As Long, categorie As Integer, sousCategorie As Integer,
                      Optional priorite As Integer = 1,
                      Optional drcId As Long = 0,
                      Optional commentaire As String = "PPS de test",
                      Optional dateFin As Date? = Nothing) As Long
        Dim drcRetenue = If(drcId = 0, CreerDrc(), drcId)
        Dim nouveau As New Pps With {
            .PatientId = CInt(patientId),
            .CategorieId = categorie,
            .SousCategorieId = sousCategorie,
            .DrcId = CInt(drcRetenue),
            .Priorite = priorite,
            .Commentaire = commentaire,
            .AffichageSynthese = True,
            .Inactif = False,
            .DateFin = If(dateFin, FinPpsDeTest)
        }
        Dim dao As New PpsDao
        dao.CreationPPS(nouveau, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Return CLng(Scalaire("SELECT MAX(oa_pps_id) FROM oasis.oa_patient_pps WHERE oa_pps_patient_id = @p0", patientId))
    End Function

    ''' <summary>Retire la date de fin d'un PPS (état que laisse ModificationPPS sans date de fin), sans historique.</summary>
    Sub RetirerDateFinPps(ppsId As Long)
        Executer("UPDATE oasis.oa_patient_pps SET oa_pps_date_fin = NULL WHERE oa_pps_id = @p0", ppsId)
    End Sub

    ''' <summary>
    ''' Retire un PPS de la synthèse (oa_pps_affichage_synthese = 0). CreationPPS pose
    ''' toujours 1 et aucun DAO n'écrit 0 : SQL brut.
    ''' </summary>
    Sub MasquerPpsDeLaSynthese(ppsId As Long)
        Executer("UPDATE oasis.oa_patient_pps SET oa_pps_affichage_synthese = 0 WHERE oa_pps_id = @p0", ppsId)
    End Sub

    ''' <summary>
    ''' Sous-catégorie de PPS de référence, créée si elle manque. getAllPPSbyPatient part
    ''' de cette table : sans elle, aucun PPS ne sort. Aucun singleton ne la lit, elle
    ''' peut donc être créée par un test.
    ''' </summary>
    Sub AssurerSousCategoriePps(categorie As Integer, sousCategorie As Integer, ordreAffichage As Integer,
                                Optional typeSousCategorie As String = "")
        Executer("IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_pps_sous_categorie" &
                 " WHERE oa_r_pps_categorie_id = @p0 AND oa_r_pps_sous_categorie_id = @p1)" &
                 " INSERT INTO oasis.oa_r_pps_sous_categorie (oa_r_pps_categorie_id, oa_r_pps_sous_categorie_id," &
                 " oa_r_pps_sous_categorie_type, oa_r_pps_sous_categorie_ordre_affichage)" &
                 " VALUES (@p0, @p1, @p2, @p3)",
                 categorie, sousCategorie, typeSousCategorie, ordreAffichage)
    End Sub

End Module
