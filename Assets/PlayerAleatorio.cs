using System;

namespace Bankrupt
{
	/// <summary>
	/// Player aleatorio, que compra caso tenha o saldo disponível com uma chance de 50%.
	/// </summary>
	public class PlayerAleatorio : Player
	{
		private Random rand;
		private float probabilidadeCompra = 0.5f;

		public PlayerAleatorio ()
		{
			rand = new Random (System.Environment.TickCount);
		}

		public bool DecideComprar (int saldoAtual, CasaTabuleiro casa)
		{
			return saldoAtual >= casa.valorCompra && rand.NextDouble () <= probabilidadeCompra;
		}

		public override string ToString ()
		{
			return  "Sr. Aleatório";

		}
   
	}
}

