using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Classe que representa um dado aleatório com uma lista de possibilidades que pode ser rolado.
/// </summary>
public class Dado:MonoBehaviour
{
	private System.Random rand;
	private List<int> opcoes;
	public Text texto;

	public void SetValues (List<int> opcoes)
	{
		this.opcoes = opcoes;
		rand = new System.Random (System.Environment.TickCount);
	}

	/// <summary>
	/// Retorna uma das opções possíveis com chances equiprovaveis.
	/// </summary>
	public int Rolar ()
	{
		int valor = opcoes [rand.Next (opcoes.Count)];
		texto.text = valor.ToString ();
		return valor;
	}
}
