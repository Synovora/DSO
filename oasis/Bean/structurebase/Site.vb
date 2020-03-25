Public Class Site
    Private _oa_site_id As Long
    Private _oa_site_description As String
    Private _oa_site_territoire_id As Integer
    Private _oa_site_unite_sanitaire_id As Long
    Private _oa_site_adresse1 As String
    Private _oa_site_adresse2 As String
    Private _oa_site_ville As String
    Private _oa_site_code_postal As String
    Private _oa_site_inactif As Boolean
    Private _telephone As String
    Private _mail As String
    Private _fax As String

    Public Property Oa_site_id As Long
        Get
            Return _oa_site_id
        End Get
        Set(value As Long)
            _oa_site_id = value
        End Set
    End Property

    Public Property Oa_site_description As String
        Get
            Return _oa_site_description
        End Get
        Set(value As String)
            _oa_site_description = value
        End Set
    End Property

    Public Property Oa_site_territoire_id As Object
        Get
            Return _oa_site_territoire_id
        End Get
        Set(value As Object)
            _oa_site_territoire_id = value
        End Set
    End Property

    Public Property Oa_site_unite_sanitaire_id As Long
        Get
            Return _oa_site_unite_sanitaire_id
        End Get
        Set(value As Long)
            _oa_site_unite_sanitaire_id = value
        End Set
    End Property

    Public Property Oa_site_adresse1 As String
        Get
            Return _oa_site_adresse1
        End Get
        Set(value As String)
            _oa_site_adresse1 = value
        End Set
    End Property

    Public Property Oa_site_adresse2 As String
        Get
            Return _oa_site_adresse2
        End Get
        Set(value As String)
            _oa_site_adresse2 = value
        End Set
    End Property

    Public Property Oa_site_ville As String
        Get
            Return _oa_site_ville
        End Get
        Set(value As String)
            _oa_site_ville = value
        End Set
    End Property

    Public Property Oa_site_code_postal As String
        Get
            Return _oa_site_code_postal
        End Get
        Set(value As String)
            _oa_site_code_postal = value
        End Set
    End Property

    Public Property Oa_site_inactif As Boolean
        Get
            Return _oa_site_inactif
        End Get
        Set(value As Boolean)
            _oa_site_inactif = value
        End Set
    End Property

    Public Property Telephone As String
        Get
            Return _telephone
        End Get
        Set(value As String)
            _telephone = value
        End Set
    End Property

    Public Property Mail As String
        Get
            Return _mail
        End Get
        Set(value As String)
            _mail = value
        End Set
    End Property

    Public Property Fax As String
        Get
            Return _fax
        End Get
        Set(value As String)
            _fax = value
        End Set
    End Property

    ''' <summary>
    ''' return in "(id1,id2, ... , idn)"
    ''' </summary>
    ''' <param name="lstOrigine"></param>
    ''' <returns></returns>
    Public Shared Function getQueryInForIds(lstOrigine As List(Of Site)) As String
        Dim lstId As List(Of Long) = New List(Of Long)

        If lstOrigine Is Nothing Then Return ""
        For Each sitelu In lstOrigine
            lstId.Add(sitelu.Oa_site_id)
        Next

        Return " in ( " + String.Join(",", lstId) + ") "

    End Function

End Class
