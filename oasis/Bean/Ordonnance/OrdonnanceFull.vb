Imports System.IO
Imports Oasis_Common

<Serializable()>
Public Class OrdonnanceFull
    Property Ordonnance As OrdonnanceBase
    Property Details As List(Of OrdonnanceDetailBase)

    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                writer.Write(Ordonnance.Serialize().Length) 'Int
                writer.Write(Ordonnance.Serialize()) 'Dyn
                writer.Write(Details.Count) 'Int
                For Each x In Details
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
                result._ordonnance = OrdonnanceBase.Deserialize(reader.ReadBytes(ordonnanceSize))
                Dim detailListSize As Integer = reader.ReadInt32()
                result._details = New List(Of OrdonnanceDetailBase)
                For i = 0 To detailListSize - 1
                    Dim detailSize As Integer = reader.ReadInt32()
                    result._details.Add(OrdonnanceDetailBase.Deserialize(reader.ReadBytes(detailSize)))
                Next
            End Using
        End Using
        Return result
    End Function

    Public Shared Function Invoke(ordonnanceId As Integer) As OrdonnanceFull
        Dim ordonnanceDao As OrdonnanceDao = New OrdonnanceDao 'Why instanciate ?
        Dim ordonnanceDetailDao As OrdonnanceDetailDao = New OrdonnanceDetailDao 'Why instanciate ?

        Dim ordonnance As OrdonnanceBase = ordonnanceDao.GetOrdonnaceById(ordonnanceId)
        Dim ordonnanceDetailA As List(Of OrdonnanceDetailBase) = ordonnanceDetailDao.GetOrdonnanceLigneByOrdonnanceId(ordonnanceId)
        Dim ordonnanceDetail As List(Of OrdonnanceDetailBase) = TryCast(CObj(ordonnanceDetailA), List(Of OrdonnanceDetailBase))

        Dim ordonnanceFull As OrdonnanceFull = New OrdonnanceFull With {
            .Ordonnance = ordonnance,
            .Details = ordonnanceDetail
        }
        Return ordonnanceFull
    End Function

End Class

