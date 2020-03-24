Public Class SousEpisodeFusion
    Property USNom As String
    Property USAdr1 As String
    Property USAdr2 As String
    Property USCP As String
    Property USVille As String
    Property USTel As String
    Property USFax As String
    Property USEmail As String

    Property SiteNom As String
    Property SiteAdr1 As String
    Property SiteAdr2 As String
    Property SiteCP As String
    Property SiteVille As String
    Property SiteTel As String
    Property SiteFax As String
    Property SiteEmail As String

    Property Patient_PrenomNom As String
    Property Patient_Date_Naissance As String
    Property Patient_NIR As String
    Property Patient_Age As String
    Property Patient_Poids As String
    Property Patient_sexe As String

    Property Episode_DateHeure As String


    Property Type_Libelle As String
    Property Sous_Type_Libelle As String
    Property Sous_Type_Libelle_Detail_ALD As String
    Property Sous_Type_Libelle_Detail_Non_ALD As String
    Property Commentaire As String
    Property ALD_Avec_Entete As String = "AFFECTION EXONERANTE" & vbCrLf & "Prescriptions relatives au traitement de l’affection de longue durée reconnue (liste ou hors liste)"
    Property ALD_Sans_Entete As String = "MALADIES INTERCURRENTES" & vbCrLf & "Prescriptions sans rapport avec l'affection de longue durée"

    Property Signataire_PrenomNom As String
    Property Signataire_Fonction As String

    Property Signature_Date As String

End Class
