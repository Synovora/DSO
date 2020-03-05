Imports System.DateTime
Imports System.Configuration
Imports System.Data.SqlClient
Imports Telerik.WinControls.UI
Imports Oasis_Common

Friend Module outils

    Public Const JoursAAjouterPourCalculAgePreScolaire As Integer = 4

    Dim logDao As New LogDao
    Dim log As Log

    Public Sub CreateLog(Description As String, FormName As String, TypeLog As String)
        If Log Is Nothing Then
            Log = New Log()
        End If
        If TypeLog = LogDao.EnumTypeLog.ERREUR.ToString Or TypeLog = LogDao.EnumTypeLog.INFO.ToString Then
        Else
            TypeLog = LogDao.EnumTypeLog.INFO.ToString
        End If
        log.Description = Description
        log.TypeLog = TypeLog
        log.Origine = FormName
        logDao.CreateLog(Log)
    End Sub

    Public Function GetProfilUserString() As String
        Return " (" & userLog.UtilisateurPrenom.Trim &
                " " & userLog.UtilisateurNom.Trim &
                " -  " & userLog.UtilisateurProfilId.ToLower.Trim.Replace("_", " ") &
                " / " & userLog.TypeProfil.ToLower.Trim & ")"
    End Function

    Public Sub afficheTitleForm(form As RadForm, titre As String)
        ' --- centrage et chgt de style du titre du formulaire
        With form
            .Text = titre & " -" & GetProfilUserString() & " - " & String.Format("Version {0}", AssemblyVersion) & "   Date : " & Date.Now.ToString("dd.MM.yyyy")
            .FormElement.TitleBar.TitlePrimitive.StretchHorizontally = True
            .FormElement.TitleBar.TitlePrimitive.TextAlignment = ContentAlignment.MiddleCenter
            .FormElement.TitleBar.TitlePrimitive.ForeColor = Color.DarkBlue
            '.FormElement.TitleBar.TitlePrimitive.Font = New Font(.FormElement.TitleBar.Font, FontStyle.Bold)
        End With
    End Sub

    Public ReadOnly Property AssemblyVersion() As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        End Get
    End Property

    Public Function AccesFonctionMedicaleSynthese(patient As Patient) As Boolean
        Dim CodeRetour As Boolean = False

        If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse
            (userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString AndAlso patient.BlocageMedical = False) Then
            CodeRetour = True
        End If

        Return CodeRetour
    End Function

    Public Function AccesFonctionMedicale() As Boolean
        Dim CodeRetour As Boolean = False

        If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Then
            CodeRetour = True
        End If

        Return CodeRetour
    End Function

    Public Function CalculDureeEnJourEtHeureString(dateDebut As Date, dateFin As Date) As String
        Dim DureeString As String = ""
        Dim Duree As Integer

        Duree = DateDiff(DateInterval.Hour, dateDebut, dateFin)
        If Duree >= 24 Then
            Dim Jour As Integer = Fix(Duree / 24)
            Dim Heure As Integer = Duree Mod 24
            If Heure <> 0 Then
                DureeString = Jour.ToString & " jour(s) et " & Heure.ToString & " heure(s)"
            Else
                DureeString = Jour.ToString & " jour(s)"
            End If
        Else
            DureeString = Duree.ToString & " heure(s)"
        End If

        Return DureeString
    End Function

    Public Function CalculDureeEnJourString(dateDebut As Date, dateFin As Date) As String
        Dim DureeString As String = ""
        Dim Duree As Integer

        Duree = DateDiff(DateInterval.Day, dateDebut, dateFin)
        If Duree <= 31 Then
            If Duree <> 0 Then
                If Duree = 1 Then
                    DureeString = " 1 jour"
                Else
                    DureeString = Duree.ToString & " Jours"
                End If
            Else
                DureeString = " 0 jour"
            End If
        Else
            Duree = DateDiff(DateInterval.Month, dateDebut, dateFin)
            DureeString = Duree.ToString & " mois"
        End If

        Return DureeString
    End Function

    Public Function ConvertirEnJourDureeEnMois(Mois As Integer) As Integer
        Dim Duree As Integer
        Duree = Mois * 30.4375

        Return Duree
    End Function

    Public Function CalculAgeEnmois(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim mois As Integer

        mois = DateDiff("m", dateNaissance, datetimenow)

        Return mois
    End Function

    Public Function CalculAgeEnJour(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim Jour As Integer

        Jour = DateDiff("d", dateNaissance, datetimenow)

        Return Jour
    End Function
    Public Function getConnectionString() As String
        Dim SqlConnection As String
        SqlConnection = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString
        Return SqlConnection
    End Function

    'Calcul de la durée du traitement
    Public Function CalculDureeTraitementEnJour(dateDebut As Date, dateFin As Date) As Integer
        Dim duree As Integer
        Dim dateDebutaComparer As New Date(dateDebut.Year, dateDebut.Month, dateDebut.Day, 0, 0, 0)
        Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
        duree = DateDiff(DateInterval.Day, dateDebutaComparer, dateFinaComparer)
        duree += 1
        Return duree
    End Function

    'Calcul de la durée du traitement
    Public Function CalculDureeTraitementEnJourString(dateDebut As Date, dateFin As Date) As String
        Dim duree As String
        Dim jour As Integer
        Dim dateDebutaComparer As New Date(dateDebut.Year, dateDebut.Month, dateDebut.Day, 0, 0, 0)
        Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
        jour = DateDiff(DateInterval.Day, dateDebutaComparer, dateFinaComparer)
        jour += 1
        If jour = 1 Then
            duree = "1 jour"
        Else
            duree = jour.ToString + " jours"
        End If
        Return duree
    End Function

    Public Function FormatageDateAffichage(dateAFormater As Date, Optional DateComplete As Boolean = False) As String
        Dim dateCreationNote As String
        If DateComplete = True Then
            If dateAFormater.Month = Date.Now.Month And dateAFormater.Year = Date.Now.Year Then
                dateCreationNote = " " + dateAFormater.ToString("dd.MM.yyyy")
            Else
                If DateDiff(DateInterval.Year, Date.Now, dateAFormater) < 5 Then
                    dateCreationNote = " " + dateAFormater.ToString("MM.yyyy")
                Else
                    dateCreationNote = " " + dateAFormater.ToString("yyyy")
                End If
            End If
        Else
            If DateDiff(DateInterval.Year, Date.Now, dateAFormater) < 5 Then
                dateCreationNote = " " + dateAFormater.ToString("MM.yyyy")
            Else
                dateCreationNote = " " + dateAFormater.ToString("yyyy")
            End If
        End If

        Return dateCreationNote
    End Function

    Public Function CalculAgeEnAnneeEtMoisString(DateNaissance As Date) As String
        Dim lMois As Integer
        Dim Age As String
        Dim PatientMoisRestant, PatientAn As Integer
        lMois = CalculAgeEnmois(DateNaissance)
        If lMois > 35 Then
            PatientMoisRestant = lMois Mod 12
            lMois = lMois - PatientMoisRestant
            PatientAn = lMois / 12
        Else
            Dim lJour = CalculAgeEnJour(DateNaissance)
            lJour += JoursAAjouterPourCalculAgePreScolaire
            Dim lJourRestant = lJour Mod 30.4375
            lJour = lJour - lJourRestant
            lMois = lJour \ 30.4375
        End If
        Select Case lMois
            Case 0 To 35
                If lMois <> 0 Then
                    Age = "(" & lMois & " mois)"
                Else
                    Age = "(Nouveau né)"
                End If
            Case 36 To 119
                Age = "(" & PatientAn & " ans " & PatientMoisRestant & " mois)"
            Case Else
                Age = "(" & PatientAn.ToString & " ans)"
        End Select

        Return Age
    End Function

    Public Function CalculAgeEnAnnee(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim age As Integer

        age = CInt(Now.Year - dateNaissance.Year)

        If dateNaissance.Month > Now.Month Then
            age = age - 1
        End If

        If ((dateNaissance.Month = Now.Month) And (dateNaissance.Day > Now.Day)) Then
            age = age - 1
        End If

        Return age
    End Function

    Public Function CalculProchainRendezVous(dateReference As Date, rythme As Integer, Base As String) As Date
        Dim DateRendezVous As Date = Nothing
        If dateReference <> Nothing Then
            Select Case Base
                Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                    DateRendezVous = dateReference.AddDays(1)
                Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                    DateRendezVous = dateReference.AddDays(7)
                Case ParcoursDao.EnumParcoursBaseCode.ParMois
                    If rythme <> 0 Then
                        Dim Jour As Integer = 30 / rythme
                        DateRendezVous = dateReference.AddDays(Jour)
                    End If
                Case ParcoursDao.EnumParcoursBaseCode.ParAn
                    If rythme <> 0 Then
                        Dim Jour As Integer = 365 / rythme
                        DateRendezVous = dateReference.AddDays(Jour)
                    End If
                Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                    DateRendezVous = dateReference.AddYears(2)
                Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                    DateRendezVous = dateReference.AddYears(3)
                Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                    DateRendezVous = dateReference.AddYears(4)
                Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                    DateRendezVous = dateReference.AddYears(5)
            End Select
        End If

        Return DateRendezVous
    End Function

End Module
