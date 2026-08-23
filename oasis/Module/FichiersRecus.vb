Imports System.IO
Imports System.Diagnostics

''' <summary>
''' Écriture et ouverture des fichiers reçus de l'extérieur (pièces jointes de
''' mails, documents de réponse à un sous-épisode).
'''
''' Le nom d'origine est choisi par le correspondant : l'utiliser tel quel comme
''' chemin puis le passer à Process.Start revient à exécuter ce qu'il envoie
''' (.exe, .bat, .js, .hta, .lnk sont lancés par le shell) et permet d'écrire
''' hors du dossier de téléchargement avec un nom contenant "..\".
''' On écrit donc sous un nom que l'application choisit, et on n'ouvre que des
''' types de documents connus.
''' </summary>
Public Module FichiersRecus

    ''' <summary>Extensions ouvrables sans risque d'exécution de code.</summary>
    Private ReadOnly ExtensionsAutorisees As String() = {
        ".pdf", ".docx", ".doc", ".odt", ".rtf", ".txt", ".csv",
        ".xlsx", ".xls", ".pptx", ".ppt",
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tif", ".tiff",
        ".eml"
    }

    ''' <summary>
    ''' Dossier de cache propre à l'utilisateur. L'ancien emplacement
    ''' (c:\oasis\telechargement) est lisible par tous les comptes de la machine,
    ''' alors qu'il contient des documents médicaux.
    ''' </summary>
    Public Function DossierCache() As String
        Dim dossier = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Oasis", "cache")
        Directory.CreateDirectory(dossier)
        Return dossier
    End Function

    ''' <summary>
    ''' Vrai si l'extension du nom fourni figure dans la liste autorisée.
    ''' </summary>
    Public Function EstExtensionAutorisee(nomOrigine As String) As Boolean
        If String.IsNullOrWhiteSpace(nomOrigine) Then Return False
        Dim ext = Path.GetExtension(nomOrigine).ToLowerInvariant()
        Return ExtensionsAutorisees.Contains(ext)
    End Function

    ''' <summary>
    ''' Écrit le contenu dans le cache sous un nom généré par l'application et
    ''' renvoie le chemin obtenu. L'extension d'origine est conservée uniquement
    ''' si elle est autorisée ; sinon la fonction lève une exception.
    ''' </summary>
    Public Function EcrireDansCache(nomOrigine As String, contenu As Byte()) As String
        If Not EstExtensionAutorisee(nomOrigine) Then
            Throw New NotSupportedException(
                "Type de fichier non autorisé : " & Path.GetExtension(If(nomOrigine, "")) & vbCrLf &
                "Ce document ne peut pas être ouvert depuis Oasis.")
        End If
        Dim ext = Path.GetExtension(nomOrigine).ToLowerInvariant()
        Dim chemin = Path.Combine(DossierCache(), Guid.NewGuid().ToString("N") & ext)
        File.WriteAllBytes(chemin, contenu)
        Return chemin
    End Function

    ''' <summary>
    ''' Écrit le contenu dans le cache puis l'ouvre avec l'application associée.
    ''' Renvoie le chemin écrit, ou Nothing si le type n'est pas autorisé (un
    ''' message a alors déjà été affiché à l'utilisateur).
    ''' </summary>
    Public Function EcrireEtOuvrir(nomOrigine As String, contenu As Byte()) As String
        Dim chemin As String
        Try
            chemin = EcrireDansCache(nomOrigine, contenu)
        Catch ex As NotSupportedException
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Pièce jointe")
            Return Nothing
        End Try

        Try
            Process.Start(New ProcessStartInfo(chemin) With {.UseShellExecute = True})
        Catch err As Exception
            MsgBox("Impossible d'ouvrir le document." & vbCrLf &
                   "Il est enregistré ici : " & vbCrLf & chemin,
                   MsgBoxStyle.Information, "Pièce jointe")
        End Try
        Return chemin
    End Function

    ''' <summary>
    ''' Supprime les fichiers du cache antérieurs au nombre d'heures indiqué.
    ''' Appelé à la fermeture de l'application : les documents médicaux ne doivent
    ''' pas s'accumuler indéfiniment sur le poste.
    ''' </summary>
    Public Sub PurgerCache(Optional ageMaxiHeures As Integer = 0)
        Try
            Dim limite = DateTime.Now.AddHours(-ageMaxiHeures)
            For Each fichier In Directory.GetFiles(DossierCache())
                Try
                    If File.GetLastWriteTime(fichier) <= limite Then File.Delete(fichier)
                Catch
                    ' Fichier encore ouvert : il sera purgé au prochain démarrage.
                End Try
            Next
        Catch
            ' Le cache est un confort : son nettoyage ne doit jamais bloquer.
        End Try
    End Sub

End Module
