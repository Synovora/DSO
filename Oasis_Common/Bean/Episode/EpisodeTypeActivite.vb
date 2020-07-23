Public Class EpisodeTypeActivite

    Enum EnumEditMode
        Modification = 1
        Creation = 2
    End Enum

    Property Type As String
    Property Nature As String
    Property Description As String
    Property Inactif As Boolean
End Class
