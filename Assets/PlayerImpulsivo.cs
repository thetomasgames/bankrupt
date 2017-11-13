using System;

namespace Bankrupt
{
	/// <summary>
	/// Player impulsivo que compra sempre que tem o valor disponível.
	/// </summary>
	public class PlayerImpulsivo: Player
	{
		public PlayerImpulsivo ()
		{
		}

		public bool DecideComprar (int saldoAtual, CasaTabuleiro casa)
		{
			return saldoAtual >= casa.valorCompra;
		}

		public override string ToString ()
		{
			return  "Sr. Impulsivo";

		}
   
	}
}

