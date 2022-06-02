Imports System.IO
Imports Oasis_Common
Imports Oasis_Common.ParametreMail
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.FormatProviders.Pdf

Public Class FrmMailSousEpisodeOuSynthese

    Dim mailOasis As MailOasis
    Dim patient As Patient
    Dim sousEpisode As SousEpisode

    Dim adrSiege As String = ""

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="patient"></param>
    ''' <param name="mailOasis"></param>
    Sub New(patient As Patient, objet As Object, mailOasis As MailOasis)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        Select Case mailOasis.Type
            Case ParametreMail.TypeMailParams.CARNET_VACCINAL
                Me.Text = "Envoi d'un carnet vaccinal par email"
            Case ParametreMail.TypeMailParams.SYNTHESE
                Me.Text = "Envoi d'une synthèse en email"
            Case ParametreMail.TypeMailParams.SOUS_EPISODE
                Me.Text = "Envoi d'un sous-épisode en email"
                Me.sousEpisode = TryCast(objet, SousEpisode)
        End Select

        ' -- Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        AfficheTitleForm(Me, Me.Text, userLog)

        Me.mailOasis = mailOasis
        Me.patient = patient
        Me.mailOasis.IdSiege = patient.PatientSiegeId

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        initFormulaire()


    End Sub

    Private Sub initFormulaire()

        ' ------------------------------------ params mail
        Dim parametreMailDao As New ParametreMailDao
        Dim parametreMail = parametreMailDao.GetParametreMailBySiegeIdTypeMailParam(patient.PatientSiegeId, mailOasis.Type)

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

        refreshGrid()

        Me.TxtObjet.Text = replaceZonesVariables(parametreMail.Objet)
        Me.TxtBody.Text = replaceZonesVariables(parametreMail.Body)

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
        Select Case mailOasis.Type
            Case ParametreMail.TypeMailParams.CARNET_VACCINAL
                source = source.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
            Case ParametreMail.TypeMailParams.SYNTHESE
                source = source.Replace("@Reference", If(sousEpisode IsNot Nothing, sousEpisode.Reference, ""))
                source = source.Replace("@DateCreation", Now.ToString("dd/MM/yyyy"))
            Case Else
                source = source.Replace("@DateCreation", If(sousEpisode IsNot Nothing, sousEpisode.HorodateCreation.ToString("dd/MM/yyyy"), ""))
        End Select
        Return source.Replace("@PatientNom", patient.PatientNom) _
                     .Replace("@PatientPrenom", patient.PatientPrenom) _
                     .Replace("@SiegeCoord", adrSiege)
    End Function

    Public Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        Dim isDejaOk As Boolean = False
        Dim tbl As String() = TxtTo.Text.Split(",")
        If tbl.Length > 1 Then
            If MsgBox("Il y a plusieurs adresses email dans le champs destinataire ! " & vbCrLf & "Envoyer à tous ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question, "Attention") <> MsgBoxResult.Yes Then
                Return
            End If
            isDejaOk = True
        End If
        For Each adr In tbl
            If IsValidEmail(adr) = False Then
                MsgBox("Adresse destinataire """ & adr & """incorrecte !", MsgBoxStyle.Exclamation, "Attention")
                Return
            End If
        Next

        If Trim(TxtObjet.Text) = "" Then
            MsgBox("L'objet ne peut être vide !", MsgBoxStyle.Exclamation, "Attention")
            Return
        End If

        If isDejaOk = False Then
            If MsgBox("Envoyer cet Email ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Question, "Validation") <> MsgBoxResult.Yes Then
                Return
            End If
        End If

        Me.Send()
        Me.Close()

    End Sub

    Public Sub Send()
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            If mailOasis.Type = ParametreMail.TypeMailParams.SYNTHESE OrElse mailOasis.Type = ParametreMail.TypeMailParams.CARNET_VACCINAL Then mailOasis.ConvertToPdf()
            With mailOasis
                .AliasFrom = ""
                .Body = TxtBody.Text
                .Subject = TxtObjet.Text
                .AdressTo = TxtTo.Text
            End With
            mailOasis.Send(loginRequestLog)
            Notification.show("Emission Email", "Email envoyé !")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur Envoi Email")
            Return
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Dim parcourdDao = New ParcoursDao
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = parcourdDao.GetAllEmailParcoursbyPatient(patient.PatientId)
            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadParcoursGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadParcoursGrid.CurrentRow) Then
                exId = Me.RadParcoursGrid.CurrentRow.Cells("IdRor").Value
                exPosit = Me.RadParcoursGrid.CurrentRow.Index
            End If
            RadParcoursGrid.Rows.Clear()

            For Each row In data.Rows
                Dim newRow As GridViewRowInfo = RadParcoursGrid.Rows.NewRow()
                '------------------- Alimentation du DataGridView
                With newRow
                    .Cells("IdRor").Value = row("oa_ror_id")
                    If .Cells("idRor").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("specialite").Value = row("oa_r_specialite_description")
                    .Cells("nomIntervenant").Value = row("oa_ror_nom")
                    .Cells("email").Value = row("email")

                    ' -- on garnit le tag pour affichage tooltip
                    newRow.Tag = Coalesce(row("oa_r_specialite_description"), "") & vbCrLf &
                                 Coalesce(row("oa_ror_nom"), "") & vbCrLf &
                                 Coalesce(row("email"), "") & vbCrLf

                End With
                RadParcoursGrid.Rows.Add(newRow)
                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                Me.RadParcoursGrid.CurrentRow = Nothing 'RadParcoursGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub rbPatient_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles rbPatient.ToggleStateChanged
        If args.ToggleState = ToggleState.On Then
            Me.TxtTo.Text = rbPatient.Tag
        End If
    End Sub

    Private Sub MasterTemplate_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RbParcours_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RbParcours.ToggleStateChanged
        If args.ToggleState = ToggleState.On AndAlso Not (Me.RadParcoursGrid.CurrentRow Is Nothing _
                OrElse Me.RadParcoursGrid.Rows.Count = 0 _
                OrElse Me.RadParcoursGrid.CurrentRow.IsSelected = False) Then
            Me.TxtTo.Text = Me.RadParcoursGrid.CurrentRow.Cells("email").Value
        End If

    End Sub

    Private Sub RadParcoursGrid_Click(sender As Object, e As EventArgs) Handles RadParcoursGrid.Click
        If Not (Me.RadParcoursGrid.CurrentRow Is Nothing _
                OrElse Me.RadParcoursGrid.Rows.Count = 0 _
                OrElse Me.RadParcoursGrid.CurrentRow.IsSelected = False) Then
            RbParcours.CheckState = ToggleState.Off
            RbParcours.CheckState = ToggleState.On
        End If
    End Sub
End Class
