using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player aleatorio, que compra caso tenha o saldo disponível com uma chance de 50%.
/// </summary>
public class PlayerAleatorio : Player {

	private float probabilidadeCompra = 0.5f;

	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		then (saldoAtual >= casa.valorCompra && rand.NextDouble () <= probabilidadeCompra);
	}

	public override string ToString () {
		return "Sr. Aleatório";

	}

	protected override string[] GetReacaoPorTipoEvento (TipoEvento tipo) {
		switch (tipo) {
			case TipoEvento.SuaVezDeJogar:
				return new [] { "Agora vai" };
			case TipoEvento.PagouAluguel:
				return new [] { "Oh não..." };
			case TipoEvento.RecebeuAluguel:
				return new [] { "Ihhul" };
			case TipoEvento.FicouComPoucoDinheiro:
				return new [] { ":|" };
			case TipoEvento.ComprouCasa:
				return new [] { "Mais uma..." };
			case TipoEvento.FoiEliminado:
				return new [] { "Eu nem gosto desse jogo mesmo" };
			case TipoEvento.OutroPlayerEliminado:
				return new [] { "Antes ele que eu" };
			case TipoEvento.OutroPlayerFicouComPoucoDinheiro:
				return new [] { "Respira por aparelhos" };
			default:
				return new string[] { };
		}
	}

}