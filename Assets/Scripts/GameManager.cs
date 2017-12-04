using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	public static GameManager Instance { get { return _instance; } }

	private void Awake () {
		if (_instance != null && _instance != this) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
		}
	}

	private Transform caixasDialogo;
	public CompraCasa compraCasa;

	public Banco banco;
	private TabuleiroManager tabuleiroManager;
	public Dado dado;
	private int valorRecebidoPorVoltaCompleta;

	private PlayersAtuaisManager playersAtuaisManager;
	private int numeroMaximoRodadas;

	public int rodada;

	public bool jogoIniciado;

	public Button botaoIniciaJogo;

	void Start () {
		jogoIniciado = false;
		ConfiguracoesJogo configuracoes = new ConfiguracoesJogo ();
		configuracoes.casasTabuleiro = obtemCasasTabuleiroDoArquivo ("gameConfig.txt");
		rodada = 0;
		dado.SetValues (configuracoes.opcoesDado);
		valorRecebidoPorVoltaCompleta = configuracoes.valorRecebidoPorVoltaCompleta;
		GameObject tabuleiroGo = new GameObject ();
		tabuleiroGo.AddComponent<TabuleiroManager> ();
		tabuleiroManager = tabuleiroGo.GetComponent<TabuleiroManager> ();
		List<Player> listaplayers = new List<Player> {
			PlayerFactory.criaPlayerImpulsivo (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta),
			PlayerFactory.criaPlayerExigente (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta, 50),
			PlayerFactory.criaPlayerCauteloso (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta, 80),
			PlayerFactory.criaPlayerAleatorio (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta),
			PlayerFactory.criaPlayerReal (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta)
		};
		for (var i = 0; i < listaplayers.Count; i++) {
			listaplayers[i].SetCor (Color.HSVToRGB ((float) i / listaplayers.Count, 1, 1));
		}

		playersAtuaisManager = new PlayersAtuaisManager (decideOrdemDePlayers (listaplayers));
		tabuleiroManager.IniciaCasas (configuracoes.casasTabuleiro, listaplayers);
		banco.IniciaContas (listaplayers, configuracoes.dinheiroInicial);
		numeroMaximoRodadas = configuracoes.numeroMaximoRodadas;
	}

	public Player GetPlayerAtual () {
		return playersAtuaisManager.GetPlayerAtual ();
	}

	/// <summary>
	/// Retorna o vencedor, levando em conta e caso de empate os critérios: 
	/// 1: Saldo no banco
	/// 2: Vez mais próxima no término da rodada
	/// </summary>
	/// <returns>The vencedor.</returns>
	public Player GetVencedor () {
		List<Player> vencedores = playersAtuaisManager.GetVencedores ();
		if (vencedores.Count > 1) {
			vencedores.Sort ((v1, v2) => banco.GetSaldo (v2).CompareTo (banco.GetSaldo (v1)));
			if (banco.GetSaldo (vencedores[0]) == banco.GetSaldo (vencedores[1])) {
				Player proximoPlayer;
				do {
					proximoPlayer = playersAtuaisManager.PassaAVezParaProximo ();
				} while (!(proximoPlayer.Equals (vencedores[0]) || proximoPlayer.Equals (vencedores[1])));
				return proximoPlayer;
			}
		}
		return vencedores[0];
	}

	/// <summary>
	/// Condição de parada para que o jogo se declare encerrado (
	/// </summary>
	/// <param name="rodada">Rodada.</param>
	private bool deveEncerrarJogo () {
		return playersAtuaisManager.restaSomenteUm () || rodada >= numeroMaximoRodadas;
	}

	public void IniciaJogo () {
		botaoIniciaJogo.gameObject.active = false;
		jogoIniciado = true;
		ExecutaJogada ();
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
	private void ExecutaJogada () {
		rodada++;
		Player playerAtual = GetPlayerAtual ();
		playerAtual.ExecutarJogada ();
	}

	private void eliminaPlayer (Player player) {
		tabuleiroManager.DisponibilizaCasas (player);
		playersAtuaisManager.EliminaPlayer (player);
	}

	/// <summary>
	/// Utiliza o lançamento de dados de cada player para decidir a ordem de jogo
	/// </summary>
	/// <param name="players">Players.</param>
	private List<Player> decideOrdemDePlayers (List<Player> players) {
		List<Player> melhoresPlayers = new List<Player> ();
		List<Player> playersRestantes = new List<Player> (players);
		while (playersRestantes.Count > 0) {
			Player melhor = selecionaMelhorPlayersNoDado (playersRestantes);
			playersRestantes.Remove (melhor);
			melhoresPlayers.Add (melhor);
		}
		return melhoresPlayers;
	}

	public void NotificaFimJogada () {
		Player playerAtual = GetPlayerAtual ();
		if (banco.TemSaldoNegativo (playerAtual)) {
			eliminaPlayer (playerAtual);
		}
		if (!deveEncerrarJogo ()) {
			playersAtuaisManager.PassaAVezParaProximo ();
			ExecutaJogada ();
		} else {
			print ("ganhou");
		}
	}

	/// <summary>
	/// Função recursiva que em caso de empate nos dados chama a si própria com os players empatados
	/// </summary>
	/// <param name="players">Players.</param>
	private Player selecionaMelhorPlayersNoDado (List<Player> players) {
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
			return melhoresPlayers[0];
		}
	}

	private List<CasaTabuleiro> obtemCasasTabuleiroDoArquivo (string nome) {
		List<CasaTabuleiro> casasLidas = new List<CasaTabuleiro> ();
		GameObject parent = new GameObject ("Casas Tabuleiro");
		string[] linhas = System.IO.File.ReadAllLines (@nome);
		for (int i = 0; i < linhas.Length - 1; i++) {
			string[] valores = linhas[i].Split (new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries);
			string valorCompra = valores[0].Trim ();
			string valorAluguel = valores[1].Trim ();
			GameObject go = GameObject.Instantiate (Resources.Load ("CasaTabuleiro"), parent.transform) as GameObject;
			go.name = "Casa " + i + "(" + valorCompra + "," + valorAluguel + ")";
			CasaTabuleiro casa = go.GetComponent<CasaTabuleiro> ();
			casa.Init (int.Parse (valorCompra), int.Parse (valorAluguel));
			casasLidas.Add (casa);
		}
		return casasLidas;
	}

	public void ApresentaCompraParaPlayer (CasaTabuleiro casa, Action<bool> then) {
		compraCasa.ApresentaCompraParaPlayer (casa, then);
	}

	public void CriaCaixaDialogo (Transform posicao, string texto) {
		if (caixasDialogo == null) {
			caixasDialogo = new GameObject ("Caixas dialogo").transform;
		}
		GameObject go = GameObject.Instantiate (Resources.Load ("caixaDialogo"), posicao) as GameObject;
		go.GetComponent<CaixaDialogo> ().Init (texto);
	}

}