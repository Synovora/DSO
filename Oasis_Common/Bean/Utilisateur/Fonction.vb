Public Class Fonction
    Private _id As Long
    Private _designation As String
    Private _libelle As String
    Private _Type As String
    Private _RorId As Long
    Private _isInactif As Boolean

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property Designation As String
        Get
            Return _designation
        End Get
        Set(value As String)
            _designation = value
        End Set
    End Property

    Public Property Libelle As String
        Get
            Return _libelle
        End Get
        Set(value As String)
            _libelle = value
        End Set
    End Property

    Public Property IsInactif As Boolean
        Get
            Return _isInactif
        End Get
        Set(value As Boolean)
            _isInactif = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return _Type
        End Get
        Set(value As String)
            _Type = value
        End Set
    End Property

    Public Property RorId As Long
        Get
            Return _RorId
        End Get
        Set(value As Long)
            _RorId = value
        End Set
    End Property

    ''' <summary>
    ''' return in "(id1,id2, ... , idn)"
    ''' </summary>
    ''' <param name="lstOrigine"></param>
    ''' <returns></returns>
    Public Shared Function getQueryInForIds(lstOrigine As List(Of Fonction)) As String
        Dim lstId As List(Of Long) = New List(Of Long)

        If lstOrigine Is Nothing Then Return ""
        For Each bean In lstOrigine
            lstId.Add(bean.Id)
        Next

        Return " in ( " + String.Join(",", lstId) + ") "

    End Function



End Class
