using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICasaTabueiro : MonoBehaviour
{

	public Text textoCompra;
	public Text textoAluguel;

	public void Init (CasaTabuleiro casa)
	{
		this.textoCompra.text = casa.valorCompra.ToString ();
		this.textoAluguel.text = casa.valorAluguel.ToString ();

		GetComponent<SpriteRenderer> ().color = Color.HSVToRGB ((float)casa.valorCompra / 450, 1, 1);
	}

}
