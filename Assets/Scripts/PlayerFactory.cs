using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory {

	private static GameObject parent = new GameObject ("Players");

	private static GameObject criaGameObject () {
		return GameObject.Instantiate (Resources.Load ("Player"), PlayerFactory.parent.transform) as GameObject;
	}

	public static Player criaPlayerImpulsivo (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		GameObject go = criaGameObject ();
		PlayerImpulsivo player = go.AddComponent<PlayerImpulsivo> ();
		player.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		return player;
	}

	public static Player criaPlayerExigente (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta, int valorAluguel) {
		GameObject go = criaGameObject ();
		PlayerExigente player = go.AddComponent<PlayerExigente> ();
		player.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta, valorAluguel);
		return player;
	}

	public static Player criaPlayerCauteloso (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta, int valorReserva) {
		GameObject go = criaGameObject ();
		PlayerCauteloso player = go.AddComponent<PlayerCauteloso> ();
		player.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta, valorReserva);
		return player;

	}

	public static Player criaPlayerAleatorio (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		GameObject go = criaGameObject ();
		PlayerAleatorio player = go.AddComponent<PlayerAleatorio> ();
		player.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		return player;

	}
	public static Player criaPlayerReal (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		GameObject go = criaGameObject ();
		PlayerReal player = go.AddComponent<PlayerReal> ();
		player.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		return player;

	}
}