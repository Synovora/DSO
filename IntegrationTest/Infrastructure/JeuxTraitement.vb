Imports Oasis_Common

''' <summary>
''' Traitements de test, en complément de JeuxOrdonnance.CreerTraitement. Les
''' traitements passent par TraitementDao (création, déclaration d'allergie ou de
''' contre-indication). Seules la fenêtre thérapeutique, écrite par un écran et non
''' par un DAO, et les tables de la base médicamenteuse (Theriaque / BDPM, que
''' l'application ne fait que lire) sont remplies en SQL brut.
''' </summary>
Public Module JeuxTraitement

    ''' <summary>
    ''' Traitement journalier « 1 matin, 1 soir » tel que l'écran de saisie le remplit,
    ''' non enregistré. Les dates sont à minuit pour être relues à l'identique.
    ''' </summary>
    Function TraitementDeTest(patientId As Long, rang As Integer,
                              Optional dateDebut As Date? = Nothing,
                              Optional dateFin As Date? = Nothing) As Traitement
        Return New Traitement With {
            .PatientId = CInt(patientId),
            .MedicamentId = CisDeTest + rang,
            .MedicamentMonographie = False,
            .MedicamentDci = "DCI TEST " & rang,
            .DenominationLongue = "MEDICAMENT TEST " & rang & " 500 mg, comprimé",
            .ClasseAtc = "N02BE01",
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
            .PosologieCommentaire = "Pendant le repas",
            .Commentaire = "Commentaire " & rang,
            .DateDebut = If(dateDebut, Date.Today),
            .DateFin = If(dateFin, Date.Today.AddDays(60)),
            .Fenetre = False,
            .FenetreCommentaire = "",
            .Arret = "",
            .ArretCommentaire = "",
            .Annulation = "",
            .AnnulationCommentaire = ""
        }
    End Function

    ''' <summary>
    ''' Enregistre le traitement par TraitementDao.CreationTraitement, comme l'écran
    ''' de saisie, et renvoie son id (relu en base : le DAO ne le rend pas).
    ''' </summary>
    Function EnregistrerTraitement(traitement As Traitement, utilisateurId As Long) As Long
        Dim auteur As New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}
        Dim daoTraitement As New TraitementDao
        daoTraitement.CreationTraitement(traitement, New TraitementHisto, auteur)
        Return CLng(Scalaire("SELECT MAX(oa_traitement_id) FROM oasis.oa_traitement WHERE oa_traitement_patient_id = @p0",
                             traitement.PatientId))
    End Function

    ''' <summary>
    ''' Déclare une allergie ou une contre-indication hors traitement par
    ''' TraitementDao.DeclarationTraitementAllergieOuCI, comme RadFDeclarationAllergieEtCIDetail.
    ''' Renvoie l'id de la ligne créée.
    ''' </summary>
    Function DeclarerAllergieOuCI(patientId As Long, utilisateurId As Long, cis As Integer, dci As String,
                                  allergie As Boolean, Optional commentaire As String = "Déclaration") As Long
        Dim declaration As New Traitement With {
            .PatientId = CInt(patientId),
            .MedicamentId = cis,
            .MedicamentDci = dci,
            .Allergie = allergie,
            .ContreIndication = Not allergie,
            .ArretCommentaire = commentaire
        }
        Dim daoTraitement As New TraitementDao
        daoTraitement.DeclarationTraitementAllergieOuCI(declaration, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Return CLng(Scalaire("SELECT MAX(oa_traitement_id) FROM oasis.oa_traitement WHERE oa_traitement_patient_id = @p0",
                             patientId))
    End Function

    ''' <summary>
    ''' Pose une fenêtre thérapeutique sur un traitement. L'écran RadFTraitementFenetreTh
    ''' écrit ces colonnes lui-même : aucun DAO ne le fait.
    ''' </summary>
    Sub PoserFenetreTherapeutique(traitementId As Long, debut As Date, fin As Date,
                                  Optional commentaire As String = "Fenêtre de test")
        Executer("UPDATE oasis.oa_traitement SET oa_traitement_fenetre = 1, oa_traitement_fenetre_date_debut = @p0," &
                 " oa_traitement_fenetre_date_fin = @p1, oa_traitement_fenetre_commentaire = @p2" &
                 " WHERE oa_traitement_id = @p3", debut, fin, commentaire, traitementId)
    End Sub

    ''' <summary>Fixe la date de modification d'un traitement, pour éprouver un tri sans dépendre de l'horloge.</summary>
    Sub FixerDateModificationTraitement(traitementId As Long, quand As Date)
        Executer("UPDATE oasis.oa_traitement SET oa_traitement_date_modification = @p0 WHERE oa_traitement_id = @p1",
                 quand, traitementId)
    End Sub

    ''' <summary>
    ''' Ligne de composition d'un médicament (oa_r_medicament_compo, reprise de la
    ''' base médicamenteuse) : nature « SA » pour une substance active, « FT » pour
    ''' une fraction thérapeutique.
    ''' </summary>
    Sub AjouterCompositionMedicament(cis As Integer, nature As String, denomination As String)
        Executer("INSERT INTO oasis.oa_r_medicament_compo" &
                 " (oa_r_medicament_compo_cis, oa_r_medicament_compo_nature, oa_r_medicament_compo_denomination)" &
                 " VALUES (@p0, @p1, @p2)", cis, nature, denomination)
    End Sub

    ''' <summary>
    ''' Rattache un médicament à un groupe générique (oa_medicament_gener, reprise de
    ''' la base médicamenteuse : un groupe, plusieurs lignes, une par CIS).
    ''' </summary>
    Sub AjouterAuGroupeGenerique(groupeId As Integer, cis As Integer)
        Dim insertion = "INSERT INTO oasis.oa_medicament_gener (oa_medicament_gener_id, oa_medicament_gener_cis) VALUES (@p0, @p1)"
        Dim identite = Scalaire("SELECT COLUMNPROPERTY(OBJECT_ID('oasis.oa_medicament_gener'), 'oa_medicament_gener_id', 'IsIdentity')")
        If identite IsNot Nothing AndAlso Not IsDBNull(identite) AndAlso CInt(identite) = 1 Then
            insertion = "SET IDENTITY_INSERT oasis.oa_medicament_gener ON; " & insertion &
                        "; SET IDENTITY_INSERT oasis.oa_medicament_gener OFF;"
        End If
        Executer(insertion, groupeId, cis)
    End Sub

End Module
