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
}