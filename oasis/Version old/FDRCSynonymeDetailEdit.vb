Imports System.Data.SqlClient

Public Class FDRCSynonymeDetailEdit
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedDrcId As Integer
    Private privateSelectedDrcLibelle As String
    Private privateSelectedDrcSynonymeId As Integer
    Private privateSelectedDrcSynonyme As String
    Private privateCodeRetour As Boolean

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property SelectedDrcId As Integer
        Get
            Return privateSelectedDrcId
        End Get
        Set(value As Integer)
            privateSelectedDrcId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property SelectedDrcSynonymeId As Integer
        Get
            Return privateSelectedDrcSynonymeId
        End Get
        Set(value As Integer)
            privateSelectedDrcSynonymeId = value
        End Set
    End Property

    Public Property SelectedDrcSynonyme As String
        Get
            Return privateSelectedDrcSynonyme
        End Get
        Set(value As String)
            privateSelectedDrcSynonyme = value
        End Set
    End Property

    Public Property SelectedDrcLibelle As String
        Get
            Return privateSelectedDrcLibelle
        End Get
        Set(value As String)
            privateSelectedDrcLibelle = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()

    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub FNotePatientDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SelectedDrcId <> 0 Then
            LblDrcId.Text = SelectedDrcId.ToString
        Else
            LblDrcId.Text = ""
        End If

        If privateSelectedDrcLibelle <> "" Then
            LblDrcDescription.Text = SelectedDrcLibelle
        Else
            LblDrcDescription.Text = ""
        End If

        If SelectedDrcSynonymeId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            ChargementSynonyme()
        Else
            'Création
            EditMode = EnumEditMode.Creation
            TxtSynonyme.Text = ""
            'Inhiber boutons d'action de mise à jour
            BtnAnnuler.Hide()
        End If
    End Sub

    Private Sub ChargementSynonyme()
        TxtSynonyme.Text = SelectedDrcSynonyme
    End Sub

    'Modification d'une note patient en base de données
    Private Function ModificationSynonyme() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_drc_synonyme set oa_drc_synonyme_libelle = @synonyme where oa_drc_synonyme_id = @drcSynonymeId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@synonyme", TxtSynonyme.Text)
            .AddWithValue("@drcSynonymeId", SelectedDrcSynonymeId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Mot-clé modifié")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    'Création d'une note patient en base de données
    Private Function CreationSynonyme() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_drc_synonyme (oa_drc_id, oa_drc_synonyme_libelle) VALUES (@drcId, @synonyme)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            '.AddWithValue("@drcSynonymeId", SelectedDrcSynonymeId.ToString)
            .AddWithValue("@drcId", SelectedDrcId.ToString)
            .AddWithValue("@synonyme", TxtSynonyme.Text)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("Mot-clé créé")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour

    End Function

    Private Function AnnulationSynonyme() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "delete from oasis.oa_drc_synonyme where oa_drc_synonyme_id = @drcSynonymeId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@drcSynonymeId", privateSelectedDrcSynonymeId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Mot-clé supprimé")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        If EditMode = EnumEditMode.Creation Then
            If CreationSynonyme() = True Then
                CodeRetour = True
                Close()
            End If
        Else
            If EditMode = EnumEditMode.Modification Then
                If ModificationSynonyme() = True Then
                    CodeRetour = True
                    Close()
                End If
            End If
        End If

    End Sub

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub BtnAnnuler_Click(sender As Object, e As EventArgs) Handles BtnAnnuler.Click
        If MsgBox("confirmation de la suppression du mot-clé", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            If AnnulationSynonyme() = True Then
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub
End Class