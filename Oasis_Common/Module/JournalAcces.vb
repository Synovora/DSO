''' <summary>
''' Journal des accès aux dossiers.
'''
''' oa_action ne recevait que des actes métier écrits ici ou là par le client
''' lourd : aucune consultation n'y figurait, alors que la traçabilité des accès
''' aux données de santé porte sur la lecture autant que sur l'écriture.
'''
''' Ce module écrit dans la même table, à partir du serveur, pour les accès qui
''' passent par l'API. Il ne remplace pas un journal inviolable : oa_action reste
''' une table ordinaire, que le compte du serveur peut modifier. Un journal
''' réellement opposable demande une table en ajout seul et un audit SQL Server,
''' prévus à la phase suivante.
'''
''' Une écriture de journal qui échoue ne doit jamais faire échouer l'acte tracé :
''' les erreurs sont absorbées.
''' </summary>
Public Module JournalAcces

    ''' <summary>Consultation d'une donnée rattachée à un patient.</summary>
    Public Sub Consultation(utilisateur As Utilisateur, patientId As Long, libelle As String)
        Ecrire(utilisateur, patientId, "CONSULTATION : " & libelle)
    End Sub

    ''' <summary>Modification d'une donnée rattachée à un patient.</summary>
    Public Sub Modification(utilisateur As Utilisateur, patientId As Long, libelle As String)
        Ecrire(utilisateur, patientId, "MODIFICATION : " & libelle)
    End Sub

    ''' <summary>
    ''' Acte sortant : envoi de courriel, export. Le destinataire fait partie de la
    ''' trace, c'est lui qui donne son sens à l'entrée.
    ''' </summary>
    Public Sub Sortie(utilisateur As Utilisateur, patientId As Long, libelle As String)
        Ecrire(utilisateur, patientId, "SORTIE : " & libelle)
    End Sub

    ''' <summary>Refus opposé à une demande. Une trace de refus vaut celle d'un accès.</summary>
    Public Sub AccesRefuse(utilisateur As Utilisateur, patientId As Long, libelle As String)
        Ecrire(utilisateur, patientId, "REFUS : " & libelle)
    End Sub

    Private Sub Ecrire(utilisateur As Utilisateur, patientId As Long, libelle As String)
        Try
            If utilisateur Is Nothing Then Exit Sub

            Dim actionDao As New ActionDao
            actionDao.CreationAction(New Action With {
                .UtilisateurId = utilisateur.UtilisateurId,
                .PatientId = patientId,
                .Horodatage = Date.Now,
                .Action = Tronquer(libelle, 400),
                .Fonction = If(utilisateur.UtilisateurProfilId, ""),
                .FonctionId = utilisateur.FonctionParDefautId
            })
        Catch ex As Exception
            ' Journal indisponible : l'acte tracé reste valide. La perte de trace
            ' est un incident d'exploitation, pas un motif de refus de soin.
        End Try
    End Sub

    Private Function Tronquer(valeur As String, longueurMaxi As Integer) As String
        If String.IsNullOrEmpty(valeur) Then Return ""
        If valeur.Length <= longueurMaxi Then Return valeur
        Return valeur.Substring(0, longueurMaxi)
    End Function

End Module
