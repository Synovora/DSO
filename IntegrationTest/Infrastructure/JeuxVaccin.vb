Imports Oasis_Common

''' <summary>
''' Jeux de données du domaine vaccin : vaccins importés, valences, calendrier
''' vaccinal (dates et valences par patient, patient 0 pour le calendrier général),
''' programmation et administration des vaccins.
'''
''' Tout passe par les méthodes de création des DAO du dossier Vaccin, sous le
''' compte courant, comme le client lourd : chaque table du domaine a son INSERT.
''' Aucun SQL brut d'écriture ici ; seul le comptage de lignes lit la base sous le
''' compte Admin.
''' </summary>
Public Module JeuxVaccin

    ''' <summary>Base des codes Theriaque de test ; un code distinct par vaccin créé dans le processus.</summary>
    Public Const CodeVaccinDeBase As Long = 70000000L

    Private compteurCodeVaccin As Integer = 0

    ''' <summary>Code Theriaque (SP_CODE_SQ_PK) jamais encore utilisé dans ce processus.</summary>
    Function NouveauCodeVaccin() As Long
        compteurCodeVaccin += 1
        Return CodeVaccinDeBase + compteurCodeVaccin
    End Function

    ''' <summary>
    ''' Importe un vaccin comme RadFVaccin.ImportVaccin (VaccinDao.Create). Code à 0 :
    ''' un nouveau code est tiré. Sans utilisateur, un compte est créé pour
    ''' renseigner utilisateur_import. Renvoie l'id.
    ''' </summary>
    Function CreerVaccin(Optional code As Long = 0,
                         Optional dci As String = Nothing,
                         Optional utilisateurId As Long = 0) As Long
        Dim codeRetenu = If(code = 0, NouveauCodeVaccin(), code)
        Dim auteur = If(utilisateurId = 0, CreerUtilisateur(avecCle:=False), utilisateurId)
        Dim daoVaccin As New VaccinDao
        Return daoVaccin.Create(New Vaccin With {
            .Code = codeRetenu,
            .CodeAtc = "J07BX99",
            .Dci = If(dci, "VACCIN TEST " & codeRetenu),
            .DciLongue = "VACCIN TEST " & codeRetenu & ", suspension injectable",
            .UtilisateurImport = auteur
        })
    End Function

    ''' <summary>
    ''' Crée une valence comme RadFValenceCreation (ValenceDao.Create) : active,
    ''' invisible, en dernière position. Sans utilisateur, un compte est créé.
    ''' Renvoie l'id.
    ''' </summary>
    Function CreerValence(Optional code As String = Nothing,
                          Optional utilisateurId As Long = 0) As Long
        Dim auteur = If(utilisateurId = 0, CreerUtilisateur(avecCle:=False), utilisateurId)
        Dim codeRetenu = If(code, "VAL" & Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant())
        Dim daoValence As New ValenceDao
        Return daoValence.Create(New Valence With {
            .Code = codeRetenu,
            .Description = "Description " & codeRetenu,
            .Precaution = "Precaution " & codeRetenu,
            .UtilisateurCreation = auteur,
            .UtilisateurModification = auteur
        })
    End Function

    ''' <summary>
    ''' Rattache une valence à un vaccin (ValenceDao.CreateRelation). La colonne
    ''' vaccin reçoit le code Theriaque du vaccin, pas son id : c'est ce que fait
    ''' RadFVaccin et ce sur quoi joint GetListVaccinValence. Renvoie l'id.
    ''' </summary>
    Function LierVaccinValence(codeVaccin As Long, valenceId As Long) As Long
        Dim daoValence As New ValenceDao
        Return daoValence.CreateRelation(New RelationVaccinValence With {.Vaccin = codeVaccin, .Valence = valenceId})
    End Function

    ''' <summary>
    ''' Inscrit une valence au calendrier d'un patient (CGVValenceDao.Create) ;
    ''' patient 0 pour le calendrier général. Renvoie l'id.
    ''' </summary>
    Function CreerValenceCgv(valenceId As Long, patientId As Long) As Long
        Dim daoCgvValence As New CGVValenceDao
        Return daoCgvValence.Create(New CGVValence With {.Valence = valenceId, .Patient = patientId})
    End Function

    ''' <summary>Date du calendrier, en jours depuis la naissance (CGVDateDao.Create). Renvoie l'id.</summary>
    Function CreerDateCgv(jours As Long, patientId As Long) As Long
        Dim daoCgvDate As New CGVDateDao
        Return daoCgvDate.Create(New CGVDate With {.Days = jours, .Patient = patientId})
    End Function

    ''' <summary>Coche une valence à une date du calendrier (CGVDateDao.CreateRelation). Renvoie l'id.</summary>
    Function LierValenceDateCgv(dateId As Long, valenceId As Long, patientId As Long,
                                Optional statut As Short = 0) As Long
        Dim daoCgvDate As New CGVDateDao
        Return daoCgvDate.CreateRelation(New RelationValenceDate With {
            .Date = dateId, .Valence = valenceId, .Patient = patientId, .Status = statut})
    End Function

    ''' <summary>
    ''' Programme un vaccin à une date du calendrier d'un patient, comme RadFVaccinInfo
    ''' (VaccinDao.CreateVaccinProgramRelation) : vaccin reçoit l'id du vaccin,
    ''' relation_vaccin_valence son code. Renvoie l'id.
    ''' </summary>
    Function ProgrammerVaccin(dateId As Long, patientId As Long, vaccinId As Long, codeVaccin As Long) As Long
        Dim daoVaccin As New VaccinDao
        Return daoVaccin.CreateVaccinProgramRelation(New VaccinProgramRelation With {
            .Date = dateId, .Patient = patientId, .Vaccin = vaccinId, .RelationVaccinValence = codeVaccin})
    End Function

    ''' <summary>
    ''' Lot et péremption d'un vaccin programmé, comme RadFVaccinInput
    ''' (VaccinDao.CreateVaccinProgramAdministration). Renvoie l'id.
    ''' </summary>
    Function CreerAdministrationVaccin(programmeId As Long,
                                       Optional lot As String = "LOT-TEST",
                                       Optional commentaire As String = "") As Long
        Dim daoVaccin As New VaccinDao
        Return daoVaccin.CreateVaccinProgramAdministration(New VaccinProgramAdmin With {
            .VaccinProgramRelation = programmeId,
            .Lot = lot,
            .Expiration = New Date(2027, 6, 1),
            .Comment = commentaire
        })
    End Function

    ''' <summary>
    ''' Nombre de lignes d'une table du schéma oasis qui vérifient la condition,
    ''' sous le compte Admin. Les valeurs deviennent @p0, @p1... comme pour Scalaire.
    ''' </summary>
    Function CompterLignesVaccin(table As String, condition As String, ParamArray valeurs() As Object) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis." & table & " WHERE " & condition, valeurs))
    End Function

End Module
