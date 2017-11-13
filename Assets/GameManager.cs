using System;
using System.Collections.Generic;

namespace Bankrupt
{
	public class GameManager
	{
		private Banco banco;
		private PlayersAtuaisManager playersAtuaisManager;
		private TabuleiroManager tabuleiroManager;
		private Dado dado;
		private int valorRecebidoPorVoltaCompleta;
		private int numeroMaximoRodadas;

		public int rodada;

		private List<FimJogadaListener> fimJogadaListener = new List<FimJogadaListener> ();
		private List<DadoRoladoListener> dadoRoladoListener = new List<DadoRoladoListener> ();

		public GameManager (ConfiguracoesJogo configuracoes)
		{
			rodada = 0;
			dado = new Dado (configuracoes.opcoesDado);
			playersAtuaisManager = new	PlayersAtuaisManager (decideOrdemDePlayers (configuracoes.listaplayers));
			tabuleiroManager = new	TabuleiroManager (configuracoes.casasTabuleiro, configuracoes.listaplayers);
			banco = new Banco (configuracoes.listaplayers, configuracoes.dinheiroInicial);
			valorRecebidoPorVoltaCompleta = configuracoes.valorRecebidoPorVoltaCompleta;
			numeroMaximoRodadas = configuracoes.numeroMaximoRodadas;
		}

		/// <summary>
		/// Retorna o vencedor, levando em conta e caso de empate os critérios: 
		/// 1: Saldo no banco
		/// 2: Vez mais próxima no término da rodada
		/// </summary>
		/// <returns>The vencedor.</returns>
		public Player GetVencedor ()
		{
			List<Player> vencedores = playersAtuaisManager.GetVencedores ();
			if (vencedores.Count > 1) {
				vencedores.Sort ((v1, v2) => banco.GetSaldo (v2).CompareTo (banco.GetSaldo (v1)));
				if (banco.GetSaldo (vencedores [0]) == banco.GetSaldo (vencedores [1])) {
					Player proximoPlayer;
					do {
						proximoPlayer = playersAtuaisManager.GetProximoPlayer ();
					} while(!(proximoPlayer.Equals (vencedores [0]) || proximoPlayer.Equals (vencedores [1])));
					return proximoPlayer;
				} 
			}
			return vencedores [0];
		}

		/// <summary>
		/// Executa a próxima jogada, rodada a rodada até que a condição de parada seja antedida
		/// </summary>
		public void ExecutaJogoAteOFim (Action func)
		{
			while (!deveEncerrarJogo ()) {
				rodada++;
				func ();
				ExecutaJogada ();
			}
		}

		/// <summary>
		/// Condição de parada para que o jogo se declare encerrado (
		/// </summary>
		/// <param name="rodada">Rodada.</param>
		private bool deveEncerrarJogo ()
		{
			return playersAtuaisManager.restaSomenteUm () || rodada >= numeroMaximoRodadas;
		}

		/// <summary>
		/// Em uma jogada ocorre:
		/// 1: O player do qual é a vez é selecionado
		/// 2: O player joga o dado para descobrir quantas casas deve andar
		/// 3: Caso tenha completado uma volta ele é bonificado com o valor estipulado
		/// 3: Caso a casa em que ele parou tem dono ele paga seu aluguel
		/// 4: Caso a casa em que ele parou não tenha dono ele decide de deve compra-la
		/// 4: Caso ele tenha saldo negativo ele é eliminado do jogo
		/// 
		/// </summary>
		public void ExecutaJogada ()
		{
			Player playerAtual = playersAtuaisManager.GetProximoPlayer ();
			int numeroFaceDado = dado.Rolar ();
			dadoRoladoListener.ForEach (l => l.NotificaDadoRolado (numeroFaceDado));
			bool completouVolta = tabuleiroManager.AndarCasasVerificandoVoltaCompleta (playerAtual, numeroFaceDado);
			if (completouVolta) {
				banco.AdicionaSaldo (playerAtual, valorRecebidoPorVoltaCompleta);
			}
			CasaTabuleiro casaAtual = tabuleiroManager.GetCasaAtual (playerAtual);
			switch (tabuleiroManager.GetStatusCasa (casaAtual)) {
			case StatusCasaTabuleiro.DISPONIVEL:
				if (playerAtual.DecideComprar (banco.GetSaldo (playerAtual), casaAtual)) {
					compraCasa (playerAtual, casaAtual);
				}
				break;
			case StatusCasaTabuleiro.OCUPADA:
				Player dono = tabuleiroManager.GetDonoDaCasa (casaAtual);
				if (!playerAtual.Equals (dono)) {
					banco.PagaAluguel (playerAtual, dono, casaAtual.valorAluguel);
				}
				break;
			}
			if (banco.TemSaldoNegativo (playerAtual)) {
				eliminaPlayer (playerAtual);
			}
			fimJogadaListener.ForEach (l => l.NotificaFimJogada (banco, playerAtual, tabuleiroManager));

		}

		private void eliminaPlayer (Player player)
		{
			tabuleiroManager.DisponibilizaCasas (player);
			playersAtuaisManager.EliminaPlayer (player);
		}

		private void compraCasa (Player player, CasaTabuleiro casa)
		{
			tabuleiroManager.ComprarCasa (player, casa);
			banco.CompraCasa (player, casa);
		}

		/// <summary>
		/// Utiliza o lançamento de dados de cada player para decidir a ordem de jogo
		/// </summary>
		/// <param name="players">Players.</param>
		private List<Player> decideOrdemDePlayers (List<Player> players)
		{
			List<Player> melhoresPlayers = new List<Player> ();
			List<Player> playersRestantes = new List<Player> (players);
			while (playersRestantes.Count > 0) {
				Player melhor = selecionaMelhorPlayersNoDado (playersRestantes);
				playersRestantes.Remove (melhor);
				melhoresPlayers.Add (melhor);
			}
			return melhoresPlayers;
		}

		/// <summary>
		/// Função recursiva que em caso de empate nos dados chama a si própria com os players empatados
		/// </summary>
		/// <param name="players">Players.</param>
		private Player selecionaMelhorPlayersNoDado (List<Player> players)
		{
			List<Player> melhoresPlayers = new List<Player> ();
			int maiorNumero = 0;
			players.ForEach (p => {
				int valorDado = dado.Rolar ();
				if (valorDado == maiorNumero) {
					melhoresPlayers.Add (p);
				} else if (valorDado > maiorNumero) {
					melhoresPlayers = new List<Player> ();
					melhoresPlayers.Add (p);
				}
			});

			if (melhoresPlayers.Count != 1) {
				return selecionaMelhorPlayersNoDado (melhoresPlayers);
			} else {
				return melhoresPlayers [0];
			}
		}

		public void AddFimJogadaListener (FimJogadaListener listener)
		{
			this.fimJogadaListener.Add (listener);
		}

		public void AddDadoRoladoListener (DadoRoladoListener listener)
		{
			this.dadoRoladoListener.Add (listener);
		}

	}
}