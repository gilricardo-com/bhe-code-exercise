using System;
using System.Collections.Generic;

namespace Sieve;

public interface ISieve
{
    long NthPrime(long n);
}

public class SieveImplementation : ISieve
{
    /// Devuelve el N-ésimo número primo usando indexación basada en 0
    
    public long NthPrime(long n)
    {
        if (n < 0)
            throw new ArgumentException("El índice no puede ser negativo");

        // Devolver automáticamente los primeros 5 primos para velocidad y simplicidad
        if (n == 0) return 2;
        if (n == 1) return 3;
        if (n == 2) return 5;
        if (n == 3) return 7;
        if (n == 4) return 11;

        // Para números mayores, usar la Criba de Eratóstenes
        return EncontrarPrimoConCriba(n);
    }

    /// <summary>
    /// Encuentra el N-ésimo primo usando la Criba de Eratóstenes
    /// </summary>
    private long EncontrarPrimoConCriba(long n)
    {
        // Estimar el tamaño necesario para la lista de booleanos
        int tamanioEstimado = EstimarTamanio(n);
        
        List<int> primosEncontrados = new List<int>();
        
        // Si la estimación es baja, aumentar el tamaño hasta encontrar suficientes primos
        while (primosEncontrados == null || primosEncontrados.Count <= n)
        {
            primosEncontrados = CribaDeEratostenes(tamanioEstimado);
            
            if (primosEncontrados.Count <= n)
            {
                tamanioEstimado = (int)(tamanioEstimado * 1.5);
            }
        }
        
        return primosEncontrados[(int)n];
    }

    /// <summary>
    /// Implementa la Criba de Eratóstenes según el algoritmo griego
    /// </summary>
    /// <param name="limite">Límite superior hasta donde hacer la criba</param>
    /// <returns>Lista de todos los números primos encontrados</returns>
    private List<int> CribaDeEratostenes(int limite)
    {
        if (limite < 2) return new List<int>();

        // Crear lista de booleanos - true significa "posible primo"
        bool[] esPrimo = new bool[limite + 1];
        
        // Marcar todos los números del 2 en adelante como posibles primos
        for (int i = 2; i <= limite; i++)
        {
            esPrimo[i] = true;
        }

        // Proceso principal de la criba
        for (int i = 2; i * i <= limite; i++)
        {
            if (esPrimo[i]) // Si i es primo
            {
                // Tachar todos los múltiplos de i, empezando desde i*i
                // (los múltiplos menores ya fueron tachados por números anteriores)
                for (int multiplo = i * i; multiplo <= limite; multiplo += i)
                {
                    esPrimo[multiplo] = false; // Marcar como no primo
                }
            }
        }

        // Recopilar todos los números que quedaron marcados como primos
        List<int> primos = new List<int>();
        for (int i = 2; i <= limite; i++)
        {
            if (esPrimo[i])
            {
                primos.Add(i);
            }
        }

        return primos;
    }

    /// <summary>
    /// Estima el tamaño necesario para encontrar el N-ésimo primo
    /// Usa aproximaciones matemáticas basadas en la distribución de primos
    /// </summary>
    /// <param name="n">Posición del primo buscado</param>
    /// <returns>Tamaño estimado para la criba</returns>
    private int EstimarTamanio(long n)
    {
        if (n < 6) return 20;

        // Usar la aproximación: N-ésimo primo ≈ n * ln(n)
        double logaritmoN = Math.Log(n);
        double estimacion = n * (logaritmoN + Math.Log(logaritmoN));
        
        // Agregar margen de seguridad
        return (int)(estimacion * 1.2) + 100;
    }
}