using System;

namespace Bankrupt
{
	/// <summary>
	/// Player exigente que compra somente se o valor aluguel é maior que o mínimo estabelecido.
	/// </summary>
	public class PlayerExigente: Player
	{

		private int valorMinimoAluguel = 50;

		public PlayerExigente ()
		{
		}

		public bool DecideComprar (int saldoAtual, CasaTabuleiro casa)
		{
			return saldoAtual > casa.valorCompra && casa.valorAluguel >= valorMinimoAluguel;
		}

		public override string ToString ()
		{
			return  "Sr. Exigente";
		}
	}
}

