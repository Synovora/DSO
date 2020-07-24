﻿Imports System.IO

Public Class SousEpisodeReponse

    Property Id As Long
    Property IdSousEpisode As Long
    Property CreateUserId As Long
    Property HorodateCreation As DateTime
    Property NomFichier As String
    Property Commentaire As String


    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.IdSousEpisode = row("id_sous_episode")

        Me.CreateUserId = row("create_user_id")
        Me.HorodateCreation = row("horodate_creation")

        Me.NomFichier = Coalesce(row("nom_fichier"), "")
        Me.Commentaire = Coalesce(row("commentaire"), "")

    End Sub

    Public Function GetFilenameServer(idEpisode As Long, Optional idSEREponse As Long = 0) As String
        If idSEREponse = 0 Then idSEREponse = Me.Id
        Return "Episode_" & idEpisode & "_SousEpisode_" & Me.IdSousEpisode & "_SousEpisodeReponse_" & idSEREponse & Path.GetExtension(Me.NomFichier)
    End Function


End Class
