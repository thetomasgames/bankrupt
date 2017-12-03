using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaixaDialogo : MonoBehaviour {

	public Text texto;
	public void Init (Vector3 posicao, string text) {
		transform.position = posicao;
		this.texto.text = text;
		Destroy (this.gameObject, 1.5f);
	}
}