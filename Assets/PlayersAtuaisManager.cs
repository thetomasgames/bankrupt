using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe gerencia os turnos de cada player
/// </summary>
public class PlayersAtuaisManager {

	private List<Player> players;
	private int indicePlayerAtual;

	public PlayersAtuaisManager (List<Player> players) {
		this.players = players;
		indicePlayerAtual = 0;
	}

	/// <summary>
	/// Verificação a condição de que só restou um jogador (utilizado para encerrar o jogo)
	/// </summary>
	public bool restaSomenteUm () {
		return this.players.Count == 1;
	}

	/// <summary>
	/// Retorna de quem é a vez de jogar
	/// </summary>
	public Player PassaAVezParaProximo () {
		indicePlayerAtual++;
		if (indicePlayerAtual >= players.Count) {
			indicePlayerAtual = 0;
		}
		Player playerAtual = GetPlayerAtual ();
		playerAtual.ReageAEvento (TipoEvento.SuaVezDeJogar);
		return playerAtual;
	}

	public Player GetPlayerAtual () {
		return players[indicePlayerAtual];
	}

	/// <summary>
	/// Elimina um player da lista de jogadores atuais
	/// </summary>
	/// <param name="player">Player.</param>
	public void EliminaPlayer (Player player) {
		if (indicePlayerAtual > players.IndexOf (player)) {
			indicePlayerAtual--;
		}
		foreach (var p in players) {
			p.ReageAEvento (p == player?TipoEvento.FoiEliminado : TipoEvento.OutroPlayerEliminado);
		}
		this.players.Remove (player);
	}

	/// <summary>
	/// Retorna os players restantes
	/// </summary>
	/// <returns>The vencedores.</returns>
	public List<Player> GetVencedores () {
		return players;
	}

}