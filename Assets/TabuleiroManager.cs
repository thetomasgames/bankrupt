using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que gerencia as casas do tabuleiro, a quem elas pertencem e em qual casa cada player está
/// </summary>
public class TabuleiroManager : MonoBehaviour {

	private List<CasaTabuleiro> casas;
	private Dictionary<Player, int> casaAtualPorPlayer;
	private Dictionary<CasaTabuleiro, Player> casasCompradasPorPlayer;

	private float deslocamentoDentroDaCasa;

	private int numeroInicialPlayers;

	public void IniciaCasas (List<CasaTabuleiro> casas, List<Player> players) {
		this.deslocamentoDentroDaCasa = 1.5f;
		this.numeroInicialPlayers = players.Count;
		this.casas = casas;
		for (var i = 0; i < casas.Count; i++) {
			casas[i].transform.position = getPosicaoCasaPorIndice (i);
		}
		players.ForEach (p => p.transform.position = casas[0].transform.position);
		casasCompradasPorPlayer = new Dictionary<CasaTabuleiro, Player> ();
		casas.ForEach (c => casasCompradasPorPlayer.Add (c, null));
		casaAtualPorPlayer = new Dictionary<Player, int> ();
		foreach (Player p in players) {
			casaAtualPorPlayer.Add (p, 0);
		}
	}

	private Vector3 getPosicaoCasaPorIndice (int indice) {
		int total = casas.Count;
		const int width = 23;
		const int height = 18;
		const float heightOffset = 3f;
		float percentual = (float) indice / total;
		Vector3 posicao;
		float curWidth;
		float curHeight;
		if (percentual < 0.25f) {
			curWidth = width;
			curHeight = (0.25f - percentual) / 0.25f * height;
		} else if (percentual < 0.5f) {
			curWidth = (0.25f - (percentual - 0.25f)) / 0.25f * width;
			curHeight = -heightOffset;
		} else if (percentual < 0.75f) {
			curWidth = 0;
			curHeight = (percentual - 0.5f) / 0.25f * height;
		} else {
			curWidth = (percentual - 0.75f) / 0.25f * width;
			curHeight = height + heightOffset;
		}
		return new Vector3 (curWidth, curHeight, 0) - new Vector3 (width / 2, height / 2, 0);
		//return 15 * new Vector3 (Mathf.Sin (percentual * 2 * Mathf.PI), Mathf.Cos (percentual * 2 * Mathf.PI), 0);
	}

	/// <summary>
	/// Anda o número de casas informado e retorna se uma volta completa no tabuleiro foi atingida
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="numeroCasas">Numero de casas.</param>
	public bool AndarCasasVerificandoVoltaCompleta (Player player, int numeroCasas) {
		casaAtualPorPlayer[player] += numeroCasas;
		if (casaAtualPorPlayer[player] >= casas.Count) {
			casaAtualPorPlayer[player] = casaAtualPorPlayer[player] % casas.Count;
			return true;
		} else {
			return false;
		}

	}

	/// <summary>
	/// Retorna a casa do tabuleiro que o player se encontra
	/// </summary>
	/// <param name="player">Player.</param>
	public CasaTabuleiro GetCasaAtual (Player player) {
		return casas[casaAtualPorPlayer[player]];
	}

	/// <summary>
	/// Retorna o status deu uma casa no tabuleiro (ocupada/disponível)
	/// </summary>
	/// <param name="casa">Casa.</param>
	public StatusCasaTabuleiro GetStatusCasa (CasaTabuleiro casa) {
		return GetDonoDaCasa (casa) == null ? StatusCasaTabuleiro.DISPONIVEL : StatusCasaTabuleiro.OCUPADA;
	}

	/// <summary>
	/// Retorna o dono de uma casa do tabuleiro (ou nulo caso não tenha dono)
	/// </summary>
	/// <returns>The dono da casa.</returns>
	/// <param name="casa">Casa.</param>
	public Player GetDonoDaCasa (CasaTabuleiro casa) {
		return casasCompradasPorPlayer[casa];
	}

	/// <summary>
	/// Assoscia uma casa a um player
	/// </summary>
	/// <param name="player">Player.</param>
	/// <param name="casa">Casa.</param>
	public void ComprarCasa (Player player, CasaTabuleiro casa) {
		casasCompradasPorPlayer[casa] = player;
		casa.atualizaCor (player.GetCor ());
		player.movimentaPlayer (casa.transform.position, () => { });
	}

	/// <summary>
	/// Disponibiliza todas as casas de um player (executada no momento que um player sai do jogo)
	/// </summary>
	/// <param name="player">Player.</param>
	public void DisponibilizaCasas (Player player) {
		List<CasaTabuleiro> casas = new List<CasaTabuleiro> ();
		foreach (KeyValuePair<CasaTabuleiro, Player> casaPlayer in casasCompradasPorPlayer) {
			if (player.Equals (casaPlayer.Value)) {
				casas.Add (casaPlayer.Key);
			}
		}
		casas.ForEach (c => {
			casasCompradasPorPlayer[c] = null;
			c.atualizaCor (Color.white);
		});
		casaAtualPorPlayer.Remove (player);
		Vector3 posicao = getPosicaoCasaPorIndice (0) + Vector3.left * 5;
		posicao += Vector3.left * (numeroInicialPlayers + 1 - casaAtualPorPlayer.Keys.Count) *
			deslocamentoDentroDaCasa;
		player.movimentaPlayer (posicao);
	}

	public void AbreEspacoParaPlayer (CasaTabuleiro casa, Player player) {
		int offset = 1;
		foreach (var kv in casaAtualPorPlayer) {
			if (kv.Key != player && casas[kv.Value] == casa) { // se já tem um player diferente na casa
				kv.Key.movimentaPlayer (casa.transform.position +
					new Vector3 (offset * deslocamentoDentroDaCasa, 0, 0));
				if (offset < 0) {
					offset = -offset + 1;
				} else {
					offset = -offset;
				}
			}
		}
	}

	public int getNumeroPlayersPorCasa (CasaTabuleiro casa) {
		return casas.IndexOf (casa);
	}
}