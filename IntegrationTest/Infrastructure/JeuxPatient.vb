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

End Module
