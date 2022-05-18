Imports System
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks

Namespace HttpClientSample
    Public Class Product
        Public Property Id As String
        Public Property Name As String
        Public Property Price As Decimal
        Public Property Category As String
    End Class

    Class Program
        Shared client As HttpClient = New HttpClient()

        Private Shared Sub ShowProduct(ByVal product As Product)
            Console.WriteLine($"Name: {product.Name}\tPrice: " & $"{product.Price}\tCategory: {product.Category}")
        End Sub

        Private Shared Async Function CreateProductAsync(ByVal product As Product) As Task(Of Uri)
            Dim response As HttpResponseMessage = Await client.PostAsJsonAsync("api/products", product)
            response.EnsureSuccessStatusCode()
            Return response.Headers.Location
        End Function

        Private Shared Async Function GetProductAsync(ByVal path As String) As Task(Of Product)
            Dim product As Product = Nothing
            Dim response As HttpResponseMessage = Await client.GetAsync(path)

            If response.IsSuccessStatusCode Then
                product = Await response.Content.ReadAsAsync(Of Product)()
            End If

            Return product
        End Function

        Private Shared Async Function UpdateProductAsync(ByVal product As Product) As Task(Of Product)
            Dim response As HttpResponseMessage = Await client.PutAsJsonAsync($"api/products/{product.Id}", product)
            response.EnsureSuccessStatusCode()
            product = Await response.Content.ReadAsAsync(Of Product)()
            Return product
        End Function

        Private Shared Async Function DeleteProductAsync(ByVal id As String) As Task(Of HttpStatusCode)
            Dim response As HttpResponseMessage = Await client.DeleteAsync($"api/products/{id}")
            Return response.StatusCode
        End Function

        Private Shared Sub Main()
            RunAsync().GetAwaiter().GetResult()
        End Sub

        Private Shared Async Function RunAsync() As Task
            client.BaseAddress = New Uri("http://localhost:64195/")
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

            Try
                Dim product As Product = New Product With {
                    .Name = "Gizmo",
                    .Price = 100,
                    .Category = "Widgets"
                }
                Dim url = Await CreateProductAsync(product)
                Console.WriteLine($"Created at {url}")
                product = Await GetProductAsync(url.PathAndQuery)
                ShowProduct(product)
                Console.WriteLine("Updating price...")
                product.Price = 80
                Await UpdateProductAsync(product)
                product = Await GetProductAsync(url.PathAndQuery)
                ShowProduct(product)
                Dim statusCode = Await DeleteProductAsync(product.Id)
                Console.WriteLine($"Deleted (HTTP Status = {CInt(statusCode)})")
            Catch e As Exception
                Console.WriteLine(e.Message)
            End Try

            Console.ReadLine()
        End Function
    End Class
End Namespace
