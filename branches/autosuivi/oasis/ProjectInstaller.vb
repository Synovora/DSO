Imports System.ComponentModel
Imports System.Configuration.Install

Public Class ProjectInstaller

    Public Sub New()
        MyBase.New()

        'Cet appel est requis par le Concepteur de composants.
        InitializeComponent()

        'Ajoutez le code d'initialisation après l'appel de InitializeComponent

    End Sub

    Private Sub ServiceProcessInstaller1_AfterInstall(sender As Object, e As InstallEventArgs) Handles ServiceProcessInstaller1.AfterInstall

    End Sub

    Private Sub ServiceInstaller1_AfterInstall(sender As Object, e As InstallEventArgs) Handles ServiceInstaller1.AfterInstall

    End Sub
End Class
