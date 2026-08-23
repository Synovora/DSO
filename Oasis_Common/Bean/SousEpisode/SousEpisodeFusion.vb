Public Class SousEpisodeFusion
    Public Property USNom As String
    Public Property USAdr1 As String
    Public Property USAdr2 As String
    Public Property USCP As String
    Public Property USVille As String
    Public Property USTel As String
    Public Property USFax As String
    Public Property USEmail As String

    Public Property SiteNom As String
    Public Property SiteAdr1 As String
    Public Property SiteAdr2 As String
    Public Property SiteCP As String
    Public Property SiteVille As String
    Public Property SiteTel As String
    Public Property SiteFax As String
    Public Property SiteEmail As String

    Public Property IntervenantNom
    Public Property IntervenantSpecialite
    Public Property IntervenantStructure

    Public Property Patient_Prenom As String
    Public Property Patient_Nom As String
    Public Property Patient_Addresse_1 As String
    Public Property Patient_Addresse_2 As String
    Public Property Patient_Ville As String
    Public Property Patient_Tel_1 As String
    Public Property Patient_Tel_2 As String
    Public Property Patient_PrenomNom As String
    Public Property Patient_Date_Naissance As String
    Public Property Patient_NIR As String
    Public Property Patient_Age As String
    Public Property Patient_Poids As String
    Public Property Patient_FC As String
    Public Property Patient_PAS As String
    Public Property Patient_PAD As String
    Public Property Patient_SAT As String
    Public Property Patient_Dextro As String
    Public Property Patient_sexe As String

    Public Property Episode_DateHeure As String


    Public Property Type_Libelle As String
    Public Property Sous_Type_Libelle As String
    Public Property Sous_Type_Libelle_Detail_ALD As String
    Public Property Sous_Type_Libelle_Detail_commentaire_ALD As String
    Public Property Sous_Type_Libelle_Detail_Non_ALD As String
    Public Property Sous_Type_Libelle_Detail_commentaire_non_ALD As String
    Public Property Commentaire As String
    Public Property ALD_Avec_Entete As String = "AFFECTION EXONERANTE" & vbCrLf &
                                        "Prescriptions relatives au traitement de l’affection de longue durée reconnue (liste ou hors liste)"
    Public Property ALD_Avec_FaireFaire As String

    Public Property ALD_Sans_Entete As String = "MALADIES INTERCURRENTES" & vbCrLf &
                                         "Prescriptions sans rapport avec l'affection de longue durée"
    Public Property ALD_Sans_FaireFaire As String

    Public Property Signataire_PrenomNom As String
    Public Property Signataire_Fonction As String
    Public Property Signataire_PrenomNom_ALD As String
    Public Property Signataire_Fonction_ALD As String
    Public Property Signature_Date As String

    Public Property Reference As String

    Public Property Intervenant_Destinataire As String
    Public Property Contexte As String
    Public Property Antecedent As String
    Public Property Traitement As String


End Class
