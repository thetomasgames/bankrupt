using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe representando o banco, mantendo o saldo de cada player e realizando suas movimentações financeiras.
/// </summary>
public class Banco : MonoBehaviour {

	Transform objetosDinheiro;
	public Text textoSaldos;
	private Dictionary<Player, int> contas;

	public void IniciaContas (List<Player> players, int dinheiroInicial) {
		contas = new Dictionary<Player, int> ();
		players.ForEach (p => contas.Add (p, dinheiroInicial));
		atualizaTextoSaldos ();
	}

	public bool TemSaldo (Player player, int quantidade) {
		return GetSaldo (player) >= quantidade;
	}

	public void AdicionaBonusVoltaCompleta (Player player, int valor) {
		AdicionaSaldo (player, valor);
		StartCoroutine (instanciaDinheiro (transform, player.transform, valor));
	}

	private void AdicionaSaldo (Player player, int quantidade) {
		realizaMovimentacao (player, quantidade);
	}

	public int GetSaldo (Player player) {
		return contas[player];
	}

	public void PagaAluguel (Player playerPagante, Player playerBeneficiado, int valorAluguel) {
		removeSaldo (playerPagante, valorAluguel);
		playerPagante.ReageAEvento (TipoEvento.PagouAluguel);

		AdicionaSaldo (playerBeneficiado, valorAluguel);
		playerBeneficiado.ReageAEvento (TipoEvento.RecebeuAluguel);
		StartCoroutine (instanciaDinheiro (playerPagante.transform, playerBeneficiado.transform, valorAluguel));
	}

	private IEnumerator instanciaDinheiro (Transform origemDinheiro, Transform playerBeneficiado, int valor) {
		if (objetosDinheiro == null) {
			objetosDinheiro = new GameObject ("Objetos dinheiro").transform;
		}
		for (int i = 0, items = valor / 30; i < items; i++) {
			yield return new WaitForSeconds (0.1f);
			GameObject dinheiro = GameObject.Instantiate (Resources.Load ("dinheiro"), objetosDinheiro) as GameObject;
			dinheiro.GetComponent<Dinheiro> ().Init (origemDinheiro.position + new Vector3 (1, 1, 0) * 0.1f * i, playerBeneficiado.transform);
		}
		yield return null;
	}

	public void CompraCasa (Player player, CasaTabuleiro casa) {
		removeSaldo (player, casa.valorCompra);
		player.ReageAEvento (TipoEvento.ComprouCasa);
		StartCoroutine (instanciaDinheiro (player.transform, transform, casa.valorCompra));

	}

	public bool TemSaldoNegativo (Player player) {
		return GetSaldo (player) < 0;
	}

	private void realizaMovimentacao (Player player, int valor) {
		contas[player] += valor;
		atualizaTextoSaldos ();
	}

	private void removeSaldo (Player player, int quantidade) {
		realizaMovimentacao (player, -quantidade);
		if (contas[player] < 100) {
			foreach (var p in contas.Keys) {
				p.ReageAEvento (p == player?TipoEvento.FicouComPoucoDinheiro : TipoEvento.OutroPlayerFicouComPoucoDinheiro);
			}
		}
	}

	private void atualizaTextoSaldos () {
		string text = "";
		foreach (var kv in contas) {
			string textoPlayer = kv.Key.ToString ();
			if (kv.Key == GameManager.Instance.GetPlayerAtual ()) {
				textoPlayer = "<b>" + textoPlayer + "</b>";
			}
			text += "<color=#" + ColorUtility.ToHtmlStringRGB (kv.Key.GetCor ()) + ">" + textoPlayer + "</color>: " + kv.Value + "\n";
		}
		textoSaldos.text = text;
	}
}