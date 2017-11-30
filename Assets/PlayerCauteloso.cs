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

}