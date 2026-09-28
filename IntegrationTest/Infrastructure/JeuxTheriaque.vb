Imports Oasis_Common

''' <summary>
''' Jeux de données du lot Theriaque : médicaments de la base de référence, ALD et
''' codes CIM-10, antécédents ALD, nomenclatures NOS de l'ANS.
'''
''' Les antécédents ALD passent par AntecedentDao.CreationAntecedent, l'INSERT de
''' la fenêtre RadFAntecedentDetailEdit. Les allergies et contre-indications se
''' créent dans les tests par leur DAO (et, pour une substance père, par
''' JeuxPatient.CreerAllergieSubstancePere et CreerContreIndicationSubstancePere).
''' Le SQL brut se limite à ce qu'aucun DAO n'écrit : les tables de référence
''' reprises de bases externes (oa_r_medicament, oa_ald_cim10, ans_nos_*), et les
''' états qu'aucun écran ne laisse (inactif NULL, date de fin d'ALD vide, type changé).
'''
''' Les lignes d'oa_ald viennent de Schema/29-reference-theriaque.sql : le singleton
''' Table_ald les garde en mémoire pour tout le processus.
'''
''' La base Theriaque elle-même (base Theriak, procédures theriaque.GET_THE_*) n'est
''' pas dans l'export du schéma d'oasis : aucun test ne la remplit.
''' </summary>
Public Module JeuxTheriaque

    ''' <summary>ALD de 29-reference-theriaque.sql : diabète.</summary>
    Public Const AldReferenceDiabeteId As Integer = 9501
    Public Const AldReferenceDiabeteCode As String = "91"
    Public Const AldReferenceDiabeteDescription As String = "ALD de test : diabète"

    ''' <summary>ALD de 29-reference-theriaque.sql : insuffisance cardiaque.</summary>
    Public Const AldReferenceCardiaqueId As Integer = 9502
    Public Const AldReferenceCardiaqueCode As String = "92"
    Public Const AldReferenceCardiaqueDescription As String = "ALD de test : insuffisance cardiaque"

    ''' <summary>
    ''' Vrai quand la base Theriak existe sur l'instance de test. Les requêtes de
    ''' TheriaqueDao y basculent (ChangeDatabase, noms en trois parties) ; sans elle,
    ''' elles échouent toutes.
    ''' </summary>
    Function BaseTheriakPresente() As Boolean
        Dim id = Scalaire("SELECT DB_ID('Theriak')")
        Return id IsNot Nothing AndAlso Not IsDBNull(id)
    End Function

    ''' <summary>
    ''' Vrai quand la colonne accepte NULL dans le schéma de test. Sert à rendre
    ''' Inconclusive un test de colonne NULL quand l'export la déclare NOT NULL.
    ''' </summary>
    Function ColonneNullableTheriaque(table As String, colonne As String) As Boolean
        Dim resultat = Scalaire("SELECT COLUMNPROPERTY(OBJECT_ID(@p0), @p1, 'AllowsNull')", table, colonne)
        Return resultat IsNot Nothing AndAlso Not IsDBNull(resultat) AndAlso CInt(resultat) = 1
    End Function

    ''' <summary>
    ''' Médicament de la base de référence (oa_r_medicament, reprise de la BDPM, que
    ''' l'application ne fait que lire). Nothing est écrit NULL.
    ''' </summary>
    Sub CreerMedicamentReference(cis As Integer, dci As String, forme As String, titulaire As String, voie As String)
        Dim insertion = "INSERT INTO oasis.oa_r_medicament (oa_medicament_cis, oa_medicament_dci, oa_medicament_forme," &
                        " oa_medicament_titulaire, oa_medicament_voie_administration) VALUES (@p0, @p1, @p2, @p3, @p4)"
        If EstIdentiteTheriaque("oasis.oa_r_medicament", "oa_medicament_cis") Then
            insertion = "SET IDENTITY_INSERT oasis.oa_r_medicament ON; " & insertion &
                        "; SET IDENTITY_INSERT oasis.oa_r_medicament OFF;"
        End If
        Executer(insertion, cis, dci, forme, titulaire, voie)
    End Sub

    ''' <summary>
    ''' Code CIM-10 rattaché à une ALD (oa_ald_cim10, liste officielle que
    ''' l'application ne fait que lire). Nothing est écrit NULL. Renvoie son id.
    ''' </summary>
    Function CreerAldCim10Reference(aldId As Integer?, aldCode As String, code As String, description As String) As Long
        Dim colonnes = "oa_ald_cim10_ald_id, oa_ald_cim10_ald_code, oa_ald_cim10_code, oa_ald_cim10_description"
        If EstIdentiteTheriaque("oasis.oa_ald_cim10", "oa_ald_cim10_id") Then
            Return CLng(Scalaire("INSERT INTO oasis.oa_ald_cim10 (" & colonnes & ") VALUES (@p0, @p1, @p2, @p3);" &
                                 " SELECT CAST(SCOPE_IDENTITY() AS BIGINT);",
                                 If(aldId.HasValue, CObj(aldId.Value), Nothing), aldCode, code, description))
        End If
        Dim id = CLng(Scalaire("SELECT ISNULL(MAX(oa_ald_cim10_id), 0) + 1 FROM oasis.oa_ald_cim10"))
        Executer("INSERT INTO oasis.oa_ald_cim10 (oa_ald_cim10_id, " & colonnes & ") VALUES (@p0, @p1, @p2, @p3, @p4)",
                 id, If(aldId.HasValue, CObj(aldId.Value), Nothing), aldCode, code, description)
        Return id
    End Function

    ''' <summary>
    ''' Antécédent (type A) portant une ALD, comme RadFAntecedentDetailEdit quand
    ''' l'utilisateur coche l'ALD : AntecedentDao.CreationAntecedent, niveau 1, publié
    ''' par défaut, ALD demandée hors cours. aldValide à False : CreationAntecedent
    ''' remplace alors les dates d'ALD par Date.MaxValue. Une DRC est créée pour lui.
    ''' Renvoie son id.
    ''' </summary>
    Function CreerAntecedentAld(patientId As Long, utilisateurId As Long, aldId As Integer, aldCim10Id As Long,
                                dateFinAld As Date,
                                Optional statutAffichage As String = "P",
                                Optional aldValide As Boolean = True) As Long
        Dim nouveau As New Antecedent With {
            .PatientId = CInt(patientId),
            .DrcId = CInt(CreerDrc()),
            .Description = "Antécédent ALD de test",
            .DateDebut = New Date(2020, 3, 15),
            .StatutAffichage = statutAffichage,
            .Diagnostic = 1,
            .AldId = aldId,
            .AldCim10Id = CInt(aldCim10Id),
            .AldValide = aldValide,
            .AldDateDebut = New Date(2020, 3, 15),
            .AldDateFin = dateFinAld,
            .AldDemandeEnCours = False,
            .ChaineEpisodeDateFin = Date.Today.AddMonths(6)
        }
        Dim daoAntecedent As New AntecedentDao
        Return daoAntecedent.CreationAntecedent(nouveau, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Function

    ''' <summary>Annule un antécédent ALD (oa_antecedent_inactif = 1), sans historique.</summary>
    Sub DesactiverAntecedentAld(antecedentId As Long)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_inactif = 1 WHERE oa_antecedent_id = @p0", antecedentId)
    End Sub

    ''' <summary>Change le type d'un antécédent ALD (C pour un contexte, par exemple).</summary>
    Sub ChangerTypeAntecedentAld(antecedentId As Long, typeAntecedent As String)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_type = @p0 WHERE oa_antecedent_id = @p1",
                 typeAntecedent, antecedentId)
    End Sub

    ''' <summary>Retire la date de fin d'ALD d'un antécédent (NULL) : aucun écran ne le fait.</summary>
    Sub EffacerDateFinAld(antecedentId As Long)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_ald_date_fin = NULL WHERE oa_antecedent_id = @p0", antecedentId)
    End Sub

    ''' <summary>
    ''' Met inactif à NULL sur une ligne d'allergie ou de contre-indication, état
    ''' qu'aucun DAO ne laisse (ils écrivent toujours 0 ou 1) mais que les requêtes
    ''' de lecture prévoient.
    ''' </summary>
    Sub EffacerInactifTheriaque(table As String, colonneId As String, id As Long)
        Executer("UPDATE " & table & " SET inactif = NULL WHERE " & colonneId & " = @p0", id)
    End Sub

    ''' <summary>Profession de santé de la nomenclature NOS G15. Nothing est écrit NULL.</summary>
    Sub CreerProfessionSanteNos(code As Integer, libelle As String, oid As String)
        Executer("INSERT INTO oasis.ans_nos_g15_profession_sante (oid, code, libelle) VALUES (@p0, @p1, @p2)",
                 oid, code, libelle)
    End Sub

    ''' <summary>Spécialité ordinale de la nomenclature NOS R38. Nothing est écrit NULL.</summary>
    Sub CreerSpecialiteOrdinaleNos(code As String, libelle As String, oid As String)
        Executer("INSERT INTO oasis.ans_nos_r38_specialite_ordinale (oid, code, libelle) VALUES (@p0, @p1, @p2)",
                 oid, code, libelle)
    End Sub

    ''' <summary>Compétence exclusive de la nomenclature NOS R40. Nothing est écrit NULL.</summary>
    Sub CreerCompetenceExclusiveNos(code As String, libelle As String, oid As String)
        Executer("INSERT INTO oasis.ans_nos_r40_competence_exclusive (oid, code, libelle) VALUES (@p0, @p1, @p2)",
                 oid, code, libelle)
    End Sub

    Private Function EstIdentiteTheriaque(table As String, colonne As String) As Boolean
        Dim identite = Scalaire("SELECT COLUMNPROPERTY(OBJECT_ID(@p0), @p1, 'IsIdentity')", table, colonne)
        Return identite IsNot Nothing AndAlso Not IsDBNull(identite) AndAlso CInt(identite) = 1
    End Function

End Module
