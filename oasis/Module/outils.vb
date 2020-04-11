Imports System.DateTime
Imports System.Configuration
Imports System.Data.SqlClient
Imports Telerik.WinControls.UI
Imports Oasis_Common
Imports System.Runtime.InteropServices
Imports System.IO

Friend Module outils

    Public Const JoursAAjouterPourCalculAgePreScolaire As Integer = 4

    Dim logDao As New LogDao
    Dim log As Log

    Public Sub CreateLog(Description As String, FormName As String, TypeLog As String)
        If log Is Nothing Then
            log = New Log()
        End If
        If TypeLog = LogDao.EnumTypeLog.ERREUR.ToString Or TypeLog = LogDao.EnumTypeLog.INFO.ToString Then
        Else
            TypeLog = LogDao.EnumTypeLog.INFO.ToString
        End If
        log.Description = Description
        log.TypeLog = TypeLog
        log.Origine = FormName
        logDao.CreateLog(log)
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
            Try
                .FormElement.TitleBar.TitlePrimitive.StretchHorizontally = True
                .FormElement.TitleBar.TitlePrimitive.TextAlignment = ContentAlignment.MiddleCenter
                .FormElement.TitleBar.TitlePrimitive.ForeColor = Color.DarkBlue
                '.FormElement.TitleBar.TitlePrimitive.Font = New Font(.FormElement.TitleBar.Font, FontStyle.Bold)
            Catch
            Finally
            End Try
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
                    Age = lMois & " mois"
                Else
                    Age = "Nouveau né"
                End If
            Case 36 To 119
                Age = PatientAn & " ans " & PatientMoisRestant & " mois"
            Case Else
                Age = PatientAn.ToString & " ans"
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

    '========================================================================================
    '=== Envoi mail
    '========================================================================================
    Class MAPI
        Public Function AddRecipientTo(ByVal email As String) As Boolean
            Return AddRecipient(email, howTo.MAPI_TO)
        End Function

        Public Function AddRecipientCC(ByVal email As String) As Boolean
            Return AddRecipient(email, howTo.MAPI_TO)
        End Function

        Public Function AddRecipientBCC(ByVal email As String) As Boolean
            Return AddRecipient(email, howTo.MAPI_TO)
        End Function

        Public Sub AddAttachment(ByVal strAttachmentFileName As String)
            m_attachments.Add(strAttachmentFileName)
        End Sub

        Public Function SendMailPopup(ByVal strSubject As String,
                ByVal strBody As String) As Integer
            Return SendMail(strSubject, strBody, MAPI_LOGON_UI Or MAPI_DIALOG)
        End Function

        Public Function SendMailDirect(ByVal strSubject As String,
            ByVal strBody As String) As Integer
            Return SendMail(strSubject, strBody, MAPI_LOGON_UI)
        End Function


        <DllImport("MAPI32.DLL")>
        Private Shared Function MAPISendMail(ByVal sess As IntPtr,
             ByVal hwnd As IntPtr, ByVal message As MapiMessage,
             ByVal flg As Integer, ByVal rsv As Integer) As Integer
        End Function

        Private Function SendMail(ByVal strSubject As String,
            ByVal strBody As String, ByVal how As Integer) As Integer
            Dim msg As MapiMessage = New MapiMessage()
            msg.subject = strSubject
            msg.noteText = strBody

            msg.recips = GetRecipients(msg.recipCount)
            msg.files = GetAttachments(msg.fileCount)

            m_lastError = MAPISendMail(New IntPtr(0), New IntPtr(0), msg, how,
                0)
            If m_lastError > 1 Then
                MessageBox.Show("MAPISendMail failed! " + GetLastError(),
                    "MAPISendMail")
            End If

            Cleanup(msg)
            Return m_lastError
        End Function

        Private Function AddRecipient(ByVal email As String,
            ByVal howTo As howTo) As Boolean
            Dim recipient As MapiRecipDesc = New MapiRecipDesc()

            recipient.recipClass = CType(howTo, Integer)
            recipient.name = email
            m_recipients.Add(recipient)

            Return True
        End Function

        Private Function GetRecipients(ByRef recipCount As Integer) As IntPtr
            recipCount = 0
            If m_recipients.Count = 0 Then
                Return 0
            End If

            Dim size As Integer = Marshal.SizeOf(GetType(MapiRecipDesc))
            Dim intPtr As IntPtr = Marshal.AllocHGlobal(
                m_recipients.Count * size)

            Dim ptr As Integer = CType(intPtr, Integer)
            Dim mapiDesc As MapiRecipDesc
            For Each mapiDesc In m_recipients
                Marshal.StructureToPtr(mapiDesc, CType(ptr, IntPtr), False)
                ptr += size
            Next

            recipCount = m_recipients.Count
            Return intPtr
        End Function

        Private Function GetAttachments(ByRef fileCount As Integer) As IntPtr
            fileCount = 0
            If m_attachments Is Nothing Then
                Return 0
            End If

            If (m_attachments.Count <= 0) Or (m_attachments.Count >
                maxAttachments) Then
                Return 0
            End If

            Dim size As Integer = Marshal.SizeOf(GetType(MapiFileDesc))
            Dim intPtr As IntPtr = Marshal.AllocHGlobal(
                m_attachments.Count * size)

            Dim mapiFileDesc As MapiFileDesc = New MapiFileDesc()
            mapiFileDesc.position = -1
            Dim ptr As Integer = CType(intPtr, Integer)

            Dim strAttachment As String
            For Each strAttachment In m_attachments
                mapiFileDesc.name = Path.GetFileName(strAttachment)
                mapiFileDesc.path = strAttachment
                Marshal.StructureToPtr(mapiFileDesc, CType(ptr, IntPtr), False)
                ptr += size
            Next

            fileCount = m_attachments.Count
            Return intPtr
        End Function

        Private Sub Cleanup(ByRef msg As MapiMessage)
            Dim size As Integer = Marshal.SizeOf(GetType(MapiRecipDesc))
            Dim ptr As Integer = 0

            If msg.recips <> IntPtr.Zero Then
                ptr = CType(msg.recips, Integer)
                Dim i As Integer
                For i = 0 To msg.recipCount - 1 Step i + 1
                    Marshal.DestroyStructure(CType(ptr, IntPtr),
                        GetType(MapiRecipDesc))
                    ptr += size
                Next
                Marshal.FreeHGlobal(msg.recips)
            End If

            If msg.files <> IntPtr.Zero Then
                size = Marshal.SizeOf(GetType(MapiFileDesc))

                ptr = CType(msg.files, Integer)
                Dim i As Integer
                For i = 0 To msg.fileCount - 1 Step i + 1
                    Marshal.DestroyStructure(CType(ptr, IntPtr),
                        GetType(MapiFileDesc))
                    ptr += size
                Next
                Marshal.FreeHGlobal(msg.files)
            End If

            m_recipients.Clear()
            m_attachments.Clear()
            m_lastError = 0
        End Sub

        Public Function GetLastError() As String
            If m_lastError <= 26 Then
                Return errors(m_lastError)
            End If
            Return "MAPI error [" + m_lastError.ToString() + "]"
        End Function

        ReadOnly errors() As String = New String() {"OK [0]", "User abort [1]",
            "General MAPI failure [2]", "MAPI login failure [3]",
            "Disk full [4]", "Insufficient memory [5]", "Access denied [6]",
            "-unknown- [7]", "Too many sessions [8]",
            "Too many files were specified [9]",
            "Too many recipients were specified [10]",
            "A specified attachment was not found [11]",
            "Attachment open failure [12]", "Attachment write failure [13]",
            "Unknown recipient [14]", "Bad recipient type [15]",
            "No messages [16]", "Invalid message [17]", "Text too large [18]",
            "Invalid session [19]", "Type not supported [20]",
            "A recipient was specified ambiguously [21]",
            "Message in use [22]", "Network failure [23]",
            "Invalid edit fields [24]", "Invalid recipients [25]",
            "Not supported [26]"}

        Dim m_recipients As New List(Of MapiRecipDesc)
        Dim m_attachments As New List(Of String)
        Dim m_lastError As Integer = 0

        Private Const MAPI_LOGON_UI As Integer = &H1
        Private Const MAPI_DIALOG As Integer = &H8
        Private Const maxAttachments As Integer = 20

        Enum howTo
            MAPI_ORIG = 0
            MAPI_TO
            MAPI_CC
            MAPI_BCC
        End Enum

    End Class

    Public Class MapiMessage
        Public reserved As Integer
        Public subject As String
        Public noteText As String
        Public messageType As String
        Public dateReceived As String
        Public conversationID As String
        Public flags As Integer
        Public originator As IntPtr
        Public recipCount As Integer
        Public recips As IntPtr
        Public fileCount As Integer
        Public files As IntPtr
    End Class

    Public Class MapiFileDesc
        Public reserved As Integer
        Public flags As Integer
        Public position As Integer
        Public path As String
        Public name As String
        Public type As IntPtr
    End Class

    Public Class MapiRecipDesc
        Public reserved As Integer
        Public recipClass As Integer
        Public name As String
        Public address As String
        Public eIDSize As Integer
        Public enTryID As IntPtr
    End Class

End Module
