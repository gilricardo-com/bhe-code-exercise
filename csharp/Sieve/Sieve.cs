using System;
using System.Collections.Generic;

namespace Sieve;

public interface ISieve
{
    long NthPrime(long n);
}

public class SieveImplementation : ISieve
{
    /// Returns the Nth prime number using 0-based indexing
       
    public long NthPrime(long n)
    {
        // Validate input
        if (n < 0)
            throw new ArgumentException("Index cannot be negative");

        // Return hardcoded values for first 5 primes for performance and simplicity
        if (n == 0) return 2;
        if (n == 1) return 3;
        if (n == 2) return 5;
        if (n == 3) return 7;
        if (n == 4) return 11;

        // For larger numbers, use the Sieve of Eratosthenes: here is the cool part
        return FindPrimeUsingSieve(n);
    }

    
    /// Finds the Nth prime using the Sieve of Eratosthenes algorithm
    
    private long FindPrimeUsingSieve(long n)
    {
        // Estimate the required size for the boolean array
        // How can we now how big the array should be? How can we estimate how many numbers we need to check the Nth prime?
        int estimatedSize = EstimateUpperBound(n);
        
        List<int> foundPrimes = new List<int>();

        // If estimation is too low, increase size until we find enough primes // Validating we found enough primes
        while (foundPrimes.Count <= n)
        {
            foundPrimes = SieveOfEratosthenes(estimatedSize); // Magic happens here (List is being filled)

            if (foundPrimes.Count <= n) // If not enough primes found, increase the size and try again
            {
                estimatedSize = (int)(estimatedSize * 1.5);
            }
        }
        
        return foundPrimes[(int)n];
    }

    /// Implements the Sieve of Eratosthenes algorithm according to the Greek method
    
    private List<int> SieveOfEratosthenes(int limit)
    {
        if (limit < 2) return new List<int>();

        // Create boolean array - true means "possible prime"
        bool[] isPrime = new bool[limit + 1];
        
        // Mark all numbers from 2 onwards as potential primes
        for (int i = 2; i <= limit; i++)
        {
            isPrime[i] = true;
        }

        // Main sieve process
        for (int i = 2; i * i <= limit; i++)
        {
            if (isPrime[i]) // If i is prime
            {
                // Mark all multiples of i, starting from i*i
                // (smaller multiples were already marked by previous numbers)
                for (int multiple = i * i; multiple <= limit; multiple += i)
                {
                    isPrime[multiple] = false; // Mark as not prime
                }
            }
        }

        // Collect all numbers that remained marked as prime
        List<int> primes = new List<int>();
        for (int i = 2; i <= limit; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// Estimates the required size to find the Nth prime
    
    private int EstimateUpperBound(long n)
    {
        if (n < 6) return 20;

        // Use approximation: Nth prime ≈ n * ln(n)
        double logN = Math.Log(n);
        double estimation = n * (logN + Math.Log(logN));
        
        // Add safety margin
        return (int)(estimation * 1.2) + 100;
    }
}