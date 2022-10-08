﻿Imports System.IO

Public Class SousEpisodeReponse

    Property Id As Long
    Property IdSousEpisode As Long
    Property CreateUserId As Long
    Property HorodateCreation As DateTime
    Property NomFichier As String
    Property Commentaire As String
    Property ValidateState As String
    Property ValidateUserId As Long
    Property ValidateDate As DateTime
    Property EpisodeId As Long

    '--
    Property SousEpisodeLibelle As String
    Property SousEpisodeSousLibelle As String
    Property TypeActivite As String
    Property Conclusion As String



    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.IdSousEpisode = row("id_sous_episode")
        Me.CreateUserId = row("create_user_id")
        Me.HorodateCreation = row("horodate_creation")
        Me.NomFichier = Coalesce(row("nom_fichier"), "")
        Me.Commentaire = Coalesce(row("commentaire"), "")
        Me.ValidateState = Coalesce(row("validate_state"), "!")
        Me.ValidateUserId = Coalesce(row("validate_user_id"), Nothing)
        Me.ValidateDate = Coalesce(row("validate_date"), Nothing)
        Me.EpisodeId = Coalesce(row("episode_id"), Nothing)
        Me.SousEpisodeLibelle = If(row.Table.Columns.Contains("sous_episode_libelle"), row("sous_episode_libelle"), Nothing)
        Me.SousEpisodeSousLibelle = If(row.Table.Columns.Contains("sous_episode_sous_libelle"), row("sous_episode_sous_libelle"), Nothing)
        Me.TypeActivite = If(row.Table.Columns.Contains("type_activite"), row("type_activite"), Nothing)

    End Sub

    Public Function GetFilenameServer(idEpisode As Long, Optional idSEREponse As Long = 0) As String
        If idSEREponse = 0 Then idSEREponse = Me.Id
        Dim folder = "SousEpisodeReponse"
        Return folder & "\" & "Episode_" & idEpisode & "_SousEpisode_" & Me.IdSousEpisode & "_SousEpisodeReponse_" & idSEREponse & Path.GetExtension(Me.NomFichier)
    End Function


End Class
