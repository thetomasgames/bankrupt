using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe que representa um dado aleatório com uma lista de possibilidades que pode ser rolado.
/// </summary>
public class Dado : MonoBehaviour {

	private float deslocamentoEnquantoRola = 0.5f;

	private int valor = 1;
	private System.Random rand;
	private List<int> opcoes;
	public Text texto;

	public void SetValues (List<int> opcoes) {
		this.opcoes = opcoes;
		rand = new System.Random (System.Environment.TickCount);
	}

	/// <summary>
	/// Retorna uma das opções possíveis com chances equiprovaveis.
	/// </summary>
	public void RolarSimulando (Action<int> next) {
		StartCoroutine (simulaDadoRolando (next));
	}

	public int Rolar () {
		return opcoes[rand.Next (opcoes.Count)];
	}

	IEnumerator simulaDadoRolando (Action<int> next) {
		Vector3 startPos = transform.position;
		for (int i = 0; i < 6; i++) {
			valor = Rolar ();
			texto.text = valor.ToString ();
			transform.position = startPos + new Vector3 ((i % 2 == 0 ? 1 : -1) * deslocamentoEnquantoRola,
				2 * deslocamentoEnquantoRola, 0);
			yield return new WaitForSeconds (0.05f);
		}
		transform.position = startPos;
		yield return new WaitForSeconds (0.2f);
		next (valor);
	}
}