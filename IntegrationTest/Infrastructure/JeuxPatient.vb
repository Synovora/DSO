Imports Oasis_Common

''' <summary>
''' Patients de test. Ils passent par PatientDao.CreationPatient, l'INSERT que la
''' fiche patient du client lourd exécute en production.
''' </summary>
Public Module JeuxPatient

    ''' <summary>
    ''' Date que la fiche patient utilise pour « non renseignée » (MaxDate du
    ''' sélecteur). Une date d'entrée égale à celle-ci empêche CreationPatient de
    ''' créer le parcours de soins par défaut, qui demanderait le référentiel ROR.
    ''' </summary>
    Public ReadOnly DateNonRenseignee As New Date(9998, 12, 31)

    ''' <summary>Base des NIR de test : 13 chiffres, un par patient créé dans le processus.</summary>
    Private Const NirDeBase As Long = 1700175000000L

    Private compteurNir As Integer = 0

    ''' <summary>
    ''' Crée un patient hors dispositif Oasis (sans date d'entrée), comme la fiche
    ''' patient quand l'utilisateur confirme la création sans date d'entrée.
    ''' Renvoie son id.
    ''' </summary>
    Function CreerPatient(Optional nom As String = "TEST",
                          Optional prenom As String = "Patient",
                          Optional siteId As Integer = 0,
                          Optional uniteSanitaireId As Integer = 0,
                          Optional siegeId As Integer = 0) As Long
        compteurNir += 1
        Dim nouveau As New Patient With {
            .PatientNir = NirDeBase + compteurNir,
            .INS = 0,
            .PatientNom = nom,
            .PatientPrenom = prenom,
            .PatientNomMarital = "",
            .PatientDateNaissance = New Date(1970, 1, 15),
            .PatientGenreId = Patient.EnumGenreId.Feminin,
            .PatientAdresse1 = "1 rue du Test",
            .PatientAdresse2 = "",
            .PatientCodePostal = "97600",
            .PatientVille = "Mamoudzou",
            .PatientTel1 = "",
            .PatientTel2 = "",
            .PatientEmail = "",
            .PatientDateEntree = DateNonRenseignee,
            .PatientDateSortie = DateNonRenseignee,
            .PatientCommentaireSortie = "",
            .PatientDateDeces = DateNonRenseignee,
            .PatientSiteId = siteId,
            .PatientUniteSanitaireId = uniteSanitaireId,
            .PatientInternet = False,
            .Profession = "",
            .PharmacienId = 0
        }
        ' CreationPatient ne lit que le siège de l'utilisateur connecté.
        Dim auteur As New Utilisateur With {.UtilisateurSiegeId = siegeId}
        Dim daoPatient As New PatientDao
        daoPatient.CreationPatient(nouveau, auteur)

        ' CreationPatient ne renvoie pas l'id : on le retrouve par le NIR, propre à ce patient.
        Return CLng(Scalaire("SELECT MAX(oa_patient_id) FROM oasis.oa_patient WHERE oa_patient_nir = @p0",
                             nouveau.PatientNir))
    End Function

    ''' <summary>
    ''' Fiche patient prête pour CreationPatient, avec les mêmes valeurs que CreerPatient
    ''' (hors dispositif, dates « non renseignées ») et un NIR neuf tiré du même compteur,
    ''' sauf si nir est donné. Rien n'est écrit en base.
    ''' </summary>
    Function PatientDeTest(Optional nom As String = "TEST",
                           Optional prenom As String = "Patient",
                           Optional nir As Long = -1,
                           Optional ins As Long = 0,
                           Optional dateNaissance As Date? = Nothing,
                           Optional siteId As Integer = 0) As Patient
        Dim nirRetenu = nir
        If nirRetenu < 0 Then
            compteurNir += 1
            nirRetenu = NirDeBase + compteurNir
        End If
        Return New Patient With {
            .PatientNir = nirRetenu,
            .INS = ins,
            .PatientNom = nom,
            .PatientPrenom = prenom,
            .PatientNomMarital = "",
            .PatientDateNaissance = If(dateNaissance, New Date(1970, 1, 15)),
            .PatientGenreId = Patient.EnumGenreId.Feminin,
            .PatientAdresse1 = "1 rue du Test",
            .PatientAdresse2 = "",
            .PatientCodePostal = "97600",
            .PatientVille = "Mamoudzou",
            .PatientTel1 = "",
            .PatientTel2 = "",
            .PatientEmail = "",
            .PatientDateEntree = DateNonRenseignee,
            .PatientDateSortie = DateNonRenseignee,
            .PatientCommentaireSortie = "",
            .PatientDateDeces = DateNonRenseignee,
            .PatientSiteId = siteId,
            .PatientUniteSanitaireId = 0,
            .PatientInternet = False,
            .Profession = "",
            .PharmacienId = 0
        }
    End Function

    ''' <summary>
    ''' Enregistre la fiche par PatientDao.CreationPatient, sous le compte courant, et
    ''' renvoie l'id attribué (le plus grand : les tests s'exécutent l'un après l'autre).
    ''' La date d'entrée doit rester DateNonRenseignee : sinon CreationPatient crée le
    ''' parcours de soins par défaut, qui demande le référentiel ROR.
    ''' </summary>
    Function EnregistrerPatient(fiche As Patient, Optional siegeId As Integer = 0) As Long
        Dim daoPatient As New PatientDao
        daoPatient.CreationPatient(fiche, New Utilisateur With {.UtilisateurSiegeId = siegeId})
        Return CLng(Scalaire("SELECT MAX(oa_patient_id) FROM oasis.oa_patient"))
    End Function

    ''' <summary>
    ''' Pose les dates d'entrée et de sortie du dispositif Oasis ; Nothing donne NULL.
    ''' SQL brut : CreationPatient et ModificationPatient créent le parcours de soins
    ''' par défaut dès qu'une date d'entrée est renseignée sans date de sortie, et ce
    ''' parcours demande le référentiel ROR et la base « oasis » en dur de ParcoursDao.
    ''' </summary>
    Sub PoserDatesOasisPatient(patientId As Long, entree As Date?, sortie As Date?)
        Executer("UPDATE oasis.oa_patient SET oa_patient_date_entree_oasis = @p0, oa_patient_date_sortie_oasis = @p1" &
                 " WHERE oa_patient_id = @p2",
                 If(entree.HasValue, CObj(entree.Value), Nothing),
                 If(sortie.HasValue, CObj(sortie.Value), Nothing),
                 patientId)
    End Sub

    ''' <summary>Pose la date de mise à jour de la synthèse ; Nothing donne NULL.</summary>
    Sub PoserDateMajSynthese(patientId As Long, valeur As Date?)
        Executer("UPDATE oasis.oa_patient SET oa_patient_synthese_date_maj = @p0 WHERE oa_patient_id = @p1",
                 If(valeur.HasValue, CObj(valeur.Value), Nothing), patientId)
    End Sub

    ''' <summary>
    ''' Id de la dernière note créée pour ce patient dans une des tables de notes
    ''' (oa_patient_note, _directive, _medicale, _social, _vaccin). Les CreationNote ne
    ''' renvoient pas l'id.
    ''' </summary>
    Function DerniereNotePatient(table As String, patientId As Long) As Long
        Return CLng(Scalaire("SELECT MAX(oa_patient_note_id) FROM oasis." & table & " WHERE oa_patient_id = @p0", patientId))
    End Function

    ''' <summary>
    ''' Date de création d'une note, fixée pour les tests d'ordre : CreationNote
    ''' horodate à la seconde, deux notes créées d'affilée tomberaient ex aequo.
    ''' </summary>
    Sub PoserDateCreationNote(table As String, noteId As Long, dateCreation As Date)
        Executer("UPDATE oasis." & table & " SET oa_patient_note_date_creation = @p0 WHERE oa_patient_note_id = @p1",
                 dateCreation, noteId)
    End Sub

    ''' <summary>Pose l'indicateur d'annulation d'une note ; Nothing donne NULL.</summary>
    Sub PoserInvalideNote(table As String, noteId As Long, valeur As Boolean?)
        Executer("UPDATE oasis." & table & " SET oa_patient_note_invalide = @p0 WHERE oa_patient_note_id = @p1",
                 If(valeur.HasValue, CObj(valeur.Value), Nothing), noteId)
    End Sub

    ''' <summary>
    ''' Antécédent rattaché à une DRC, pour PatientDao.GetByDRC. Mêmes colonnes et mêmes
    ''' valeurs fixes que l'INSERT d'AntecedentDao.CreationAntecedent, mais en SQL brut :
    ''' CreationAntecedent écrit aussi l'historique et la synthèse et appartient au lot
    ''' Antecedent ; GetByDRC n'a besoin que du lien patient, DRC. Renvoie l'id.
    ''' </summary>
    Function CreerAntecedentSurDrc(patientId As Long, drcId As Long, utilisateurId As Long,
                                   Optional inactif As Boolean = False) As Long
        Dim sansDate As New Date(2999, 12, 31)
        Return CLng(Scalaire(
            "INSERT INTO oasis.oa_antecedent (oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id, oa_antecedent_description," &
            " oa_antecedent_date_creation, oa_antecedent_utilisateur_creation, oa_antecedent_utilisateur_modification, oa_antecedent_date_debut," &
            " oa_antecedent_niveau, oa_antecedent_nature, oa_antecedent_statut_affichage, oa_antecedent_inactif, oa_antecedent_ordre_affichage1," &
            " oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3, oa_antecedent_diagnostic, oa_antecedent_ald_id," &
            " oa_antecedent_ald_cim_10_id, oa_antecedent_ald_valide, oa_antecedent_ald_date_debut, oa_antecedent_ald_date_fin," &
            " oa_antecedent_ald_demande_en_cours, oa_antecedent_ald_demande_date, oa_chaine_episode_date_fin)" &
            " VALUES (@p0, 'A', @p1, @p2, @p3, @p4, 0, @p5, 1, 'Patient', 'P', @p6, 980, 0, 0, 0, 0, 0, 0, @p7, @p7, 0, @p7, @p7);" &
            " SELECT CAST(SCOPE_IDENTITY() AS BIGINT);",
            patientId, drcId, "Antecedent de test", Date.Now, utilisateurId, Date.Today, inactif, sansDate))
    End Function

    ''' <summary>
    ''' Allergie à une substance père. AllergieDao.CreationAllergie va chercher le nom de
    ''' la substance père dans la base Theriak, absente des tests : SQL brut, avec les
    ''' colonnes et les valeurs que le DAO écrit dans ce cas (substance_id à 0,
    ''' denomination_substance vide).
    ''' </summary>
    Sub CreerAllergieSubstancePere(patientId As Long, substancePereId As Long, denominationPere As String, utilisateurId As Long)
        Executer("INSERT INTO oasis.oa_patient_allergie (patient_id, substance_id, substance_pere_id, denomination_substance," &
                 " denomination_substance_pere, creation_user_id, creation_date, inactif)" &
                 " VALUES (@p0, 0, @p1, '', @p2, @p3, @p4, 0)",
                 patientId, substancePereId, denominationPere, utilisateurId, Date.Now)
    End Sub

    ''' <summary>Même chose pour une contre-indication à une substance père (ContreIndicationSubstanceDao).</summary>
    Sub CreerContreIndicationSubstancePere(patientId As Long, substancePereId As Long, denominationPere As String, utilisateurId As Long)
        Executer("INSERT INTO oasis.oa_patient_contre_indication_substance (patient_id, substance_id, substance_pere_id, denomination_substance," &
                 " denomination_substance_pere, creation_user_id, creation_date, inactif)" &
                 " VALUES (@p0, 0, @p1, '', @p2, @p3, @p4, 0)",
                 patientId, substancePereId, denominationPere, utilisateurId, Date.Now)
    End Sub

End Module
