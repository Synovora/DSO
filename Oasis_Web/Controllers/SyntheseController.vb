Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Net.Http.Formatting
Imports System.Web
Imports System.Web.Http
Imports System.Web.Mvc
Imports Oasis_Web
Imports Oasis_Common

Namespace Oasis_Web.Controllers
    Public Class SyntheseController
        Inherits Controller

        ReadOnly parametreDao As New ParametreDao
        ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
        ReadOnly episodeDao As New EpisodeDao
        ReadOnly patientDao As New PatientDao
        Dim episodeParametreDao As New EpisodeParametreDao

        <System.Web.Mvc.Authorize>
        Public Function Index() As ActionResult

            Dim strName As String = Constants.LAYOUT_VERTICAL
            Dim strWelcomeText As String = "Dashboard"
            If TempData("ModeName") IsNot Nothing Then strName = TempData("ModeName").ToString()
            If TempData("WelcomeText") IsNot Nothing Then strWelcomeText = TempData("WelcomeText").ToString()
            ViewBag.ModeName = strName
            ViewBag.WelcomeText = strWelcomeText

            If Request.Cookies("patientId") Is Nothing Then
                Return View("~/Views/Pages/pages-500.cshtml")
            End If

            Dim patient = patientDao.GetPatient(Request.Cookies("patientId").Value)
            ViewBag.Patient = patient
            ViewBag.Contexts = BuildContexte(patient.PatientId)
            ViewBag.Antecedents = BuildAntecedent(patient.PatientId)
            Return View()
        End Function

        Private Function BuildAntecedent(patientId As Integer) As List(Of List(Of String))

            Dim result As List(Of List(Of String)) = New List(Of List(Of String))

            Dim antecedentDao As AntecedentDao = New AntecedentDao
            Dim antecedentDataTable As DataTable = antecedentDao.GetAllAntecedentbyPatient(patientId, True, True)


            Dim i As Integer
            Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
            Dim iGrid As Integer = -1
            Dim indentation As String
            Dim dateDateModification, AldDateFin As Date
            Dim AfficheDateModification As String
            Dim diagnostic As String
            Dim antecedentCache, AldValide, AldValideOK, AldDemandeEnCours As Boolean
            Dim antecedentIdPrecedent1, antecedentIdPrecedent2 As Long
            antecedentIdPrecedent1 = 0
            antecedentIdPrecedent2 = 0

            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                'If RadChkMajeurSeul.Checked = True Then
                '    If antecedentDataTable.Rows(i)("oa_antecedent_niveau") <> 1 Then
                '        Continue For
                '    End If
                'End If


                Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                    Case 1
                        indentation = ""
                    Case 2
                        indentation = "----> "
                    Case 3
                        indentation = "-------->> "
                    Case Else
                        indentation = ""
                End Select


                'Recherche si le contexte a été modifié
                AfficheDateModification = ""
                If antecedentDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                Else
                    If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                    End If
                End If

                'Identification si l'antécédent est caché
                antecedentCache = False
                If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                    If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                        antecedentCache = True
                    End If
                End If

                AldValide = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_valide"), False)
                AldDateFin = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Nothing)
                AldValideOK = False
                If AldValide = True Then
                    If AldDateFin > Date.Now() Then
                        AldValideOK = True
                    End If
                End If
                AldDemandeEnCours = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)


                'Alimentation du DataGridView
                diagnostic = ""
                If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                    If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                        diagnostic = "Suspicion de : "
                    Else
                        If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                            diagnostic = "Notion de : "
                        End If
                    End If
                End If

                Dim antecedentDescription As String

                If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                    antecedentDescription = ""
                Else
                    antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                    antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                    tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_description"))
                    ' RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentDescription").Value = antecedentDataTable.Rows(i)("oa_antecedent_description")
                End If

                Dim DescriptionDrcAld As String = ""
                If AldValideOK Or AldDemandeEnCours Then
                    'DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
                End If
                tmp.Add(indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription)
                'RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription
                '==========

                If AldValideOK = True Or AldDemandeEnCours = True Then
                    tmp.Add("X")
                Else
                    tmp.Add("")
                End If

                'Id antécédent
                tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_id"))
                tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_drc_id"))
                tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage3"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1"), 0))
                tmp.Add(Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2"), 0))

                'Déplacement horizontal, détermination de l'antécédent précédent
                Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                    Case 1
                        tmp.Add(antecedentIdPrecedent1)
                        antecedentIdPrecedent1 = antecedentDataTable.Rows(i)("oa_antecedent_id")
                        antecedentIdPrecedent2 = 0
                    Case 2
                        tmp.Add(antecedentIdPrecedent2)
                        antecedentIdPrecedent2 = antecedentDataTable.Rows(i)("oa_antecedent_id")
                    Case 3
                        'Non concerné
                End Select

                'Récupération de l'index du dernier antécédent déplacé pour lui remettre le focus lors du réaffichage de la grid
                'If antecedentIdADeplacer <> 0 AndAlso antecedentIdADeplacer = antecedentDataTable.Rows(i)("oa_antecedent_id") Then
                '    IndexAntecedentADeplacer = iGrid
                '    antecedentIdADeplacer = 0
                'End If

                'Déplacement vertical, détermination de l'antécédent pere si niveau 2 et 3
                Select Case CInt(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                    Case 2
                        tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1"))
                    Case 3
                        tmp.Add(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2"))
                    Case Else
                        tmp.Add(0)
                End Select
                result.Add(tmp)
            Next
            Return result
        End Function

        Private Function BuildContexte(patientId As Integer) As List(Of List(Of String))
            Dim antecedentDao As AntecedentDao = New AntecedentDao
            Dim contexteDataTable As DataTable = antecedentDao.GetContextebyPatient(patientId, True)

            Dim i As Integer
            Dim dateFin, dateModification As Date
            Dim AfficheDateModification, diagnostic As String
            Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
            Dim categorieContexte, categorieContexteString As String
            Dim contexteCache As Boolean
            Dim result As List(Of List(Of String)) = New List(Of List(Of String))


            For i = 0 To rowCount Step 1
                Dim tmp As List(Of String) = New List(Of String)
                categorieContexte = ""
                If contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                    categorieContexte = contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
                End If
                Select Case categorieContexte
                    Case ContexteCourrier.EnumParcoursBaseCode.Medical
                        categorieContexteString = ContexteCourrier.EnumParcoursBaseItem.Medical
                    Case ContexteCourrier.EnumParcoursBaseCode.BioEnvironnemental
                        categorieContexteString = ContexteCourrier.EnumParcoursBaseItem.BioEnvironnemental
                    Case Else
                        categorieContexteString = ""
                End Select

                'DateFin
                If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                    dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
                Else
                    dateFin = "31/12/9999"
                End If

                'Recherche si le contexte a été modifié (médical uniquement)
                AfficheDateModification = ""
                If categorieContexte = "M" Then
                    If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    Else
                        If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                            dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                            AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                        End If
                    End If
                End If

                'Contexte caché
                contexteCache = False
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                    If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                        contexteCache = True
                    End If
                End If

                'Alimentation du DataGridView
                tmp.Add(categorieContexteString)
                'RadContexteDataGridView.Rows(iGrid).Cells("categorieContexte").Value = categorieContexteString

                diagnostic = ""
                If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                        diagnostic = "Suspicion de "
                    Else
                        If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                            diagnostic = "Notion de "
                        End If
                    End If
                End If

                Dim contexteDescription As String
                contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
                If contexteDescription <> "" Then
                    contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                End If

                tmp.Add(AfficheDateModification & diagnostic & " " & contexteDescription)
                tmp.Add(contexteDataTable.Rows(i)("oa_antecedent_id"))
                result.Add(tmp)
            Next
            Return result
        End Function

    End Class
End Namespace
