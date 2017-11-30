using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player aleatorio, que compra caso tenha o saldo disponível com uma chance de 50%.
/// </summary>
public class PlayerAleatorio : Player {

	private System.Random rand;
	private float probabilidadeCompra = 0.5f;

	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta) {
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);
		rand = new System.Random (System.Environment.TickCount);
		this.gameObject.name = this.ToString ();
	}

	public override void DecideComprar (int saldoAtual, CasaTabuleiro casa, Action<bool> then) {
		then (saldoAtual >= casa.valorCompra && rand.NextDouble () <= probabilidadeCompra);
	}

	public override string ToString () {
		return "Sr. Aleatório";

	}

}