using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReal : Player {

	protected Dictionary<TipoEvento, List<String>> reacoesPorTipoEvento;
	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		if (saldoAtual >= casa.valorCompra) {
			GameManager.Instance.ApresentaCompraParaPlayer (casa, then);
		} else {
			then (false);
		}
	}

	public override string ToString () {
		return "Sr. Real";
	}

	protected override string[] GetReacaoPorTipoEvento (TipoEvento tipo) {
		switch (tipo) {
			case TipoEvento.SuaVezDeJogar:
				return new [] { "Vamos lá" };
			case TipoEvento.PagouAluguel:
				return new [] { "Que droga" };
			case TipoEvento.RecebeuAluguel:
				return new [] { "Isso!" };
			case TipoEvento.FicouComPoucoDinheiro:
				return new [] { "Ah não..." };
			case TipoEvento.ComprouCasa:
				return new [] { "Essa valia a pena" };
			case TipoEvento.FoiEliminado:
				return new [] { "Fica pra próxima..." };
			case TipoEvento.OutroPlayerEliminado:
				return new [] { "Agora tenho mais chances" };
			case TipoEvento.OutroPlayerFicouComPoucoDinheiro:
				return new [] { "Vai ser eliminado" };
			default:
				return new string[] { };
		}
	}
}