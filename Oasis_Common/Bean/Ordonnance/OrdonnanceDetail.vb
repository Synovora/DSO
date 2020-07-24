Imports System.IO

Public Class OrdonnanceDetail

    Property LigneId As Integer
    Property OrdonnanceId As Integer
    Property Traitement As Boolean
    Property TraitementId As Integer
    Property Ald As Boolean
    Property ADelivrer As Boolean
    Property OrdreAffichage As Integer
    Property MedicamentCis As Integer
    Property MedicamentDci As String
    Property DateDebut As Date
    Property DateFin As Date
    Property Duree As Integer
    Property Posologie As String
    Property PosologieBase As String
    Property PosologieRythme As Integer
    Property PosologieMatin As Integer
    Property PosologieMidi As Integer
    Property PosologieApresMidi As Integer
    Property PosologieSoir As Integer
    Property FractionMatin As String
    Property FractionMidi As String
    Property FractionApresMidi As String
    Property FractionSoir As String
    Property PosologieCommentaire As String
    Property Commentaire As String
    Property Fenetre As Boolean
    Property FenetreDateDebut As Date
    Property FenetreDateFin As Date
    Property FenetreCommentaire As String
    Property Inactif As Boolean

    Public Function Clone() As OrdonnanceDetail
        Dim newInstance As OrdonnanceDetail = DirectCast(Me.MemberwiseClone(), OrdonnanceDetail)
        Return newInstance
    End Function

    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                'writer.Write(_ligneId) 'Int
                writer.Write(OrdonnanceId) 'Int
                writer.Write(Traitement) 'Bool
                writer.Write(TraitementId) 'Int
                writer.Write(Ald) 'Bool
                writer.Write(ADelivrer) 'Bool
                writer.Write(OrdreAffichage) 'Int
                writer.Write(MedicamentCis) 'Int
                writer.Write(If(MedicamentDci, "")) 'String
                writer.Write(DateDebut.Ticks) 'Date -> Long
                writer.Write(DateFin.Ticks) 'Date -> Long
                writer.Write(Duree) 'Int
                writer.Write(If(Posologie, "")) 'String
                writer.Write(If(PosologieBase, "")) 'String
                writer.Write(PosologieRythme) 'Int
                writer.Write(PosologieMatin) 'Int
                writer.Write(PosologieMidi) 'Int
                writer.Write(PosologieApresMidi) 'Int
                writer.Write(PosologieSoir) 'Int
                writer.Write(FractionMatin) 'String
                writer.Write(FractionMidi) 'String
                writer.Write(If(FractionApresMidi, "")) 'String
                writer.Write(FractionSoir) 'String
                writer.Write(If(PosologieCommentaire, "")) 'String
                writer.Write(If(Commentaire, "")) 'String
                writer.Write(Fenetre) 'Bool
                writer.Write(FenetreDateDebut.Ticks) 'Date -> Long
                writer.Write(FenetreDateFin.Ticks) 'Date -> Long
                writer.Write(If(FenetreCommentaire, "")) 'String
            End Using
            Return m.ToArray()
        End Using
    End Function

    Public Shared Function Deserialize(ByVal data As Byte()) As OrdonnanceDetail
        Dim result As OrdonnanceDetail = New OrdonnanceDetail()
        Using m As MemoryStream = New MemoryStream(data)
            Using reader As BinaryReader = New BinaryReader(m)
                'result.LigneId = reader.ReadInt32()
                result.OrdonnanceId = reader.ReadInt32()
                result.Traitement = reader.ReadBoolean()
                result.TraitementId = reader.ReadInt32()
                result.Ald = reader.ReadBoolean()
                result.ADelivrer = reader.ReadBoolean()
                result.OrdreAffichage = reader.ReadInt32()
                result.MedicamentCis = reader.ReadInt32()
                result.MedicamentDci = reader.ReadString()
                result.DateDebut = New Date(reader.ReadInt64())
                result.DateFin = New Date(reader.ReadInt64())
                result.Duree = reader.ReadInt32()
                result.Posologie = reader.ReadString()
                result.PosologieBase = reader.ReadString()
                result.PosologieRythme = reader.ReadInt32()
                result.PosologieMatin = reader.ReadInt32()
                result.PosologieMidi = reader.ReadInt32()
                result.PosologieApresMidi = reader.ReadInt32()
                result.PosologieSoir = reader.ReadInt32()
                result.FractionMatin = reader.ReadString()
                result.FractionMidi = reader.ReadString()
                result.FractionApresMidi = reader.ReadString()
                result.FractionSoir = reader.ReadString()
                result.PosologieCommentaire = reader.ReadString()
                result.Commentaire = reader.ReadString()
                result.Fenetre = reader.ReadBoolean()
                result.FenetreDateDebut = New Date(reader.ReadInt64())
                result.FenetreDateFin = New Date(reader.ReadInt64())
                result.FenetreCommentaire = reader.ReadString()
            End Using
        End Using
        Return result
    End Function

End Class
