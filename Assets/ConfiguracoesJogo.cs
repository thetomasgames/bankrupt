using System;
using System.Collections.Generic;

namespace Bankrupt
{
	/// <summary>
	/// Configuracoes possíveis para o jogo.
	/// </summary>
	public class ConfiguracoesJogo
	{
		public int dinheiroInicial;
		public List<int> opcoesDado;
		public int valorRecebidoPorVoltaCompleta;
		public int numeroMaximoRodadas;

		public List<Player> listaplayers;
		public List<CasaTabuleiro> casasTabuleiro;

		public ConfiguracoesJogo ()
		{
			dinheiroInicial = 300;
			opcoesDado = new List<int>{ 1, 2, 3, 4, 5, 6 };
			valorRecebidoPorVoltaCompleta = 100;
			numeroMaximoRodadas = 1000;
		}
	}
}

