Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

<Serializable()>
Public Class OrdonnanceFull
    Private _ordonnance As Ordonnance
    Private _details As List(Of OrdonnanceDetail)

    Public Property Ordonnance As Ordonnance
        Get
            Return _ordonnance
        End Get
        Set(value As Ordonnance)
            _ordonnance = value
        End Set
    End Property

    Public Property Details As List(Of OrdonnanceDetail)
        Get
            Return _details
        End Get
        Set(value As List(Of OrdonnanceDetail))
            _details = value
        End Set
    End Property

    '[Size of ordonnance][Ordonnance][Size of list of detail][Size of detail][Detail]...
    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                writer.Write(_ordonnance.Serialize().Length) 'Int
                writer.Write(_ordonnance.Serialize()) 'Dyn
                writer.Write(_details.Count) 'Int
                For Each x In _details
                    writer.Write(x.Serialize().Length) 'Int
                    writer.Write(x.Serialize()) 'Dyn
                Next
            End Using
            Return m.ToArray()
        End Using
    End Function

    Public Shared Function Deserialize(ByVal data As Byte()) As OrdonnanceFull
        Dim result As OrdonnanceFull = New OrdonnanceFull()
        Using m As MemoryStream = New MemoryStream(data)
            Using reader As BinaryReader = New BinaryReader(m)
                Dim ordonnanceSize As Integer = reader.ReadInt32()
                result._ordonnance = Ordonnance.Deserialize(reader.ReadBytes(ordonnanceSize))
                Dim detailListSize As Integer = reader.ReadInt32()
                result._details = New List(Of OrdonnanceDetail)
                For i = 0 To detailListSize - 1
                    Dim detailSize As Integer = reader.ReadInt32()
                    result._details.Add(OrdonnanceDetail.Deserialize(reader.ReadBytes(detailSize)))
                Next
            End Using
        End Using
        Return result
    End Function

    Public Shared Function Invoke(ordonnanceId As Integer) As OrdonnanceFull
        Dim ordonnanceDao As OrdonnanceDao = New OrdonnanceDao 'Why instanciate ?
        Dim ordonnanceDetailDao As OrdonnanceDetailDao = New OrdonnanceDetailDao 'Why instanciate ?

        Dim ordonnance As Ordonnance = ordonnanceDao.GetOrdonnaceById(ordonnanceId)
        Dim ordonnanceDetail As List(Of OrdonnanceDetail) = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(ordonnanceId)

        Dim ordonnanceFull As OrdonnanceFull = New OrdonnanceFull With {
            ._ordonnance = ordonnance,
            ._details = ordonnanceDetail
        }
        Return ordonnanceFull
    End Function

End Class

