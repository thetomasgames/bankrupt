using System;
using System.Collections.Generic;

namespace Bankrupt
{
	/// <summary>
	/// Classe que representa um dado aleatório com uma lista de possibilidades que pode ser rolado.
	/// </summary>
	public class Dado
	{
		private Random rand;
		private List<int> opcoes;

		public Dado (List<int> opcoes)
		{
			this.opcoes = opcoes;
			rand = new Random (System.Environment.TickCount);
		}

		/// <summary>
		/// Retorna uma das opções possíveis com chances equiprovaveis.
		/// </summary>
		public int Rolar ()
		{
			return opcoes [rand.Next (opcoes.Count)];
		}
	}
}

