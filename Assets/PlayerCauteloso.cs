using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player cauteloso, que compra somente se após a compra ainda restará uma reserva mínima.
/// </summary>
public class PlayerCauteloso : Player {

	private int reservaMinima = 80;

	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta,
		int reservaMinima) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		this.reservaMinima = reservaMinima;

		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		then (saldoAtual - casa.valorCompra >= reservaMinima);
	}

	public override string ToString () {
		return "Sr. Cauteloso";

	}
	protected override string[] GetReacaoPorTipoEvento (TipoEvento tipo) {
		switch (tipo) {
			case TipoEvento.SuaVezDeJogar:
				return new [] { "Vou com calma" };
			case TipoEvento.PagouAluguel:
				return new [] { "Eita" };
			case TipoEvento.RecebeuAluguel:
				return new [] { "Devagar e sempre" };
			case TipoEvento.FicouComPoucoDinheiro:
				return new [] { ":O" };
			case TipoEvento.ComprouCasa:
				return new [] { "Ainda me sobra bastante dinheiro" };
			case TipoEvento.FoiEliminado:
				return new [] { "Droga" };
			case TipoEvento.OutroPlayerEliminado:
				return new [] { "Não teve cautela..." };
			case TipoEvento.OutroPlayerFicouComPoucoDinheiro:
				return new [] { "Jajá ele vai cair" };
			default:
				return new string[] { };
		}
	}

}