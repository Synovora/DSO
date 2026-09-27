Imports Oasis_Common

''' <summary>
''' Ordonnances de test. Elles sont construites comme le client lourd les construit :
''' un traitement en cours par ligne (TraitementDao.CreationTraitement), l'en-tête
''' par OrdonnanceDao.CreateOrdonnance, chaque ligne par
''' OrdonnanceDetailDao.CreationOrdonnanceDetail, la signature par
''' OrdonnanceDao.ValidationOrdonnance. Aucun INSERT brut : chaque table a son DAO.
''' </summary>
Public Module JeuxOrdonnance

    ''' <summary>Code CIS de base des médicaments de test ; le rang de la ligne s'y ajoute.</summary>
    Public Const CisDeTest As Integer = 60000000

    ''' <summary>
    ''' Valeur que CreateNewOrdonnanceDetail donne aux dates de fenêtre thérapeutique
    ''' absentes (« 31/12/2999 »).
    ''' </summary>
    Public ReadOnly FenetreAbsente As New Date(2999, 12, 31)

    ''' <summary>Ordonnance avec nbLignes lignes, non signée. Renvoie son id.</summary>
    ''' <param name="avecTraitements">
    ''' Vrai : chaque ligne renvoie à un traitement réel du patient, comme les lignes
    ''' que génère l'application (et que /Sign/Check relit). Faux : lignes sans
    ''' traitement (oa_traitement_id = 0), pour les tests qui n'ont besoin que des
    ''' tables d'ordonnance.
    ''' </param>
    Function CreerOrdonnance(patientId As Long, utilisateurId As Long,
                             Optional nbLignes As Integer = 2,
                             Optional episodeId As Long = 0,
                             Optional avecTraitements As Boolean = True) As Long
        Dim daoOrdonnance As New OrdonnanceDao
        Dim auteur As New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}
        Dim idOrdonnance As Long = daoOrdonnance.CreateOrdonnance(patientId, episodeId, auteur)
        For rang = 1 To nbLignes
            Dim idTraitement As Long = 0
            If avecTraitements Then
                idTraitement = CreerTraitement(patientId, utilisateurId, rang)
            End If
            AjouterLigne(idOrdonnance, rang, idTraitement)
        Next
        Return idOrdonnance
    End Function

    ''' <summary>
    ''' Même chose, signée par utilisateurId comme le fait l'application
    ''' (voir SignerCommeLeClient). L'utilisateur doit avoir une clé. Renvoie l'id.
    ''' </summary>
    Function CreerOrdonnanceSignee(patientId As Long, utilisateurId As Long,
                                   Optional nbLignes As Integer = 2,
                                   Optional episodeId As Long = 0) As Long
        Dim idOrdonnance = CreerOrdonnance(patientId, utilisateurId, nbLignes, episodeId)
        SignerCommeLeClient(idOrdonnance, utilisateurId)
        Return idOrdonnance
    End Function

    ''' <summary>
    ''' Signe comme le poste : ValidationOrdonnance reçoit l'utilisateur tel que le
    ''' client le connaît (sans clé privée, que oasis_client ne peut pas lire), et
    ''' Utilisateur.Sign passe par le crochet SignataireDistant. Le crochet installé ici
    ''' fait ce que fait /api/signature (SignatureController.Signer) : signer avec la
    ''' clé de l'utilisateur et renvoyer son adresse. Le crochet précédent est remis
    ''' en place ensuite.
    ''' </summary>
    Sub SignerCommeLeClient(ordonnanceId As Long, utilisateurId As Long)
        Dim serveur = SignataireServeur(utilisateurId)
        Dim precedent = Utilisateur.SignataireDistant
        Utilisateur.SignataireDistant =
            Function(charge As Byte()) As SignatureResponse
                Return New SignatureResponse With {
                    .Signature = serveur.Sign(charge),
                    .Adresse = serveur.UtilisateurAddress
                }
            End Function
        Try
            Dim daoUtilisateur As New UserDao
            Dim poste As Utilisateur = daoUtilisateur.GetUserById(CInt(utilisateurId))
            Dim daoOrdonnance As New OrdonnanceDao
            daoOrdonnance.ValidationOrdonnance(ordonnanceId, poste)
        Finally
            Utilisateur.SignataireDistant = precedent
        End Try
    End Sub

    ''' <summary>
    ''' L'utilisateur tel que le serveur le connaît : avec sa clé privée, lue sous le
    ''' compte Admin puisque oasis_client n'y a pas accès. Sign signe alors sur place,
    ''' sans passer par le crochet.
    ''' </summary>
    Function SignataireServeur(utilisateurId As Long) As Utilisateur
        Dim clePrivee = Scalaire("SELECT cle_privee FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", utilisateurId)
        Dim adresse = Scalaire("SELECT cle_publique FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", utilisateurId)
        If clePrivee Is Nothing OrElse IsDBNull(clePrivee) OrElse CStr(clePrivee) = "" Then
            Throw New InvalidOperationException(
                "L'utilisateur " & utilisateurId & " n'a pas de clé de signature : CreerUtilisateur(avecCle:=True).")
        End If
        Return New Utilisateur With {
            .UtilisateurId = CInt(utilisateurId),
            .UtilisateurLogin = "signataire" & utilisateurId,
            .UtilisateurClePrivee = CStr(clePrivee),
            .UtilisateurAddress = If(IsDBNull(adresse), "", CStr(adresse))
        }
    End Function

    ''' <summary>
    ''' Ligne d'ordonnance telle que CreateNewOrdonnanceDetail la tire d'un traitement
    ''' journalier « 1 matin, 1 soir », sur 30 jours. Toutes les dates sont à minuit
    ''' pour être relues à l'identique, que la colonne soit de type date ou datetime.
    ''' </summary>
    Function LigneDeTraitement(ordonnanceId As Long, rang As Integer,
                               Optional traitementId As Long = 0,
                               Optional enAld As Boolean = False) As OrdonnanceDetail
        Return New OrdonnanceDetail With {
            .OrdonnanceId = CInt(ordonnanceId),
            .Traitement = True,
            .TraitementId = CInt(traitementId),
            .OrdreAffichage = rang,
            .Ald = enAld,
            .ADelivrer = True,
            .MedicamentCis = CisDeTest + rang,
            .MedicamentDci = "DCI TEST " & rang,
            .DateDebut = Date.Today,
            .DateFin = Date.Today.AddDays(29),
            .Duree = 30,
            .Posologie = " 1. 0. 1",
            .PosologieBase = Traitement.EnumBaseCode.JOURNALIER,
            .PosologieRythme = 0,
            .PosologieMatin = 1,
            .PosologieMidi = 0,
            .PosologieApresMidi = 0,
            .PosologieSoir = 1,
            .FractionMatin = Traitement.EnumFraction.Non,
            .FractionMidi = Traitement.EnumFraction.Non,
            .FractionApresMidi = Traitement.EnumFraction.Non,
            .FractionSoir = Traitement.EnumFraction.Non,
            .PosologieCommentaire = "",
            .Commentaire = "",
            .Fenetre = False,
            .FenetreDateDebut = FenetreAbsente,
            .FenetreDateFin = FenetreAbsente,
            .Inactif = False
        }
    End Function

    ''' <summary>
    ''' Ajoute une ligne (LigneDeTraitement) par le DAO et renvoie son id.
    ''' CreationOrdonnanceDetail renvoie toujours 0 : son INSERT ne lit pas
    ''' SCOPE_IDENTITY. L'id est donc relu en base.
    ''' </summary>
    Function AjouterLigne(ordonnanceId As Long, rang As Integer,
                          Optional traitementId As Long = 0,
                          Optional enAld As Boolean = False) As Long
        Dim daoDetail As New OrdonnanceDetailDao
        daoDetail.CreationOrdonnanceDetail(LigneDeTraitement(ordonnanceId, rang, traitementId, enAld))
        Return DerniereLigne(ordonnanceId)
    End Function

    ''' <summary>Id de la dernière ligne créée pour cette ordonnance.</summary>
    Function DerniereLigne(ordonnanceId As Long) As Long
        Return CLng(Scalaire("SELECT MAX(oa_ordonnance_ligne_id) FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_id = @p0",
                             ordonnanceId))
    End Function

    ''' <summary>
    ''' Traitement en cours pour le patient, par TraitementDao.CreationTraitement
    ''' (qui écrit aussi l'historique et la date de mise à jour de la synthèse) :
    ''' journalier, 1 matin et 1 soir, du jour à J+60. Renvoie son id.
    ''' Sa place naturelle sera un JeuxTraitement quand le domaine sera couvert.
    ''' </summary>
    Function CreerTraitement(patientId As Long, utilisateurId As Long, rang As Integer) As Long
        Dim nouveau As New Traitement With {
            .PatientId = CInt(patientId),
            .MedicamentId = CisDeTest + rang,
            .MedicamentMonographie = False,
            .MedicamentDci = "DCI TEST " & rang,
            .DenominationLongue = "MEDICAMENT TEST " & rang & " 500 mg, comprimé",
            .ClasseAtc = "",
            .OrdreAffichage = rang,
            .PosologieBase = Traitement.EnumBaseCode.JOURNALIER,
            .PosologieRythme = 0,
            .PosologieMatin = 1,
            .PosologieMidi = 0,
            .PosologieApresMidi = 0,
            .PosologieSoir = 1,
            .FractionMatin = Traitement.EnumFraction.Non,
            .FractionMidi = Traitement.EnumFraction.Non,
            .FractionApresMidi = Traitement.EnumFraction.Non,
            .FractionSoir = Traitement.EnumFraction.Non,
            .PosologieCommentaire = "",
            .Commentaire = "",
            .DateDebut = Date.Today,
            .DateFin = Date.Today.AddDays(60),
            .Fenetre = False,
            .FenetreCommentaire = "",
            .Arret = "",
            .ArretCommentaire = "",
            .Annulation = "",
            .AnnulationCommentaire = ""
        }
        Dim auteur As New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}
        Dim daoTraitement As New TraitementDao
        daoTraitement.CreationTraitement(nouveau, New TraitementHisto, auteur)
        Return CLng(Scalaire("SELECT MAX(oa_traitement_id) FROM oasis.oa_traitement WHERE oa_traitement_patient_id = @p0",
                             patientId))
    End Function

End Module
