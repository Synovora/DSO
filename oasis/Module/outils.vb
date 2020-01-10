Imports System.DateTime
Imports System.Configuration
Imports System.Data.SqlClient
Imports Telerik.WinControls.UI

Friend Module outils

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
            .Text = titre & " -" & GetProfilUserString() & " - " & String.Format("Version {0}", AssemblyVersion)
            .FormElement.TitleBar.TitlePrimitive.StretchHorizontally = True
            .FormElement.TitleBar.TitlePrimitive.TextAlignment = ContentAlignment.MiddleCenter
            .FormElement.TitleBar.TitlePrimitive.ForeColor = Color.DarkRed
            .FormElement.TitleBar.TitlePrimitive.Font = New Font(.FormElement.TitleBar.Font, FontStyle.Bold)
        End With
    End Sub

    Public ReadOnly Property AssemblyVersion() As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        End Get
    End Property

    Public Function CalculDuree(dateDebut As Date, dateFin As Date) As String
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

    Public Function CalculAge(dateNaissance As Date) As Integer
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

    Public Function Calculmois(dateNaissance As Date) As Integer
        Dim datetimenow = DateTime.Now
        Dim mois As Integer

        mois = DateDiff("m", dateNaissance, datetimenow)

        Return mois
    End Function
    Public Function getConnectionString() As String
        Dim SqlConnection As String
        SqlConnection = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString
        Return SqlConnection
    End Function

    'Calcul de la durée du traitement
    Public Function CalculDureeTraitement(dateDebut As Date, dateFin As Date) As String
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

    Public Function CalculAgeString(DateNaissance As Date) As String
        Dim lMois As Integer
        Dim Age As String
        Dim PatientMoisRestant, PatientAn As Integer
        lMois = Calculmois(DateNaissance)
        If lMois > 36 Then
            PatientMoisRestant = lMois Mod 12
            lMois = lMois - PatientMoisRestant
            PatientAn = lMois / 12
        End If
        Select Case lMois
            Case 0 To 35
                Age = "(" & lMois & " mois)"
            Case 36 To 119
                Age = "(" & PatientAn & " ans " & PatientMoisRestant & " mois)"
            Case Else
                Age = "(" & PatientAn.ToString & " ans)"
        End Select

        Return Age
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
