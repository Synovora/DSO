Imports System.Configuration

''' <summary>
''' Contrôle des destinataires de courriel sortant.
'''
''' /api/sendMail acceptait n'importe quelle adresse, avec n'importe quel objet,
''' n'importe quel corps et une pièce jointe, expédiés par le compte SMTP de la
''' structure. Tout compte authentifié disposait donc d'un canal d'exfiltration
''' des documents du dossier, et d'une plateforme d'hameçonnage signée du domaine
''' de la structure. Rien n'en était tracé.
'''
''' Une adresse n'est acceptée que si elle est connue de la base (patient,
''' correspondant du dossier) ou si son domaine figure dans la configuration.
''' MailDomainesAutorises est une liste séparée par des points-virgules ; vide,
''' seules les adresses connues de la base passent.
''' </summary>
Public Module DestinatairesMail

    ''' <summary>
    ''' Sépare et normalise la liste de destinataires reçue du client.
    ''' </summary>
    Public Function Separer(adresses As String) As List(Of String)
        Dim liste As New List(Of String)
        For Each brute In If(adresses, "").Split(","c)
            Dim adresse = brute.Trim()
            If adresse <> "" Then liste.Add(adresse)
        Next
        Return liste
    End Function

    ''' <summary>
    ''' Vrai si l'adresse peut recevoir un envoi. patientId vaut 0 quand le message
    ''' n'est rattaché à aucun dossier.
    ''' </summary>
    Public Function EstAutorise(adresse As String, patientId As Long) As Boolean
        If String.IsNullOrWhiteSpace(adresse) OrElse Not IsValidEmail(adresse) Then Return False

        ' Une adresse ne doit jamais porter de saut de ligne : MimeKit refuserait,
        ' mais la vérification est faite ici pour que le refus soit explicite.
        If adresse.IndexOfAny(New Char() {ChrW(13), ChrW(10), ChrW(0)}) >= 0 Then Return False

        If DomaineAutorise(adresse) Then Return True
        If AdresseConnueEnBase(adresse, patientId) Then Return True

        Return False
    End Function

    ''' <summary>Domaines de confiance déclarés en configuration.</summary>
    Private Function DomaineAutorise(adresse As String) As Boolean
        Dim configures = ConfigurationManager.AppSettings("MailDomainesAutorises")
        If String.IsNullOrWhiteSpace(configures) Then Return False

        Dim arobase = adresse.LastIndexOf("@"c)
        If arobase < 0 Then Return False
        Dim domaine = adresse.Substring(arobase + 1).Trim()

        For Each autorise In configures.Split(";"c)
            Dim attendu = autorise.Trim().TrimStart("@"c)
            If attendu <> "" AndAlso String.Equals(domaine, attendu, StringComparison.OrdinalIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Adresse déjà enregistrée : celle du patient concerné, ou celle d'un
    ''' correspondant de l'annuaire professionnel.
    ''' </summary>
    Private Function AdresseConnueEnBase(adresse As String, patientId As Long) As Boolean
        Try
            If patientId > 0 Then
                Dim patientDao As New PatientDao
                Dim patient = patientDao.GetPatient(patientId)
                If patient IsNot Nothing AndAlso
                   String.Equals(If(patient.PatientEmail, "").Trim(), adresse, StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            End If

            Dim annuaireDao As New AnnuaireProfessionnelBalDao
            Return annuaireDao.ExisteAdresse(adresse)

        Catch ex As Exception
            ' Base indisponible : on refuse. Un contrôle qui ne peut pas s'exécuter
            ' ne doit pas se conclure par une autorisation.
            Return False
        End Try
    End Function

End Module
