Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par PatientNoteDirectiveDao.buildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestPatientNoteDirectiveDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_patient_note_id", "oa_patient_id", "oa_patient_note",
        "oa_patient_note_utilisateur_creation", "oa_patient_note_date_creation",
        "oa_patient_note_utilisateur_modification", "oa_patient_note_date_modification",
        "oa_patient_note_invalide"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = PatientNoteDirectiveDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_patient_note_id", 101},
            {"oa_patient_id", 102},
            {"oa_patient_note", "valeur_3"},
            {"oa_patient_note_utilisateur_creation", 104},
            {"oa_patient_note_date_creation", New Date(2024, 6, 6)},
            {"oa_patient_note_utilisateur_modification", 106},
            {"oa_patient_note_date_modification", New Date(2024, 8, 8)},
            {"oa_patient_note_invalide", True}}))

        Assert.AreEqual(101, b.NoteId)
        Assert.AreEqual(102, b.PatientId)
        Assert.AreEqual("valeur_3", b.PatientNote)
        Assert.AreEqual(104, b.UserCreation)
        Assert.AreEqual(New Date(2024, 6, 6), b.DateCreation)
        Assert.AreEqual(106, b.UserModification)
        Assert.AreEqual(New Date(2024, 8, 8), b.DateModification)
        Assert.AreEqual(True, b.Invalide)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = PatientNoteDirectiveDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_patient_note_id", 101}}))

        Assert.AreEqual(0, b.PatientId)
        Assert.AreEqual("", b.PatientNote)
        Assert.AreEqual(0, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0, b.UserModification)
        Assert.AreEqual(Date.MinValue, b.DateModification)
        Assert.AreEqual(False, b.Invalide)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansNoteIdEstUneErreur()
        ' oa_patient_note_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_patient_note_id", 101}}
        valeurs.Remove("oa_patient_note_id")
        PatientNoteDirectiveDao.buildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
