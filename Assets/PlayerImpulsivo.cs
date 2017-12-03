using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player impulsivo que compra sempre que tem o valor disponível.
/// </summary>
public class PlayerImpulsivo : Player {
	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);

		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		then (saldoAtual >= casa.valorCompra);
	}

	public override string ToString () {
		return "Sr. Impulsivo";

	}

	protected override string[] GetReacaoPorTipoEvento (TipoEvento tipo) {
		switch (tipo) {
			case TipoEvento.SuaVezDeJogar:
				return new [] { "ATÉ QUE ENFIM" };
			case TipoEvento.PagouAluguel:
				return new [] { "AH NÃO" };
			case TipoEvento.RecebeuAluguel:
				return new [] { "EBAAAAA" };
			case TipoEvento.FicouComPoucoDinheiro:
				return new [] { "AINDA DÁ PRA GANHAR" };
			case TipoEvento.ComprouCasa:
				return new [] { "EU COMPRO MESMO" };
			case TipoEvento.FoiEliminado:
				return new [] { "NAAAAAAAAO" };
			case TipoEvento.OutroPlayerEliminado:
				return new [] { "FALTAM POUCOS" };
			case TipoEvento.OutroPlayerFicouComPoucoDinheiro:
				return new [] { "A HORA DELE TA CHEGANDO" };
			default:
				return new string[] { };
		}
	}
}