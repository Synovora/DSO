Imports System.Data

''' <summary>
''' Fabrique une ligne de résultat sans base de données. DataTableReader est à
''' la fois un DbDataReader et un IDataRecord, donc il convient à toutes les
''' signatures de BuildBean, et il accepte DBNull dans n'importe quelle colonne.
''' </summary>
Friend Module LigneDeTest

    ''' <summary>
    ''' Une ligne portant exactement les colonnes données. Toute colonne absente
    ''' de valeurs vaut DBNull. Le lecteur est déjà positionné sur la ligne.
    ''' </summary>
    Friend Function Ligne(colonnes As IEnumerable(Of String), valeurs As IDictionary(Of String, Object)) As DataTableReader
        Dim table As New DataTable()
        For Each colonne In colonnes
            table.Columns.Add(colonne, GetType(Object))
        Next

        Dim rangee = table.NewRow()
        For Each colonne In colonnes
            If valeurs IsNot Nothing AndAlso valeurs.ContainsKey(colonne) Then
                rangee(colonne) = valeurs(colonne)
            Else
                rangee(colonne) = DBNull.Value
            End If
        Next
        table.Rows.Add(rangee)

        Dim lecteur = table.CreateDataReader()
        If Not lecteur.Read() Then Throw New InvalidOperationException("Ligne de test illisible.")
        Return lecteur
    End Function

End Module
