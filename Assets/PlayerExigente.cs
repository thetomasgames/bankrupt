using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player exigente que compra somente se o valor aluguel é maior que o mínimo estabelecido.
/// </summary>
public class PlayerExigente : Player {
	private int valorMinimoAluguel;

	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta,
		int valorMinimoAluguel) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		this.valorMinimoAluguel = valorMinimoAluguel;
		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		then (saldoAtual > casa.valorCompra && casa.valorAluguel >= valorMinimoAluguel);
	}

	public override string ToString () {
		return "Sr. Exigente";
	}

	protected override string[] GetReacaoPorTipoEvento (TipoEvento tipo) {
		switch (tipo) {
			case TipoEvento.SuaVezDeJogar:
				return new [] { "Minha vez!" };
			case TipoEvento.PagouAluguel:
				return new [] { "Dinheiro saindo..." };
			case TipoEvento.RecebeuAluguel:
				return new [] { "Menos de " + valorMinimoAluguel + " eu nem quero" };
			case TipoEvento.FicouComPoucoDinheiro:
				return new [] { "Vou me recuperar ainda" };
			case TipoEvento.ComprouCasa:
				return new [] { "Essa vai render bastante" };
			case TipoEvento.FoiEliminado:
				return new [] { "Estava indo bem :(" };
			case TipoEvento.OutroPlayerEliminado:
				return new [] { "Menos gente pra me render dinheiro..." };
			case TipoEvento.OutroPlayerFicouComPoucoDinheiro:
				return new [] { "Espero que se recupere" };
			default:
				return new string[] { };
		}
	}
}