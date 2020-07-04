Imports System.IO

Public Class Ordonnance
    Property Id As Long
    Property PatientId As Long
    Property EpisodeId As Long
    Property UtilisateurCreation As Long
    Property DateCreation As Date
    Property DateValidation As Date
    Property UserValidation As Long
    Property DateEdition As Date
    Property Commentaire As String
    Property Renouvellement As Integer
    Property Inactif As Boolean
    Property Signature As String

    Public Function Clone() As Ordonnance
        Dim newInstance As Ordonnance = DirectCast(Me.MemberwiseClone(), Ordonnance)
        Return newInstance
    End Function

    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                writer.Write(Id) 'Long
                writer.Write(PatientId) 'Long
                writer.Write(EpisodeId) 'Long
                writer.Write(UtilisateurCreation) 'Long
                writer.Write(DateCreation.Ticks) 'Date -> Long
                writer.Write(DateValidation.Ticks) 'Date -> Long
                writer.Write(UserValidation) 'Long
                'writer.Write(_dateEdition.Ticks) 'Date -> LongK
                writer.Write(Commentaire) 'String
                writer.Write(Renouvellement) 'Int
            End Using
            Return m.ToArray()
        End Using
    End Function

    Public Shared Function Deserialize(ByVal data As Byte()) As Ordonnance
        Dim result As Ordonnance = New Ordonnance()
        Using m As MemoryStream = New MemoryStream(data)
            Using reader As BinaryReader = New BinaryReader(m)
                result.Id = reader.ReadInt64()
                result.PatientId = reader.ReadInt64()
                result.EpisodeId = reader.ReadInt64()
                result.UtilisateurCreation = reader.ReadInt64()
                result.DateCreation = New Date(reader.ReadInt64())
                result.DateValidation = New Date(reader.ReadInt64())
                result.UserValidation = reader.ReadInt64()
                'result.DateEdition = New Date(reader.ReadInt64())
                result.Commentaire = reader.ReadString()
                result.Renouvellement = reader.ReadInt32()
            End Using
        End Using
        Return result
    End Function

End Class
