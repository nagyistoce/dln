// -----------------------------------------------------------------------
// <copyright file="ThreadSafeRandom.cs" company="Eiríkur Fannar Torfason">
// Copyright (c) 2013 All Rights Reserved
// </copyright>
// -----------------------------------------------------------------------

namespace Eft.BioNN.Engine.Utils
{
	using System;
	using System.Threading;

	/// <summary>
	/// A thread-safe provider of random number generators.
	/// </summary>
	/// <remarks>
	/// Code by Danny Pflughoeft. See:
	/// http://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
	/// </remarks>
	public class ThreadSafeRandom
	{
		/// <summary>
		/// The global random generator, used to generate seeds for thread-local generators.
		/// </summary>
		private static Random global = new Random();

		/// <summary>
		/// The thread-local random number generator.
		/// </summary>
		[ThreadStatic]
		private static Random local;

		/// <summary>
		/// Gets a thread-local random number generator.
		/// </summary>
		/// <returns>A random number generator.</returns>
		public static Random GetThreadRandom()
		{
			//
			// Is this the first time this method is called on the current thread?
			//
		    Random instance = local;
		    if (instance == null)
		    {
				//
				// Get a seed from the global generator and use it to initialize a new random number generator. 
				//
		        int seed;
				lock (global)
				{
					seed = global.Next();
				}

		        local = instance = new Random(seed);
		    }

		    return instance;
		}
	}
}
