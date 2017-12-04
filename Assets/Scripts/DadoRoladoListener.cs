using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DadoRoladoListener : MonoBehaviour
{

	public Text texto;

	public void NotificaDadoRolado (int numero)
	{
		texto.text = numero.ToString ();

		//System.Threading.Thread.Sleep (1000);
	}
}
