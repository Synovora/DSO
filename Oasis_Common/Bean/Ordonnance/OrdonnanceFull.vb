Imports System.IO

<Serializable()>
Public Class OrdonnanceFull

    Property Ordonnance As Ordonnance
    Property Details As List(Of OrdonnanceDetail)

    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                ' Sérialiser une seule fois : appeler Serialize() deux fois par bloc
                ' refaisait tout le travail pour rien.
                Dim octetsOrdonnance = Ordonnance.Serialize()
                writer.Write(octetsOrdonnance.Length) 'Int
                writer.Write(octetsOrdonnance) 'Dyn
                writer.Write(Details.Count) 'Int
                For Each detail In Details
                    Dim octetsDetail = detail.Serialize()
                    writer.Write(octetsDetail.Length) 'Int
                    writer.Write(octetsDetail) 'Dyn
                Next
            End Using
            Return m.ToArray()
        End Using
    End Function

    Public Shared Function Deserialize(ByVal data As Byte()) As OrdonnanceFull
        Dim result As OrdonnanceFull = New OrdonnanceFull()
        Using m As MemoryStream = New MemoryStream(data)
            Using reader As BinaryReader = New BinaryReader(m)
                ' Les tailles annoncées sont bornées par la taille réelle du tampon :
                ' une charge tronquée ou trafiquée ne doit pas provoquer une
                ' allocation démesurée.
                Dim ordonnanceSize As Integer = TailleValide(reader.ReadInt32(), data.Length)
                result._Ordonnance = Ordonnance.Deserialize(reader.ReadBytes(ordonnanceSize))
                Dim detailListSize As Integer = TailleValide(reader.ReadInt32(), data.Length)
                result._Details = New List(Of OrdonnanceDetail)
                For i = 0 To detailListSize - 1
                    Dim detailSize As Integer = TailleValide(reader.ReadInt32(), data.Length)
                    result._Details.Add(OrdonnanceDetail.Deserialize(reader.ReadBytes(detailSize)))
                Next
            End Using
        End Using
        Return result
    End Function

    ''' <summary>
    ''' Valide une taille lue dans la charge : ni négative, ni supérieure à la
    ''' charge elle-même.
    ''' </summary>
    Private Shared Function TailleValide(taille As Integer, tailleMaximum As Integer) As Integer
        If taille < 0 OrElse taille > tailleMaximum Then
            Throw New InvalidDataException("Charge signée illisible : taille annoncée incohérente.")
        End If
        Return taille
    End Function

End Class

