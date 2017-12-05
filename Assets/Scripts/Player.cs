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
		this.rand = new System.Random (System.Environment.TickCount);
	}

	protected System.Random rand;

	private Color cor;
	private Banco banco;
	private TabuleiroManager tabuleiroManager;
	private Dado dado;
	private int valorRecebidoPorVoltaCompleta;
	protected abstract string[] GetReacaoPorTipoEvento (TipoEvento tipo);

	public abstract void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then);

	public void ExecutarJogada () {
		dado.RolarSimulando ((numeroFaceDado) => {
			List<CasaTabuleiro> casasPassadas;
			bool completouVolta;
			tabuleiroManager.AndarCasasVerificandoVoltaCompleta (this, numeroFaceDado,
				out completouVolta, out casasPassadas);
			if (completouVolta) {
				banco.AdicionaBonusVoltaCompleta (this, valorRecebidoPorVoltaCompleta);
			}
			CasaTabuleiro casaAtual = tabuleiroManager.GetCasaAtual (this);
			tabuleiroManager.GetDonoDaCasa (casaAtual);
			tabuleiroManager.AbreEspacoParaPlayer (casaAtual, this);
			StartCoroutine (movimentaPlayerCasaACasa (casasPassadas, decideComprar));
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
		GameManager.Instance.NotificaFimJogada ();
	}

	public void movimentaPlayer (Vector3 target) {
		StartCoroutine (movimentaPlayerEnumerator (target));
	}
	private IEnumerator movimentaPlayerEnumerator (Vector3 target) {
		Vector3 startPos = transform.position;
		for (float i = 0; i <= 1; i += 0.05f) {
			yield return new WaitForSeconds (0.01f);
			transform.position = Vector3.Lerp (startPos, target, i);
		}
		transform.position = target;
	}

	private IEnumerator movimentaPlayerCasaACasa (List<CasaTabuleiro> casas, Action then) {
		for (int casa = 0; casa < casas.Count; casa++) {
			Vector3 startPos = transform.position;
			for (float i = 0; i <= 1; i += 0.15f) {
				yield return new WaitForSeconds (0.01f);
				transform.position = Vector3.Lerp (startPos, casas[casa].transform.position, i);
			}
			tabuleiroManager.AbreEspacoParaPlayer (casas[casa], this);
			transform.position = casas[casa].transform.position;
			yield return new WaitForSeconds (0.25f);
		}
		then ();
		yield return new WaitForSeconds (0.25f);
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

	public void ReageAEvento (TipoEvento tipo) {
		string[] textos = GetReacaoPorTipoEvento (tipo);
		if (textos.Length > 0 && rand.NextDouble () < 0.3f &&
			tabuleiroManager.GetCasaAtual (this) != null &&
			tabuleiroManager.getNumeroPlayersPorCasa (tabuleiroManager.GetCasaAtual (this)) == 1) {
			GameManager.Instance.CriaCaixaDialogo (transform, textos[rand.Next (textos.Length - 1)]);
		}
	}

}