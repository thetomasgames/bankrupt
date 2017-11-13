using System;

namespace Bankrupt
{
	/// <summary>
	/// Player cauteloso, que compra somente se após a compra ainda restará uma reserva mínima.
	/// </summary>
	public class PlayerCauteloso : Player
	{

		private int reservaMinima = 80;

		public PlayerCauteloso ()
		{
		}

		public bool DecideComprar (int saldoAtual, CasaTabuleiro casa)
		{
			return saldoAtual - casa.valorCompra >= reservaMinima;
		}

		public override string ToString ()
		{
			return  "Sr. Cauteloso";

		}
   
	}
}

