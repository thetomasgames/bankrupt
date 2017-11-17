using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Player que tem como única ação a decisão de comprar ou não uma propriedade.
/// </summary>
public abstract class Player : MonoBehaviour
{

	public  void SetValores (Banco banco, TabuleiroManager tabuleiroManager, Dado dado, int valorRecebidoPorVoltaCompleta)
	{
		this.banco = banco;
		this.tabuleiroManager = tabuleiroManager;
		this.dado = dado;
		this.valorRecebidoPorVoltaCompleta = valorRecebidoPorVoltaCompleta;
	}

	private Banco banco;
	private TabuleiroManager tabuleiroManager;
	private Dado dado;
	private int valorRecebidoPorVoltaCompleta;

	public abstract bool DecideComprar (int saldoAtual, CasaTabuleiro casa);

	public void ExecutarJogada ()
	{
		int numeroFaceDado = dado.Rolar ();
		print ("tirou " + numeroFaceDado);
		bool completouVolta = tabuleiroManager.AndarCasasVerificandoVoltaCompleta (this, numeroFaceDado);
		if (completouVolta) {
			banco.AdicionaSaldo (this, valorRecebidoPorVoltaCompleta);
		}
		CasaTabuleiro casaAtual = tabuleiroManager.GetCasaAtual (this);
		switch (tabuleiroManager.GetStatusCasa (casaAtual)) {
		case StatusCasaTabuleiro.DISPONIVEL:
			if (DecideComprar (banco.GetSaldo (this), casaAtual)) {
				compraCasa (this, casaAtual);
			}
			break;
		case StatusCasaTabuleiro.OCUPADA:
			Player dono = tabuleiroManager.GetDonoDaCasa (casaAtual);
			if (!this.Equals (dono)) {
				banco.PagaAluguel (this, dono, casaAtual.valorAluguel);
			}
			break;
		}

	}

	private void compraCasa (Player player, CasaTabuleiro casa)
	{
		tabuleiroManager.ComprarCasa (player, casa);
		banco.CompraCasa (player, casa);
	}
		
		
}
