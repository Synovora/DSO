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

End Class

