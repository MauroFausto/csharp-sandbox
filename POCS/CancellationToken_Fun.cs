using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;


namespace MainConsoleApp.POCS
{
    public class CancellationToken_Fun : IDisposable
    {
        private bool disposedValue;

        public CancellationToken_Fun()
        {
            
        }
        /// <summary>
        /// Source =>  https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken?view=net-7.0
        /// O exemplo a seguir usa um gerador de números randômicos para emular aplicação que lê uma coleção de dados -
        /// declarados como valores inteiros - de 11 diferentes instrumentos hipotéticos de medida. O valor 0 indica que
        /// não foi possível realizar nenhuma medida para algum dos instrumentos, nesse caso, a operação de cálculo de 
        /// média deverá ser cancelada e nenhum cálculo de média geral deve ser computado.
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CalculateOverallInstrumentsMean()
        {
            // Definição do token de cancelamento
            CancellationTokenSource cancellationTokenSource = new ();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Random rnd = new();
            Object lockObj = new ();

            List<Task<int[]>> tasks = new();

            // Token de cancelamento é passado como parâmetro para a TaskFactory
            TaskFactory factory = new TaskFactory(cancellationToken);
            
            for (int taskCtr = 0; taskCtr <= 10; taskCtr++)
            {
                int iteration = taskCtr + 1;
                tasks.Add(factory.StartNew(() =>
                {
                    int value;
                    int[] values = new int[10];
                    for (int ctr = 1; ctr <= 10; ctr++)
                    {
                        lock (lockObj)
                        {
                            value = rnd.Next(0, 101);
                        }
                        if (value == 0)
                        {
                            cancellationTokenSource.Cancel();
                            Console.WriteLine("Cancelling at task {0}", iteration);
                            break;
                        }
                        values[ctr - 1] = value;
                    }
                    return values;

                }, cancellationToken));
            }

            try
            {
                Task<double> fTask = factory.ContinueWhenAll(tasks.ToArray(), (results) =>
                {
                    Console.WriteLine("Calculando a média geral...");
                    long sum = 0;
                    int n = 0;
                    foreach (var t in results)
                    {
                        foreach (var r in t.Result)
                        {
                            sum += r;
                            n++;
                        }
                    }
                    return sum / (double)n;
                }, cancellationToken);

                Console.WriteLine("The mean is {0}.", fTask.Result);
            }
            catch (AggregateException aggrEx)
            {
                foreach (Exception e in aggrEx.InnerExceptions)
                {
                    if (e is TaskCanceledException)
                        Console.WriteLine(
                            "Não fo possível computar a média geral. Erro: {0}", 
                            ((TaskCanceledException)e).Message);
                    else
                        Console.WriteLine("Exceção genérica lançada: {0}", e.GetType().Name);
                }
            }
            finally
            {
                cancellationTokenSource.Dispose();
            }                
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~CancellationToken_Fun()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
