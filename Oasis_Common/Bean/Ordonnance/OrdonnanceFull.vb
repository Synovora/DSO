Imports System.IO

<Serializable()>
Public Class OrdonnanceFull

    Property Ordonnance As Ordonnance
    Property Details As List(Of OrdonnanceDetail)

    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                writer.Write(Ordonnance.Serialize().Length) 'Int
                writer.Write(Ordonnance.Serialize()) 'Dyn
                writer.Write(Details.Count) 'Int
                For Each detail In Details
                    writer.Write(detail.Serialize().Length) 'Int
                    writer.Write(detail.Serialize()) 'Dyn
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
                result._Ordonnance = Ordonnance.Deserialize(reader.ReadBytes(ordonnanceSize))
                Dim detailListSize As Integer = reader.ReadInt32()
                result._Details = New List(Of OrdonnanceDetail)
                For i = 0 To detailListSize - 1
                    Dim detailSize As Integer = reader.ReadInt32()
                    result._Details.Add(OrdonnanceDetail.Deserialize(reader.ReadBytes(detailSize)))
                Next
            End Using
        End Using
        Return result
    End Function

End Class

