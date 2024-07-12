using MainConsoleApp.POCS;

namespace MainConsoleApp
{
    internal class Program
    {
        /// <tutorial - Método main assíncrono - antes da versão 7.1 do .NET>
        /// Como tornar o método Main assíncrono:
        /// 
        /// E.G.:
        /// static void Main(string[] args)
        /// {
        ///    Console.WriteLine("Antes do C# 7.1, Para usar o método async");
        ///    Console.WriteLine($"Execução do Main iniciou em : {System.DateTime.Now}");
        ///
        ///    Task.Delay(2000).GetAwaiter().GetResult(); <== Chamada em de thread assíncrona
        ///
        ///    Console.WriteLine($"Execução do Main terminou em : {System.DateTime.Now}");
        ///    Console.ReadKey();
        /// }
        /// </tutorial>
        /// 
        /// ==> Outra solução possível: 
        /// // Source: https://www.macoratti.net/20/01/c_asyncmain1.htm
        /// 
        /// using System;
        /// using System.Net.Http;
        /// using System.Threading.Tasks;
        /// 
        /// namespace Cshp_MainAsync 
        /// {
        ///     class Program 
        ///     {
        ///         static void Main(string[] args) 
        ///         {
        ///             MainAsync().Wait();
        ///         }
        ///         
        ///         static async Task MainAsync()
        ///         {
        ///             Console.WriteLine("Método assíncrono MainAsync");
        ///             var client = new HttpCliente();
        ///             byte[] resultado = await client.GetByteArrayAsync("http://www.macoratti.net");
        ///             Console.WriteLine(resultado.Length);
        ///         }
        ///     }
        /// }
        /// 
        /// <param name="args"></param>
        static async void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            using (CancellationToken_Fun cancellationToken_Fun = new CancellationToken_Fun())
            {
                await cancellationToken_Fun.CalculateOverallInstrumentsMean();
            }
        }
    }
}
