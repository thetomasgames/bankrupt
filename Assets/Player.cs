using System;

namespace Bankrupt
{
	/// <summary>
	/// Player que tem como única ação a decisão de comprar ou não uma propriedade.
	/// </summary>
	public interface Player
	{
		bool DecideComprar (int saldoAtual, CasaTabuleiro casa);
	}
}

