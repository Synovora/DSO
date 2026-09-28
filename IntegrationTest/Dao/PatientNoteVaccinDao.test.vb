Imports Oasis_Common

''' <summary>
''' PatientNoteVaccinDao contre la base (table oasis.oa_patient_note_vaccin). Notes de
''' vaccination du patient. Le client lourd (RadFPatientNoteListe,
''' RadFPatientNoteDetailEdit) crée, modifie, annule et liste : sous Compte.Client. Le
''' portail liste aussi ces notes (SyntheseController) : sous Compte.Web.
'''
''' À la différence des autres notes, chaque écriture met à jour la date de synthèse du
''' patient (PatientDao.ModificationDateMajSynthesePatient), qui bloque la fiche quand
''' l'auteur a un profil MEDICAL.
''' </summary>
<TestClass()> Public Class PatientNoteVaccinDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New PatientNoteVaccinDao

    Private Const TableNotes As String = "oa_patient_note_vaccin"
    Private Const NoteAbsente As Integer = 987654321

    ''' <summary>Crée une note par le DAO, comme la fenêtre de saisie, et renvoie son id.</summary>
    Private Function AjouterNote(patientId As Long, auteurId As Long, texte As String,
                                 Optional typeProfil As String = "PARAMEDICAL") As Long
        Assert.IsTrue(dao.CreationNote(New PatientNote With {
            .PatientId = CInt(patientId), .UserCreation = CInt(auteurId), .PatientNote = texte},
            Connecte(auteurId, typeProfil)))
        Return DerniereNotePatient(TableNotes, patientId)
    End Function

    ''' <summary>L'utilisateur connecté que la fenêtre passe en userLog.</summary>
    Private Shared Function Connecte(auteurId As Long, Optional typeProfil As String = "PARAMEDICAL") As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(auteurId), .TypeProfil = typeProfil}
    End Function

    Private Shared Function DateSynthese(patientId As Long) As Object
        Return Scalaire("SELECT oa_patient_synthese_date_maj FROM oasis.oa_patient WHERE oa_patient_id = @p0", patientId)
    End Function

    Private Shared Function FicheBloquee(patientId As Long) As Boolean
        Dim valeur = Scalaire("SELECT oa_patient_blocage_medical FROM oasis.oa_patient WHERE oa_patient_id = @p0", patientId)
        Return Not IsDBNull(valeur) AndAlso CBool(valeur)
    End Function

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_patient_note_id"))).ToArray()
    End Function

    <TestMethod()> Public Sub CreationNote_EnregistreLaNoteEtSonAuteur()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Dim id = AjouterNote(idPatient, idUtilisateur, "Premiere note")

        Dim relue = dao.getNoteById(CInt(id))
        Assert.AreEqual(id, CLng(relue.NoteId))
        Assert.AreEqual(idPatient, CLng(relue.PatientId))
        Assert.AreEqual("Premiere note", relue.PatientNote)
        Assert.AreEqual(idUtilisateur, CLng(relue.UserCreation))
        Assert.AreEqual(Date.Today, relue.DateCreation.Date)
        Assert.AreEqual(0, relue.UserModification)
        Assert.AreEqual(Date.MinValue, relue.DateModification, "jamais modifiée")
        Assert.IsFalse(relue.Invalide)
    End Sub

    <TestMethod()> Public Sub CreationNote_TexteAvecApostrophesEtGuillemets_ReluALIdentique()
        Dim texte = "L'infirmière a noté ""fièvre"" ; -- rien d'autre"
        Dim id = AjouterNote(CreerPatient(), CreerUtilisateur(avecCle:=False), texte)
        Assert.AreEqual(texte, dao.getNoteById(CInt(id)).PatientNote)
    End Sub

    <TestMethod()> Public Sub getNoteById_Inexistante_LeveArgumentException()
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.getNoteById(NoteAbsente))
    End Sub

    <TestMethod()> Public Sub getAllNoteVaccinbyPatient_DeLaPlusRecenteALaPlusAncienne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE")
        Dim ancienne = AjouterNote(idPatient, idUtilisateur, "Ancienne")
        Dim recente = AjouterNote(idPatient, idUtilisateur, "Recente")
        Dim moyenne = AjouterNote(idPatient, idUtilisateur, "Moyenne")
        AjouterNote(autrePatient, idUtilisateur, "Autre patient")
        PoserDateCreationNote(TableNotes, ancienne, New Date(2024, 1, 1, 8, 0, 0))
        PoserDateCreationNote(TableNotes, recente, New Date(2024, 3, 1, 8, 0, 0))
        PoserDateCreationNote(TableNotes, moyenne, New Date(2024, 2, 1, 8, 0, 0))

        Dim table = dao.getAllNoteVaccinbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {recente, moyenne, ancienne}, IdsDe(table))
        Assert.AreEqual("Recente", CStr(table.Rows(0)("oa_patient_note")))
        Assert.AreEqual(idUtilisateur, CLng(table.Rows(0)("oa_patient_note_utilisateur_creation")))
    End Sub

    <TestMethod()> Public Sub getAllNoteVaccinbyPatient_PatientSansNote_TableVide()
        AjouterNote(CreerPatient(), CreerUtilisateur(avecCle:=False), "Autre patient")
        Assert.AreEqual(0, dao.getAllNoteVaccinbyPatient(CInt(CreerPatient("SANS"))).Rows.Count)
    End Sub

    <TestMethod()> Public Sub getAllNoteVaccinbyPatient_IndicateurAZeroOuNul_NoteListee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim aZero = AjouterNote(idPatient, idUtilisateur, "A zero")
        Dim nulle = AjouterNote(idPatient, idUtilisateur, "Nulle")
        PoserInvalideNote(TableNotes, aZero, False)
        PoserInvalideNote(TableNotes, nulle, Nothing)

        CollectionAssert.AreEquivalent(New Long() {aZero, nulle}, IdsDe(dao.getAllNoteVaccinbyPatient(CInt(idPatient))))
        Assert.IsFalse(dao.getNoteById(CInt(nulle)).Invalide, "NULL se lit comme une note valide")
    End Sub

    <TestMethod()> Public Sub AnnulationNote_MarqueLaNoteEtLaRetireDeLaListe()
        Dim auteurCreation = CreerUtilisateur(avecCle:=False)
        Dim auteurAnnulation = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim gardee = AjouterNote(idPatient, auteurCreation, "Gardee")
        Dim annulee = AjouterNote(idPatient, auteurCreation, "Annulee")

        ' Comme la fenêtre de saisie : seuls l'id et l'auteur sont renseignés.
        Assert.IsTrue(dao.AnnulationNote(New PatientNote With {.NoteId = CInt(annulee), .UserModification = CInt(auteurAnnulation)},
                                         Connecte(auteurAnnulation)))

        CollectionAssert.AreEqual(New Long() {gardee}, IdsDe(dao.getAllNoteVaccinbyPatient(CInt(idPatient))))
        Dim relue = dao.getNoteById(CInt(annulee))
        Assert.IsTrue(relue.Invalide)
        Assert.AreEqual(auteurAnnulation, CLng(relue.UserModification))
        Assert.AreEqual(Date.Today, relue.DateModification.Date)
        Assert.AreEqual("Annulee", relue.PatientNote, "le texte reste en base")
        Assert.AreEqual(auteurCreation, CLng(relue.UserCreation))
    End Sub

    <TestMethod()> Public Sub ModificationNote_RemplaceLeTexteEtTraceLAuteur()
        Dim auteurCreation = CreerUtilisateur(avecCle:=False)
        Dim auteurModification = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = AjouterNote(idPatient, auteurCreation, "Avant")
        PoserDateCreationNote(TableNotes, id, New Date(2024, 1, 1, 8, 0, 0))

        ' Comme la fenêtre de saisie : ni le patient ni l'auteur de création ne sont renseignés.
        Assert.IsTrue(dao.ModificationNote(New PatientNote With {
            .NoteId = CInt(id), .UserModification = CInt(auteurModification), .PatientNote = "Apres"},
            Connecte(auteurModification)))

        Dim relue = dao.getNoteById(CInt(id))
        Assert.AreEqual("Apres", relue.PatientNote)
        Assert.AreEqual(auteurModification, CLng(relue.UserModification))
        Assert.AreEqual(Date.Today, relue.DateModification.Date)
        Assert.AreEqual(idPatient, CLng(relue.PatientId))
        Assert.AreEqual(auteurCreation, CLng(relue.UserCreation))
        Assert.AreEqual(New Date(2024, 1, 1, 8, 0, 0), relue.DateCreation)
        Assert.IsFalse(relue.Invalide)
    End Sub

    <TestMethod()> Public Sub ModificationNote_NoteAnnulee_ResteAnnulee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = AjouterNote(idPatient, idUtilisateur, "Avant")
        dao.AnnulationNote(New PatientNote With {.NoteId = CInt(id), .UserModification = CInt(idUtilisateur)}, Connecte(idUtilisateur))

        dao.ModificationNote(New PatientNote With {.NoteId = CInt(id), .UserModification = CInt(idUtilisateur), .PatientNote = "Apres"},
                             Connecte(idUtilisateur))

        Assert.IsTrue(dao.getNoteById(CInt(id)).Invalide)
        Assert.AreEqual(0, dao.getAllNoteVaccinbyPatient(CInt(idPatient)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub ModificationEtAnnulation_NoteInexistante_RenvoientVrai()
        ' Comportement actuel : aucune ligne touchée n'est pas une erreur.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Assert.IsTrue(dao.ModificationNote(New PatientNote With {.NoteId = NoteAbsente, .UserModification = CInt(idUtilisateur), .PatientNote = "x"},
                                           Connecte(idUtilisateur)))
        Assert.IsTrue(dao.AnnulationNote(New PatientNote With {.NoteId = NoteAbsente, .UserModification = CInt(idUtilisateur)},
                                         Connecte(idUtilisateur)))
    End Sub

    ' ---------------------------------------------------------------------
    ' Date de synthèse et blocage de la fiche
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationNote_ParUnMedecin_DateDeSyntheseDuJourEtFicheBloquee()
        Dim idPatient = CreerPatient()
        PoserDateMajSynthese(idPatient, New Date(2020, 1, 1))

        AjouterNote(idPatient, CreerUtilisateur(avecCle:=False), "Rappel DTP", "MEDICAL")

        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
        Assert.IsTrue(FicheBloquee(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationNote_ParUnNonMedecin_DateDeSyntheseDuJourSansBlocage()
        Dim idPatient = CreerPatient()
        PoserDateMajSynthese(idPatient, New Date(2020, 1, 1))

        AjouterNote(idPatient, CreerUtilisateur(avecCle:=False), "Rappel DTP", "PARAMEDICAL")

        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
        Assert.IsFalse(FicheBloquee(idPatient))
    End Sub

    <TestMethod()> Public Sub ModificationNote_CommeLaFenetre_NeMetPasAJourLaSynthese()
        ' Comportement actuel : RadFPatientNoteDetailEdit ne renseigne pas PatientId
        ' avant ModificationNote. La mise à jour de synthèse vise alors le patient 0 :
        ' ni la date de synthèse ni le blocage du vrai patient ne bougent.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = AjouterNote(idPatient, idUtilisateur, "Avant")
        PoserDateMajSynthese(idPatient, New Date(2020, 1, 1))

        Assert.IsTrue(dao.ModificationNote(New PatientNote With {
            .NoteId = CInt(id), .UserModification = CInt(idUtilisateur), .PatientNote = "Apres"},
            Connecte(idUtilisateur, "MEDICAL")))

        Assert.AreEqual("Apres", dao.getNoteById(CInt(id)).PatientNote)
        Assert.AreEqual(New Date(2020, 1, 1), CDate(DateSynthese(idPatient)).Date)
        Assert.IsFalse(FicheBloquee(idPatient))
    End Sub

    <TestMethod()> Public Sub ModificationNote_AvecLePatient_MetAJourLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = AjouterNote(idPatient, idUtilisateur, "Avant")
        PoserDateMajSynthese(idPatient, New Date(2020, 1, 1))

        dao.ModificationNote(New PatientNote With {
            .NoteId = CInt(id), .PatientId = CInt(idPatient), .UserModification = CInt(idUtilisateur), .PatientNote = "Apres"},
            Connecte(idUtilisateur, "MEDICAL"))

        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
        Assert.IsTrue(FicheBloquee(idPatient))
    End Sub

    <TestMethod()> Public Sub AnnulationNote_CommeLaFenetre_NeMetPasAJourLaSynthese()
        ' Comportement actuel : même oubli de PatientId que pour la modification.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = AjouterNote(idPatient, idUtilisateur, "Annulee")
        PoserDateMajSynthese(idPatient, New Date(2020, 1, 1))

        Assert.IsTrue(dao.AnnulationNote(New PatientNote With {.NoteId = CInt(id), .UserModification = CInt(idUtilisateur)},
                                         Connecte(idUtilisateur, "MEDICAL")))

        Assert.IsTrue(dao.getNoteById(CInt(id)).Invalide)
        Assert.AreEqual(New Date(2020, 1, 1), CDate(DateSynthese(idPatient)).Date)
        Assert.IsFalse(FicheBloquee(idPatient))
    End Sub

    <TestMethod()> Public Sub AnnulationNote_AvecLePatient_MetAJourLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = AjouterNote(idPatient, idUtilisateur, "Annulee")
        PoserDateMajSynthese(idPatient, New Date(2020, 1, 1))

        dao.AnnulationNote(New PatientNote With {.NoteId = CInt(id), .PatientId = CInt(idPatient), .UserModification = CInt(idUtilisateur)},
                           Connecte(idUtilisateur, "PARAMEDICAL"))

        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
        Assert.IsFalse(FicheBloquee(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationNote_PatientInexistant_EcritLaNotePuisEchoue()
        ' Comportement actuel : la note est insérée, puis la mise à jour de synthèse lève
        ' « Patient inexistant ». La note orpheline reste en base.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)

        Assert.ThrowsException(Of ArgumentException)(
            Sub() dao.CreationNote(New PatientNote With {.PatientId = NoteAbsente, .UserCreation = CInt(idUtilisateur), .PatientNote = "x"},
                                   Connecte(idUtilisateur)))

        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_note_vaccin WHERE oa_patient_id = @p0", NoteAbsente)))
    End Sub

    ' ---------------------------------------------------------------------
    ' Portail
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub getAllNoteVaccinbyPatient_SousWeb_CommeLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ancienne = AjouterNote(idPatient, idUtilisateur, "Ancienne")
        Dim recente = AjouterNote(idPatient, idUtilisateur, "Recente")
        Dim annulee = AjouterNote(idPatient, idUtilisateur, "Annulee")
        PoserDateCreationNote(TableNotes, ancienne, New Date(2024, 1, 1, 8, 0, 0))
        PoserDateCreationNote(TableNotes, recente, New Date(2024, 3, 1, 8, 0, 0))
        dao.AnnulationNote(New PatientNote With {.NoteId = CInt(annulee), .PatientId = CInt(idPatient), .UserModification = CInt(idUtilisateur)},
                           Connecte(idUtilisateur))
        UtiliserCompte(Compte.Web)

        Dim table = dao.getAllNoteVaccinbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {recente, ancienne}, IdsDe(table))
        Assert.AreEqual("Recente", CStr(table.Rows(0)("oa_patient_note")))
    End Sub

End Class
