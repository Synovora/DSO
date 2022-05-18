Imports Oasis_Common
Imports Oasis_Common.ParametreMail
Imports Telerik.WinControls.Enumerations

Public Class FrmMailOrdonnance
    Dim mailOasis As MailOasis
    Dim patient As Patient
    Dim ordonnance As Ordonnance
    Dim adrSiege As String = ""

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="patient"></param>
    ''' <param name="mailOasis"></param>
    Sub New(patient As Patient, ordonnance As Ordonnance, mailOasis As MailOasis)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' -- Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        AfficheTitleForm(Me, Me.Text, userLog)

        Me.mailOasis = mailOasis
        Me.patient = patient
        Me.ordonnance = ordonnance
        Me.mailOasis.IdSiege = patient.PatientSiegeId
        Me.mailOasis.IsSousEpisode = False

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        initFormulaire()


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initFormulaire()


        ' ------------------------------------ params mail
        Dim parametreMailDao As New ParametreMailDao
        Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(patient.PatientSiegeId, TypeMailParams.ORDONNANCE)

        ' ------------------------------------ params siege
        Try
            Dim siegeDao = New SiegeDao
            Dim siege = siegeDao.getSiegeById(patient.PatientSiegeId)
            With siege
                If .SiegeDescription.Trim() <> String.Empty Then adrSiege = .SiegeDescription & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeAdresse1.Trim() <> String.Empty Then adrSiege += .SiegeAdresse1 & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeAdresse2.Trim() <> String.Empty Then adrSiege += .SiegeAdresse2 & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If (.SiegeCodePostal + .SiegeVille) <> String.Empty Then adrSiege += .SiegeCodePostal & " " & .SiegeVille & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeTelephone.Trim() <> String.Empty Then adrSiege += "Tél : " & .SiegeTelephone & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeFax.Trim() <> String.Empty Then adrSiege += "Fax : " & .SiegeFax & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
                If .SiegeMail.Trim() <> String.Empty Then adrSiege += "Email : " & .SiegeMail & If(parametreMail.IsBodyHtml, "<br />", vbCrLf)
            End With
        Catch ex As Exception
        End Try


        Me.TxtObjet.Text = replaceZonesVariables(parametreMail.Objet)
        Me.TxtBody.Text = replaceZonesVariables(parametreMail.Body)

        ' ------------------------------------ pharmacie
        Dim emailPharmacien As String = ""

        If (patient.PharmacienId <> 0) Then
            Try
                Dim rorDao As New RorDao()
                Dim ror = rorDao.GetRorById(patient.PharmacienId)
                emailPharmacien = ror.Email
            Catch err As Exception
            End Try
        End If

        If IsValidEmail(emailPharmacien) Then
            rbPharmacie.Text += " : " & emailPharmacien
            rbPharmacie.Tag = emailPharmacien
        Else
            rbPharmacie.Text += " : " & "Pas d'email paramétré !"
            rbPharmacie.Enabled = False
        End If

        ' --------------------------------------------- patient
        If IsValidEmail(patient.PatientEmail) Then
            rbPatient.Text += " : " & patient.PatientEmail
            rbPatient.Tag = patient.PatientEmail
        Else
            rbPatient.Text += " : " & "Pas d'email paramétré !"
            rbPatient.Enabled = False
        End If

    End Sub

    Private Function replaceZonesVariables(source As String)
        Return source.Replace("@PatientNom", patient.PatientNom) _
                     .Replace("@PatientPrenom", patient.PatientPrenom) _
                     .Replace("@DateOrdonnance", ordonnance.DateValidation.ToString("dd/MM/yyyy")) _
                     .Replace("@SiegeCoord", adrSiege)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        If IsValidEmail(TxtTo.Text) = False Then
            MsgBox("Adresse destinataire incorrecte !", MsgBoxStyle.Exclamation, "Attention")
            Return
        End If

        If Trim(TxtObjet.Text) = "" Then
            MsgBox("L'objet ne peut être vide !", MsgBoxStyle.Exclamation, "Attention")
        Return
        End If

        If MsgBox("Envoyer cet Email ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question, "Validation") <> MsgBoxResult.Yes Then
            Return
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            With mailOasis
                .AliasFrom = ""
                .Body = TxtBody.Text
                .Subject = TxtObjet.Text
                .AdressTo = TxtTo.Text
            End With
            Using apiOasis As New ApiOasis()
                Dim ret = apiOasis.sendMailRest(loginRequestLog.login,
                              loginRequestLog.password,
                              mailOasis)
                Notification.show("Emission Email", "Email envoyé !")
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
        Me.Close()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub rbPharmacie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rbPharmacie.ToggleStateChanged
        If args.ToggleState = ToggleState.On Then
            Me.TxtTo.Text = rbPharmacie.Tag
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub rbPatient_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles rbPatient.ToggleStateChanged
        If args.ToggleState = ToggleState.On Then
            Me.TxtTo.Text = rbPatient.Tag
        End If

    End Sub


End Class
