Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class TheriaqueDao
    Inherits StandardDao

    Public Enum EnumGetSpecialite
        VIRTUEL_NE_PAS_UTILISER
        ID_THERIAQUE
        CODE_UCD
        CODE_CIP
        LABO_TITULAIRE_AMM
        LABO_EXPLOITANT
        CODE_FICHE_INDICATION
        SUBSTANCE_ACTIVE
        EXCIPIENT
        CLASSE_EPHMRA
        CLASSE_ATC
        CLASSE_PHARMACO_THERAPEUTIQUE
        CLASSE_GESTION_AP_HP
        GENERIQUE_THERIAQUE
        GENERIQUE_AFSSAPS
        NOM_SPECIALITE
        CODE_UCD13
        CODE_CIP13
    End Enum

    Public Enum EnumMonoVir
        CLASSIQUE
        VIRTUEL
        NULL
    End Enum

    Public Enum EnumTypeEffetIndesirable
        CLINIQUE = 1
        PARA_CLINIQUE = 2
        CLINIQUE_SURDOSAGE = 3
        PARA_CLINIQUE_SURDOSAGE = 4
    End Enum

    Friend Function GetSpecialiteById(SpecialiteId As Integer) As SpecialiteTheriaque
        Dim specialite As New SpecialiteTheriaque
        Dim dt As DataTable

        Try
            dt = getSpecialiteByArgument(SpecialiteId, EnumGetSpecialite.ID_THERIAQUE, TheriaqueDao.EnumMonoVir.NULL)
            Dim rowCount As Integer = dt.Rows.Count
            If dt.Rows.Count > 0 Then
                specialite = BuildBean(dt)
            Else
                specialite.Id = 0
                specialite.CodeAtc = ""
                specialite.Dci = ""
                specialite.DciLongue = ""
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return specialite
    End Function

    Private Function BuildBean(dt As DataTable) As SpecialiteTheriaque
        Dim specialite As New SpecialiteTheriaque

        specialite.Id = dt.Rows(0)("SP_CODE_SQ_PK")
        specialite.CodeAtc = Coalesce(dt.Rows(0)("SP_CODE_SQ_PK"), "")
        specialite.Dci = Coalesce(dt.Rows(0)("SP_NOM").Replace("§", ""), "")
        specialite.DciLongue = Coalesce(dt.Rows(0)("SP_NOMLONG"), "")

        Return specialite
    End Function

    '=============================================================================================
    '   ATC
    '=============================================================================================
    Public Function GetAllATC() As DataTable
        Dim SQLString As String
        SQLString = "SELECT CATC_CODE_PK, CATC_NOMF FROM [Theriak].[theriaque].[CATC_CLASSEATC]" &
                    " WHERE (CATC_CATC_CODE_FK is Null)" &
                    " AND CATC_CODE_PK NOT IN ('X')" &
                    " ORDER BY CATC_CODE_PK"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return dt
    End Function

    Friend Function getATCListeByATCPere(CodeATC As String) As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_ATC_ATC", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeATC)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return dt
    End Function

    Friend Function GetATCDenominationById(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim ATCDenomination As String = ""


        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_ATC_ID", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count
                If dt.Rows.Count > 0 Then
                    ATCDenomination = dt.Rows(0)("catc_nomf")
                Else
                    ATCDenomination = ""
                End If
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return ATCDenomination
    End Function

    '=============================================================================================
    '   Spécialité
    '=============================================================================================
    Friend Function getSpecialiteByArgument(CodeId As String, VarTyp As EnumGetSpecialite, Monovir As Integer) As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SPECIALITE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@VarTyp", VarTyp)
                command.Parameters.AddWithValue("@MonoVir", If(Monovir = TheriaqueDao.EnumMonoVir.NULL, DBNull.Value, Monovir))

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return dt
    End Function

    Friend Function getSpecialiteDenominationById(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim Denomination As String = ""

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SPECIALITE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@VarTyp", EnumGetSpecialite.ID_THERIAQUE)
                'command.Parameters.AddWithValue("@MonoVir", EnumMonoVir.VIRTUEL)
                command.Parameters.AddWithValue("@MonoVir", DBNull.Value)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    Denomination = dt.Rows(0)("SP_NOM")
                End If
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return Denomination
    End Function

    Friend Function getCodeAtcBySpecialiteId(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim CodeATC As String = ""

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SPECIALITE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@VarTyp", EnumGetSpecialite.ID_THERIAQUE)
                'command.Parameters.AddWithValue("@MonoVir", EnumMonoVir.VIRTUEL)
                command.Parameters.AddWithValue("@MonoVir", DBNull.Value)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    CodeATC = dt.Rows(0)("SP_CATC_CODE_FK")
                End If
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return CodeATC
    End Function

    Friend Function GetPharmacoCinetiqueBySpecialite(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim Pharmacotext As String = ""
        Dim PremierPassage As Boolean = False

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_CINETIQUE_SPE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    If PremierPassage = False Then
                        PremierPassage = True
                        Pharmacotext = dt.Rows(i)("PHARMACOTEXT")
                    Else
                        Pharmacotext += vbCrLf & dt.Rows(i)("PHARMACOTEXT")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return Pharmacotext
    End Function

    Friend Function GetEffetIndesirableBySpecialite(CodeId As String, Type As EnumTypeEffetIndesirable) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim EffetIndesirable As String = ""
        Dim PremierPassage As Boolean = False

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_EFFIND_SPE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@CodeId", CodeId)
                command.Parameters.AddWithValue("@TypId", Type)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    If PremierPassage = False Then
                        PremierPassage = True
                        EffetIndesirable = dt.Rows(i)("TEXTEFFET")
                    Else
                        EffetIndesirable += vbCrLf & dt.Rows(i)("TEXTEFFET")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return EffetIndesirable
    End Function

    Friend Function GetPharmacoDynamiqueBySpecialite(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim PharmacoDynamique As String = ""
        Dim PremierPassage As Boolean = False


        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_DET_PHDYNA", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@typId", 1)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    If PremierPassage = False Then
                        PremierPassage = True
                        PharmacoDynamique = dt.Rows(i)("TEXTEPH")
                    Else
                        PharmacoDynamique += vbCrLf & dt.Rows(i)("TEXTEPH")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return PharmacoDynamique
    End Function

    '=============================================================================================
    '   Substance père
    '=============================================================================================
    Friend Function getSubstancePereDenominationById(substancePereId As Integer) As String
        Dim SubstancePereDenomination As String = ""
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM theriaque.GSAC_PERE_SUBACT WHERE GSAC_CODE_SQ_PK = @id"

            command.Connection.ChangeDatabase("Theriak")
            command.Parameters.AddWithValue("@id", substancePereId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    SubstancePereDenomination = Coalesce(reader("GSAC_NOM"), "")
                Else
                    Throw New ArgumentException("Substance père inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return SubstancePereDenomination
    End Function

    '=============================================================================================
    '   Substance
    '=============================================================================================
    Friend Function GetSubstanceById(CodeId As String) As Substance
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim substance As New Substance
        Dim SubstanceDenomination As String = ""


        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SUB_ID", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@VarType", 1) 'Substance active

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count
                If dt.Rows.Count > 0 Then
                    substance.SubstanceId = dt.Rows(0)("SAC_CODE_SQ_PK")
                    substance.SubstanceDenomination = Coalesce(dt.Rows(0)("SAC_NOM"), "")
                    substance.SubstancePereId = Coalesce(dt.Rows(0)("SAC_GSAC_CODE_FK"), 0)
                Else
                    substance.SubstanceId = 0
                    substance.SubstanceDenomination = ""
                    substance.SubstancePereId = 0
                End If
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return substance
    End Function

    Friend Function GetSubstanceCodeListBySpecialite(CodeId As String) As List(Of Integer)
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim SubstanceCodeList As New List(Of Integer)

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SUB_SPE", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@typId", 2) 'Substance active

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    Dim SubstanceCode As Integer = dt.Rows(i)("CODESUBST")
                    If SubstanceCodeList.Contains(SubstanceCode) = False Then
                        SubstanceCodeList.Add(SubstanceCode)
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return SubstanceCodeList
    End Function


    Friend Function GetSubstanceDenominationById(CodeId As String) As String
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim SubstanceDenomination As String = ""


        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_SUB_ID", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", CodeId)
                command.Parameters.AddWithValue("@VarType", 1) 'Substance active

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count
                If dt.Rows.Count > 0 Then
                    SubstanceDenomination = dt.Rows(0)("SAC_NOM")
                Else
                    SubstanceDenomination = ""
                End If
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return SubstanceDenomination
    End Function

    Friend Function GetATCCodeListBySubstanceId(SubstanceId As String) As List(Of String)
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim ATCCodeList As New List(Of String)

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("theriaque.GET_THE_DET_SUBACT", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.Parameters.AddWithValue("@codeId", SubstanceId)
                command.Parameters.AddWithValue("@typId", 4) 'ATC

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)

                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    Dim ATCCode As String = dt.Rows(i)("CODE")
                    If ATCCodeList.Contains(ATCCode) = False Then
                        ATCCodeList.Add(ATCCode)
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return ATCCodeList
    End Function

    Public Function getSubstanceActiveBySubstancePereId(SubstancePereId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM theriaque.SAC_SUBACTIVE" &
                    " WHERE SAC_GSAC_CODE_FK = @substancePereId"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                da.SelectCommand.Connection.ChangeDatabase("Theriak")
                da.SelectCommand.Parameters.AddWithValue("@substancePereId", SubstancePereId)

                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        da.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using

        Return dt
    End Function


    '=============================================================================================
    '=== Contrôle contre-indication
    '=============================================================================================
    Friend Function IsSpecialiteContreIndique(patient As Patient, specialiteId As Long) As SpecialiteContreIndique
        Dim specialiteContreIndique As New SpecialiteContreIndique
        specialiteContreIndique.ContreIndication = False
        specialiteContreIndique.MessageContreIndication = ""

        Dim specialite As SpecialiteTheriaque
        specialite = GetSpecialiteById(specialiteId)

        'Contrôle contre-indication (ATC)
        Dim contreIndicationATCDao As New ContreIndicationATCDao
        Dim dt As DataTable

        Dim codeAtcSpecialite As String = getCodeAtcBySpecialiteId(specialiteId)

        dt = contreIndicationATCDao.getAllContreIndicationATCbyPatient(patient.patientId)
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Dim codeAtcContreIndication As String = dt.Rows(i)("code_atc")
            If codeAtcSpecialite.StartsWith(codeAtcContreIndication) = True Then
                specialiteContreIndique.ContreIndication = True
                Dim denominationAtcSpecialite As String = GetATCDenominationById(codeAtcSpecialite)
                If codeAtcSpecialite = codeAtcContreIndication Then
                    specialiteContreIndique.MessageContreIndication = "Attention, le médicament (" & specialite.Dci & ") appartient à la classe thérapeutique (" &
                          codeAtcSpecialite & " - " & denominationAtcSpecialite &
                          ") qui est contre-indiquée pour ce patient !"
                Else
                    Dim denominationAtcContreIndication As String = GetATCDenominationById(codeAtcContreIndication)
                    specialiteContreIndique.MessageContreIndication = "Attention, le médicament (" & specialite.Dci & ") appartient à la classe thérapeutique (" &
                          codeAtcSpecialite & " - " & denominationAtcSpecialite &
                          "), comprise dans la classe thérapeutique (" &
                          codeAtcContreIndication & " - " & denominationAtcContreIndication &
                          ") qui est contre-indiquée pour ce patient !"
                End If
            End If
        Next

        'Contrôle contre-indication (Substance)
        '--> Liste des substances du médicament
        Dim SubstanceListe As List(Of Integer)
        SubstanceListe = GetSubstanceCodeListBySpecialite(specialiteId)

        '--> Liste des substances en contre-indication pour le patient
        Dim contreIndicationSubstanceDao As New ContreIndicationSubstanceDao
        dt = contreIndicationSubstanceDao.getAllContreIndicationSubstancebyPatient(patient.patientId)
        rowCount = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Dim codeSubstanceCI As Long = dt.Rows(i)("substance_id")
            If codeSubstanceCI <> 0 Then
                Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
                While EnumeratorSubstanceListe.MoveNext()
                    Dim CodeSubstanceSpecialite As Integer = EnumeratorSubstanceListe.Current
                    If codeSubstanceCI = CodeSubstanceSpecialite Then
                        Dim SubstanceDenomination As String = GetSubstanceDenominationById(CodeSubstanceSpecialite)
                        If specialiteContreIndique.ContreIndication = True Then
                            specialiteContreIndique.MessageContreIndication += vbCrLf
                        Else
                            specialiteContreIndique.ContreIndication = True
                        End If
                        specialiteContreIndique.MessageContreIndication += "Attention, le médicament (" & specialite.Dci & ") comporte une substance (" &
                                          SubstanceDenomination &
                                          "), contre-indiquée pour ce patient !"
                    End If
                End While
            Else
                Dim codeSubstancePereCI As Long = dt.Rows(i)("substance_pere_id")
                If codeSubstancePereCI <> 0 Then
                    Dim dt2 As DataTable
                    dt2 = getSubstanceActiveBySubstancePereId(codeSubstancePereCI)
                    rowCount = dt2.Rows.Count - 1
                    For j = 0 To rowCount Step 1
                        Dim codeSubstanceId As Long = dt2.Rows(j)("SAC_CODE_SQ_PK")
                        Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
                        While EnumeratorSubstanceListe.MoveNext()
                            Dim CodeSubstanceSpecialite As Integer = EnumeratorSubstanceListe.Current
                            If codeSubstanceId = CodeSubstanceSpecialite Then
                                Dim SubstanceDenomination As String = GetSubstanceDenominationById(CodeSubstanceSpecialite)
                                If specialiteContreIndique.ContreIndication = True Then
                                    specialiteContreIndique.MessageContreIndication += vbCrLf
                                Else
                                    specialiteContreIndique.ContreIndication = True
                                End If
                                specialiteContreIndique.MessageContreIndication += "Attention, le médicament (" & specialite.Dci & ") comporte une substance (" &
                                                  SubstanceDenomination &
                                                  "), contre-indiquée pour ce patient !"
                            End If
                        End While
                    Next
                End If
            End If

        Next

        Return specialiteContreIndique
    End Function


    '=============================================================================================
    '=== Contrôle allergie
    '=============================================================================================
    Friend Function IsSpecialiteAllergique(patient As Patient, specialiteId As Long) As SpecialiteAllergique
        Dim specialiteAllergique As New SpecialiteAllergique
        specialiteAllergique.Allergie = False
        specialiteAllergique.MessageAllergie = ""

        Dim specialite As SpecialiteTheriaque
        specialite = GetSpecialiteById(specialiteId)

        Dim dt As DataTable

        Dim codeAtcSpecialite As String = getCodeAtcBySpecialiteId(specialiteId)

        '--> Liste des substances du médicament
        Dim SubstanceListe As List(Of Integer)
        SubstanceListe = GetSubstanceCodeListBySpecialite(specialiteId)


        '--> Liste des substances déclarées en allergie pour le patient
        Dim allergieDao As New AllergieDao
        dt = allergieDao.getAllAllergiebyPatient(patient.patientId)
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Dim codeSubstanceAllergie As Long = dt.Rows(i)("substance_id")
            If codeSubstanceAllergie <> 0 Then
                Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
                While EnumeratorSubstanceListe.MoveNext()
                    Dim CodeSubstanceSpecialite As Integer = EnumeratorSubstanceListe.Current
                    If codeSubstanceAllergie = CodeSubstanceSpecialite Then
                        Dim SubstanceDenomination As String = GetSubstanceDenominationById(CodeSubstanceSpecialite)
                        specialiteAllergique.Allergie = True
                        specialiteAllergique.MessageAllergie += "Attention, le médicament (" & specialite.Dci & ") comporte une substance (" &
                                          SubstanceDenomination &
                                          "), déclarée allergique pour ce patient !"
                    End If
                End While
            Else
                Dim codeSubstancePereAllergie As Long = dt.Rows(i)("substance_pere_id")
                If codeSubstancePereAllergie <> 0 Then
                    Dim dt2 As DataTable
                    dt2 = getSubstanceActiveBySubstancePereId(codeSubstancePereAllergie)
                    rowCount = dt2.Rows.Count - 1
                    For j = 0 To rowCount Step 1
                        Dim codeSubstanceId As Long = dt2.Rows(j)("SAC_CODE_SQ_PK")
                        Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
                        While EnumeratorSubstanceListe.MoveNext()
                            Dim CodeSubstanceSpecialite As Integer = EnumeratorSubstanceListe.Current
                            If codeSubstanceId = CodeSubstanceSpecialite Then
                                Dim SubstanceDenomination As String = GetSubstanceDenominationById(CodeSubstanceSpecialite)
                                specialiteAllergique.Allergie = True
                                specialiteAllergique.MessageAllergie += "Attention, le médicament (" & specialite.Dci & ") comporte une substance (" &
                                          SubstanceDenomination &
                                          "), déclarée allergique pour ce patient !"
                            End If
                        End While
                    Next
                End If
            End If
        Next

        Return specialiteAllergique
    End Function

End Class
