using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe representando uma casa do tabuleiro, que contem o valor de compra e de aluguel.
/// </summary>
public class CasaTabuleiro : MonoBehaviour {
	public int valorCompra;
	public int valorAluguel;

	public Text textoCompra;
	public Text textoAluguel;

	public void Init (int valorCompra, int valorAluguel) {
		this.valorCompra = valorCompra;
		this.valorAluguel = valorAluguel;
		this.textoCompra.text = valorCompra.ToString ();
		this.textoAluguel.text = valorAluguel.ToString ();
	}

	public void atualizaCor (Color cor) {
		GetComponent<SpriteRenderer> ().color = cor;
	}

}