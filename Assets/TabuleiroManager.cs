using System;
using System.Collections.Generic;

namespace Bankrupt
{
	/// <summary>
	/// Classe que gerencia as casas do tabuleiro, a quem elas pertencem e em qual casa cada player está
	/// </summary>
	public class TabuleiroManager
	{

		private List<CasaTabuleiro> casas;
		private Dictionary<Player,int> casaAtualPorPlayer;
		private Dictionary<CasaTabuleiro,Player> casasCompradasPorPlayer;

		public TabuleiroManager (List<CasaTabuleiro> casas, List<Player> players)
		{
			this.casas = casas;
			casasCompradasPorPlayer = new Dictionary<CasaTabuleiro, Player> ();
			casas.ForEach (c => casasCompradasPorPlayer.Add (c, null));
			casaAtualPorPlayer = new Dictionary<Player, int> ();
			players.ForEach (p => casaAtualPorPlayer.Add (p, 0));
		}

		/// <summary>
		/// Anda o número de casas informado e retorna se uma volta completa no tabuleiro foi atingida
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="numeroCasas">Numero de casas.</param>
		public bool AndarCasasVerificandoVoltaCompleta (Player player, int numeroCasas)
		{
			casaAtualPorPlayer [player] += numeroCasas;
			if (casaAtualPorPlayer [player] >= casas.Count) {
				casaAtualPorPlayer [player] = 0;
				return true;
			} else {
				return false;
			}

		}

		/// <summary>
		/// Retorna a casa do tabuleiro que o player se encontra
		/// </summary>
		/// <param name="player">Player.</param>
		public CasaTabuleiro GetCasaAtual (Player player)
		{
			return casas [casaAtualPorPlayer [player]];
		}

		/// <summary>
		/// Retorna o status deu uma casa no tabuleiro (ocupada/disponível)
		/// </summary>
		/// <param name="casa">Casa.</param>
		public StatusCasaTabuleiro GetStatusCasa (CasaTabuleiro casa)
		{
			return GetDonoDaCasa (casa) == null ? StatusCasaTabuleiro.DISPONIVEL : StatusCasaTabuleiro.OCUPADA;
		}

		/// <summary>
		/// Retorna o dono de uma casa do tabuleiro (ou nulo caso não tenha dono)
		/// </summary>
		/// <returns>The dono da casa.</returns>
		/// <param name="casa">Casa.</param>
		public Player GetDonoDaCasa (CasaTabuleiro casa)
		{
			return casasCompradasPorPlayer [casa];
		}

		/// <summary>
		/// Assoscia uma casa a um player
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="casa">Casa.</param>
		public void ComprarCasa (Player player, CasaTabuleiro casa)
		{
			casasCompradasPorPlayer [casa] = player;
		}

		/// <summary>
		/// Disponibiliza todas as casas de um player (executada no momento que um player sai do jogo)
		/// </summary>
		/// <param name="player">Player.</param>
		public void DisponibilizaCasas (Player player)
		{
			List<CasaTabuleiro> casas = new List<CasaTabuleiro> ();
			foreach (KeyValuePair<CasaTabuleiro,Player> casaPlayer in casasCompradasPorPlayer) {
				if (player.Equals (casaPlayer.Value)) {
					casas.Add (casaPlayer.Key);
				}
			}
			casas.ForEach (c => casasCompradasPorPlayer [c] = null);
		}
	}
}

