using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player impulsivo que compra sempre que tem o valor disponível.
/// </summary>
public class PlayerImpulsivo: Player
{
	public void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta)
	{
		base.SetValores (banco, tabuleiroManager, dado, valorRecebidoPorVoltaCompleta);

		this.gameObject.name = this.ToString ();
	}

	public override bool DecideComprar (int saldoAtual, CasaTabuleiro casa)
	{
		return saldoAtual >= casa.valorCompra;
	}

	public override string ToString ()
	{
		return  "Sr. Impulsivo";

	}
			
   
}


