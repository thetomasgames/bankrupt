using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReal : Player {

	Action<bool> then;

	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		this.then = then;
		GameManager.Instance.ApresentaCompraParaPlayer (casa, then);
	}

	public override string ToString () {
		return "Sr. Real";
	}

}