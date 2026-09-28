Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' Jeux de données du lot portail et paramétrage : comptes internautes (verrou,
''' clé de récupération), extensions de fichiers, paramètres de courriel par siège,
''' paramètre technique oa_parametre_oasis.
'''
''' Les comptes internautes eux-mêmes passent par JeuxUtilisateur.CreerInternaute
''' (InternauteDao.Create). Ce module ne fait que poser, sous le compte Admin, les
''' états qu'aucun DAO n'écrit directement (compteur d'échecs, date d'expiration
''' choisie, ancienne empreinte) et remplir les tables qu'aucun DAO n'alimente.
''' Aucune de ces tables n'est mise en cache par un singleton d'EnvironnementBase :
''' un test peut les remplir.
''' </summary>
Public Module JeuxInternaute

    ''' <summary>Valeur d'une colonne d'oa_internaute, lue sous le compte Admin.</summary>
    Function ValeurInternaute(colonne As String, internauteId As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_internaute WHERE id = @p0", internauteId)
    End Function

    ''' <summary>Pose le compteur d'échecs et la fin de verrou d'un compte internaute.</summary>
    Sub PoserVerrouInternaute(internauteId As Long, tentatives As Integer, verrouJusqua As Date?)
        Executer("UPDATE oasis.oa_internaute SET tentatives = @p0, verrou_jusqua = @p1 WHERE id = @p2",
                 tentatives, If(verrouJusqua.HasValue, CObj(verrouJusqua.Value), Nothing), internauteId)
    End Sub

    ''' <summary>Pose la clé de récupération et sa date d'expiration (Nothing donne NULL).</summary>
    Sub PoserRecuperationInternaute(internauteId As Long, recovery As String, expiration As Date?)
        Executer("UPDATE oasis.oa_internaute SET recovery = @p0, recovery_expiration = @p1 WHERE id = @p2",
                 recovery, If(expiration.HasValue, CObj(expiration.Value), Nothing), internauteId)
    End Sub

    ''' <summary>Remplace l'empreinte du mot de passe (Nothing donne NULL).</summary>
    Sub PoserEmpreinteInternaute(internauteId As Long, empreinte As String)
        Executer("UPDATE oasis.oa_internaute SET password = @p0 WHERE id = @p1", empreinte, internauteId)
    End Sub

    ''' <summary>
    ''' Extension de fichier du portail (oa_r_file_extension). Aucun DAO n'écrit
    ''' cette table ; colonnes reprises de FileExtensionDao.BuildBean. Nothing donne
    ''' NULL. Renvoie l'id.
    ''' </summary>
    Function CreerExtensionFichier(extension As String, description As String) As Long
        Return InsererLigneInternaute("oasis.oa_r_file_extension", "id", "ext, description", extension, description)
    End Function

    ''' <summary>
    ''' Siège minimal, actif, auquel rattacher un paramètre de courriel au cas où
    ''' oa_r_mail_parameter.siege_id porterait une clé étrangère. Colonnes reprises
    ''' de SiegeDao.BuildBean. Renvoie l'id.
    ''' </summary>
    Function CreerSiegeCourriel(description As String) As Long
        Return InsererLigneInternaute("oasis.oa_siege", "oa_siege_id",
            "oa_siege_description, oa_siege_adresse1, oa_siege_adresse2, oa_siege_ville, oa_siege_code_postal," &
            " oa_siege_telephone, oa_siege_mail, oa_siege_fax, oa_siege_statut",
            description, "1 rue du Siege", "", "Mamoudzou", "97600", "0269000000", "siege@exemple.fr", "", "A")
    End Function

    ''' <summary>
    ''' Paramètre de courriel complet. JeuxUtilisateur.CreerParametreMail ne pose ni
    ''' siège, ni objet, ni corps choisis : celui-ci les prend tous. siegeId à 0,
    ''' objet ou corps à Nothing donnent NULL. Renvoie l'id.
    ''' </summary>
    Function CreerParametreMailSiege(siegeId As Long, typeParam As String,
                                     objet As String, corps As String,
                                     Optional html As Boolean = False,
                                     Optional smtpParams As String = "SMTPServer=smtp.exemple.fr") As Long
        Return InsererLigneInternaute("oasis.oa_r_mail_parameter", "id",
            "siege_id, type_mail_param, objet, body, is_body_html, smtp_params",
            If(siegeId = 0, Nothing, CObj(siegeId)), typeParam, objet, corps, html, smtpParams)
    End Function

    ''' <summary>
    ''' Retire les paramètres de courriel d'un type. Un script de référence d'un
    ''' autre domaine peut en avoir chargé : TOP 1 choisirait alors entre deux
    ''' lignes sans siège, dans un ordre que la requête ne fixe pas.
    ''' </summary>
    Sub ViderParametresMail(typeParam As String)
        Executer("DELETE FROM oasis.oa_r_mail_parameter WHERE type_mail_param = @p0", typeParam)
    End Sub

    ''' <summary>
    ''' Remplace la ligne oa_parametre_oasis d'id donné. valeur à Nothing donne une
    ''' date NULL. Le DAO écrit la date en texte ; ici elle part typée.
    ''' </summary>
    Sub PoserParametreOasis(id As Integer, description As String, valeur As Date?)
        Executer("DELETE FROM oasis.oa_parametre_oasis WHERE oa_parametre_oasis_id = @p0;" &
                 " INSERT INTO oasis.oa_parametre_oasis (oa_parametre_oasis_id, oa_parametre_oasis_description, oa_parametre_oasis_date)" &
                 " VALUES (@p0, @p1, @p2)",
                 id, description, If(valeur.HasValue, CObj(valeur.Value), Nothing))
    End Sub

    ''' <summary>Supprime la ligne oa_parametre_oasis d'id donné, si elle existe.</summary>
    Sub RetirerParametreOasis(id As Integer)
        Executer("DELETE FROM oasis.oa_parametre_oasis WHERE oa_parametre_oasis_id = @p0", id)
    End Sub

    ''' <summary>Date enregistrée dans oa_parametre_oasis, sous le compte Admin (Nothing, DBNull ou la date).</summary>
    Function LireParametreOasis(id As Integer) As Object
        Return Scalaire("SELECT oa_parametre_oasis_date FROM oasis.oa_parametre_oasis WHERE oa_parametre_oasis_id = @p0", id)
    End Function

    ''' <summary>
    ''' Valeur que SQL Server stocke quand on écrit ce texte dans cette colonne, sous
    ''' ce compte. Plusieurs DAO envoient les dates en texte (Date.ToString() dans la
    ''' culture du poste, ou "yyyy-MM-dd HH:mm:ss") : leur lecture dépend alors de la
    ''' langue de la session (DATEFORMAT) et du type de la colonne. La copie vide de
    ''' la colonne dans une table temporaire reproduit exactement cette conversion.
    ''' Lève la SqlException de conversion quand le texte n'est pas lisible.
    ''' </summary>
    Function ConversionSqlDeTexte(quelCompte As Compte, table As String, colonne As String, texte As String) As Object
        Return ScalaireSous(quelCompte,
            "SELECT TOP 0 " & colonne & " AS valeur INTO #conversion FROM " & table & ";" &
            " INSERT INTO #conversion (valeur) VALUES (@p0);" &
            " SELECT valeur FROM #conversion;", texte)
    End Function

    ''' <summary>
    ''' Valeurs que la colonne peut avoir reçues d'un DAO qui formate Date.Now en
    ''' texte entre avant et apres : une par seconde de l'intervalle, celles que la
    ''' session ne sait pas lire en moins. Liste vide : l'écriture a dû échouer.
    ''' </summary>
    Function ConversionsSqlPossibles(quelCompte As Compte, table As String, colonne As String,
                                     avant As Date, apres As Date,
                                     formater As Func(Of Date, String)) As List(Of Object)
        Dim valeurs As New List(Of Object)
        Dim instant = New Date(avant.Year, avant.Month, avant.Day, avant.Hour, avant.Minute, avant.Second)
        While instant <= apres
            Try
                valeurs.Add(ConversionSqlDeTexte(quelCompte, table, colonne, formater(instant)))
            Catch ex As SqlException
                ' Texte illisible pour cette session : aucune valeur possible.
            End Try
            instant = instant.AddSeconds(1)
        End While
        Return valeurs
    End Function

    ''' <summary>
    ''' INSERT qui fonctionne que la clé soit une identité ou non. Les valeurs
    ''' deviennent @p0, @p1... dans l'ordre des colonnes. Renvoie l'id créé.
    ''' </summary>
    Private Function InsererLigneInternaute(table As String, colonneId As String, colonnes As String,
                                            ParamArray valeurs() As Object) As Long
        Dim marques = String.Join(", ", Enumerable.Range(0, valeurs.Length).Select(Function(i) "@p" & i))
        Dim sql =
            "DECLARE @id BIGINT;" & vbCrLf &
            "IF COLUMNPROPERTY(OBJECT_ID('" & table & "'), '" & colonneId & "', 'IsIdentity') = 1" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonnes & ") VALUES (" & marques & ");" & vbCrLf &
            "    SET @id = SCOPE_IDENTITY();" & vbCrLf &
            "END" & vbCrLf &
            "ELSE" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    SELECT @id = COALESCE(MAX(" & colonneId & "), 0) + 1 FROM " & table & ";" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonneId & ", " & colonnes & ") VALUES (@id, " & marques & ");" & vbCrLf &
            "END" & vbCrLf &
            "SELECT @id;"
        Return CLng(Scalaire(sql, valeurs))
    End Function

End Module
