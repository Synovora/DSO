Imports DeepEqual.Syntax
Imports Nethereum.Signer
Imports Oasis_WF
Imports Oasis_Common

<TestClass()> Public Class TestUtilisateur
    <TestMethod()> Public Sub TestUpdatePk()
        Dim ecKey As EthECKey = EthECKey.GenerateKey()
        Console.WriteLine("0x" & BitConverter.ToString(ecKey.GetPrivateKeyAsBytes()).Replace("-", ""))
        Console.WriteLine(ecKey.GetPublicAddress())
    End Sub
End Class

<TestClass()> Public Class TestOrdonnance

    Public Shared Function GenerateOrdonnance() As Ordonnance
        Dim ordonnance As New Ordonnance()
        ordonnance.Id = 1337
        ordonnance.PatientId = 666
        ordonnance.EpisodeId = 333
        ordonnance.UtilisateurCreation = 12341234
        ordonnance.DateCreation = New Date(1591291043)
        ordonnance.DateValidation = New Date(1591291044)
        ordonnance.UserValidation = 1
        ordonnance.Commentaire = "this is a test"
        ordonnance.Renouvellement = 1
        Return ordonnance
    End Function

    <TestMethod()> Public Sub TestSerializeDeserialize()
        Dim ordonnance As Ordonnance = GenerateOrdonnance()
        Dim ordonnanceSerialized As Byte() = ordonnance.Serialize()

        Console.WriteLine(BitConverter.ToString(ordonnanceSerialized))
        Dim ordonnance2 As Ordonnance = ordonnance.Deserialize(ordonnanceSerialized)

        ordonnance.ShouldDeepEqual(ordonnance2)
    End Sub
End Class

<TestClass()> Public Class TestOrdonnanceDetail

    Public Shared Function GenerateOrdonnanceDetail() As OrdonnanceDetail
        Dim patientDao As New PatientDao
        Dim ordonnanceDetail As New OrdonnanceDetail
        'ordonnanceDetail.LigneId = 1337
        ordonnanceDetail.Traitement = True
        ordonnanceDetail.TraitementId = 33
        ordonnanceDetail.OrdreAffichage = 1
        ordonnanceDetail.Ald = True
        ordonnanceDetail.ADelivrer = True
        ordonnanceDetail.MedicamentCis = 3
        ordonnanceDetail.MedicamentDci = ""
        ordonnanceDetail.DateDebut = New Date(1337)
        ordonnanceDetail.DateFin = New Date(1337)
        ordonnanceDetail.Duree = 1337
        ordonnanceDetail.Posologie = "qwer"
        ordonnanceDetail.PosologieBase = "ewq"
        ordonnanceDetail.PosologieRythme = 3
        ordonnanceDetail.PosologieMatin = 3
        ordonnanceDetail.PosologieMidi = 3
        ordonnanceDetail.PosologieApresMidi = 4
        ordonnanceDetail.PosologieSoir = 4
        ordonnanceDetail.FractionMatin = "qwer"
        ordonnanceDetail.FractionMidi = "qwer"
        ordonnanceDetail.FractionApresMidi = "werqwerqwer"
        ordonnanceDetail.FractionSoir = "qwerqwr"
        ordonnanceDetail.PosologieCommentaire = "qwerqwer"
        ordonnanceDetail.Commentaire = "qwerqwre"
        ordonnanceDetail.Fenetre = True
        ordonnanceDetail.FenetreDateDebut = New Date(1337)
        ordonnanceDetail.FenetreDateFin = New Date(1337)
        ordonnanceDetail.FenetreCommentaire = "rqwerqwer"
        ordonnanceDetail.Inactif = False
        Return ordonnanceDetail
    End Function

    <TestMethod()> Public Sub TestSerializeDeserialize()
        Dim ordonnanceDetail As OrdonnanceDetail = GenerateOrdonnanceDetail()
        Dim ordonnanceSerialized As Byte() = ordonnanceDetail.Serialize()

        Console.WriteLine(BitConverter.ToString(ordonnanceSerialized))
        Dim ordonnanceDetail2 As OrdonnanceDetail = ordonnanceDetail.Deserialize(ordonnanceSerialized)

        ordonnanceDetail.ShouldDeepEqual(ordonnanceDetail2)
    End Sub
End Class

<TestClass()> Public Class TestOrdonnanceFull

    <TestMethod()> Public Sub TestSerializeDeserialize()
        Dim ordonnanceFull As New OrdonnanceFull()
        Dim ordonnance As Ordonnance = TestOrdonnance.GenerateOrdonnance()
        Dim ordonnanceDetail As OrdonnanceDetail = TestOrdonnanceDetail.GenerateOrdonnanceDetail()

        ordonnanceFull.Ordonnance = ordonnance
        ordonnanceFull.Details = New List(Of OrdonnanceDetail) From {
            ordonnanceDetail,
            ordonnanceDetail,
            ordonnanceDetail
        }

        Dim ordonnanceFullSerialized As Byte() = ordonnanceFull.Serialize()

        Console.WriteLine(BitConverter.ToString(ordonnanceFullSerialized))
        Dim ordonnanceFull2 As OrdonnanceFull = OrdonnanceFull.Deserialize(ordonnanceFullSerialized)

        ordonnanceFull.ShouldDeepEqual(ordonnanceFull2)
    End Sub
End Class

<TestClass()> Public Class TestSousEpisode
    Public Shared Function GenerateSousEpisode() As SousEpisode
        Dim sousEpisode As New SousEpisode
        sousEpisode.Id = 2337
        sousEpisode.IdIntervenant = 11
        sousEpisode.EpisodeId = 33
        sousEpisode.IdSousEpisodeType = 1
        sousEpisode.IdSousEpisodeSousType = 2
        sousEpisode.CreateUserId = 3
        sousEpisode.HorodateCreation = New Date(0)
        sousEpisode.LastUpdateUserId = 1337
        sousEpisode.HorodateLastUpdate = New Date(0)
        sousEpisode.ValidateUserId = 123
        sousEpisode.HorodateValidate = New Date(0)
        sousEpisode.Commentaire = ""
        sousEpisode.IsALD = True
        sousEpisode.IsReponse = True
        sousEpisode.DelaiSinceValidation = 0
        sousEpisode.IsReponseRecue = True
        sousEpisode.HorodateLastRecu = New Date(0)
        'sousEpisode.isInactif = False
        'sousEpisode.lstDetail As List(Of SousEpisodeDetailSousType)
        'sousEpisode.Signature = ""
        Return sousEpisode
    End Function

    <TestMethod()> Public Sub TestSerializeDeserialize()
        Dim sousEpisode As SousEpisode = GenerateSousEpisode()
        Dim sousEpisodeSerialized As Byte() = sousEpisode.Serialize()

        Console.WriteLine(BitConverter.ToString(sousEpisodeSerialized))
        Dim ordonnanceDetail2 As SousEpisode = SousEpisode.Deserialize(sousEpisodeSerialized)

        sousEpisode.ShouldDeepEqual(ordonnanceDetail2)
    End Sub
End Class