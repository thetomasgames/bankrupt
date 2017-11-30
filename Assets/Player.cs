using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player que tem como única ação a decisão de comprar ou não uma propriedade.
/// </summary>
public abstract class Player : MonoBehaviour {

	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		this.banco = banco;
		this.tabuleiroManager = tabuleiroManager;
		this.dado = dado;
		this.valorRecebidoPorVoltaCompleta = valorRecebidoPorVoltaCompleta;
	}

	private Color cor;
	private Banco banco;
	private TabuleiroManager tabuleiroManager;
	private Dado dado;
	private int valorRecebidoPorVoltaCompleta;

	public abstract void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then);

	public void ExecutarJogada () {
		dado.RolarSimulando ((numeroFaceDado) => {
			bool completouVolta = tabuleiroManager.AndarCasasVerificandoVoltaCompleta (this, numeroFaceDado);
			if (completouVolta) {
				banco.AdicionaSaldo (this, valorRecebidoPorVoltaCompleta);
			}
			CasaTabuleiro casaAtual = tabuleiroManager.GetCasaAtual (this);
			StartCoroutine (movimentaPlayer (casaAtual.transform.position));
			return 0;
		});

	}

	private void decideComprar () {
		CasaTabuleiro casaAtual = tabuleiroManager.GetCasaAtual (this);
		switch (tabuleiroManager.GetStatusCasa (casaAtual)) {
			case StatusCasaTabuleiro.DISPONIVEL:
				DecideComprar (banco.GetSaldo (this), casaAtual, (decisao) => {
					if (decisao) {
						compraCasa (this, casaAtual);
					}
					finalizaJogada ();
				});
				break;
			case StatusCasaTabuleiro.OCUPADA:
				Player dono = tabuleiroManager.GetDonoDaCasa (casaAtual);
				if (!this.Equals (dono)) {
					banco.PagaAluguel (this, dono, casaAtual.valorAluguel);
				}
				finalizaJogada ();
				break;
		}
	}

	private void finalizaJogada () {
		GameManager.Instance.ExecutaJogada ();
	}

	IEnumerator movimentaPlayer (Vector3 target) {
		Vector3 startPos = transform.position;
		for (float i = 0; i < 1; i += 0.05f) {
			yield return new WaitForSeconds (0.01f);
			transform.position = Vector3.Lerp (startPos, target, i);
		}
		yield return new WaitForSeconds (0.5f);
		decideComprar ();
		yield return null;
	}

	private void compraCasa (Player player, CasaTabuleiro casa) {
		tabuleiroManager.ComprarCasa (player, casa);
		banco.CompraCasa (player, casa);
	}

	public void SetCor (Color cor) {
		GetComponent<SpriteRenderer> ().color = cor;
		this.cor = cor;
	}

	public Color GetCor () {
		return this.cor;
	}

}