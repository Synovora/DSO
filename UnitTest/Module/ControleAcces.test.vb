Imports Oasis_Common

''' <summary>
''' Un écran ou un épisode inscrit ne peut pas être ouvert une seconde fois
''' tant qu'il n'a pas été retiré.
''' </summary>
<TestClass()> Public Class TestControleAcces

    <TestMethod()> Public Sub UnEcranInscritNEstPlusAccessible()
        ControleAccesForm.RemoveFormToControl("SYNTHESE")
        Assert.IsTrue(ControleAccesForm.IsAccessToFormOK("SYNTHESE"))

        ControleAccesForm.AddFormToControl("SYNTHESE")
        Assert.IsFalse(ControleAccesForm.IsAccessToFormOK("SYNTHESE"))
        Assert.IsTrue(ControleAccesForm.IsAccessToFormOK("EPISODE"), "un autre écran reste libre")

        ControleAccesForm.RemoveFormToControl("SYNTHESE")
        Assert.IsTrue(ControleAccesForm.IsAccessToFormOK("SYNTHESE"))
    End Sub

    <TestMethod()> Public Sub InscrireDeuxFoisPuisRetirerUneFoisLibereLEcran()
        ControleAccesForm.AddFormToControl("LIGNE_DE_VIE")
        ControleAccesForm.AddFormToControl("LIGNE_DE_VIE")
        ControleAccesForm.RemoveFormToControl("LIGNE_DE_VIE")
        Assert.IsTrue(ControleAccesForm.IsAccessToFormOK("LIGNE_DE_VIE"))
    End Sub

    <TestMethod()> Public Sub RetirerUnEcranNonInscritNeFaitRien()
        ControleAccesForm.RemoveFormToControl("JAMAIS_INSCRIT")
        Assert.IsTrue(ControleAccesForm.IsAccessToFormOK("JAMAIS_INSCRIT"))
    End Sub

    <TestMethod()> Public Sub UnEpisodeInscritNEstPlusAccessible()
        ControleAccesEpisode.RemoveEpisodeToControl(42)
        Assert.IsTrue(ControleAccesEpisode.IsAccessToEpisodeOK(42))

        ControleAccesEpisode.AddEpisodeToControl(42)
        Assert.IsFalse(ControleAccesEpisode.IsAccessToEpisodeOK(42))
        Assert.IsTrue(ControleAccesEpisode.IsAccessToEpisodeOK(43), "un autre épisode reste libre")

        ControleAccesEpisode.RemoveEpisodeToControl(42)
        Assert.IsTrue(ControleAccesEpisode.IsAccessToEpisodeOK(42))
    End Sub

    <TestMethod()> Public Sub InscrireDeuxFoisPuisRetirerUneFoisLibereLEpisode()
        ControleAccesEpisode.AddEpisodeToControl(7)
        ControleAccesEpisode.AddEpisodeToControl(7)
        ControleAccesEpisode.RemoveEpisodeToControl(7)
        Assert.IsTrue(ControleAccesEpisode.IsAccessToEpisodeOK(7))
    End Sub

End Class
