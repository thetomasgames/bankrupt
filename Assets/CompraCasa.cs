using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompraCasa : MonoBehaviour {

	public Text texto;
	public Canvas canvas;

	private Action<bool> then;

	public void ApresentaCompraParaPlayer (CasaTabuleiro casa, Action<bool> then) {
		this.texto.text = "Comprar " + casa.ToString () + "?";
		this.then = then;
		this.canvas.enabled = true;
	}

	public void DecideCompra (bool decisao) {
		then (decisao);
		this.canvas.enabled = false;
	}
}