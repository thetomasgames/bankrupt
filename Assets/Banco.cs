using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe representando o banco, mantendo o saldo de cada player e realizando suas movimentações financeiras.
/// </summary>
public class Banco:MonoBehaviour
{
	private Dictionary<Player,int> contas;

	public Banco ()
	{

	}

	public void IniciaContas (List<Player> players, int dinheiroInicial)
	{
		contas = new Dictionary<Player, int> ();
		players.ForEach (p => contas.Add (p, dinheiroInicial));
	}

	public bool TemSaldo (Player player, int quantidade)
	{
		return GetSaldo (player) >= quantidade;
	}


	public void AdicionaSaldo (Player player, int quantidade)
	{
		realizaMovimentacao (player, quantidade);
	}

	public int GetSaldo (Player player)
	{
		return contas [player];
	}

	public void PagaAluguel (Player playerPagante, Player playerBeneficiado, int valorAluguel)
	{
		removeSaldo (playerPagante, valorAluguel);
		AdicionaSaldo (playerBeneficiado, valorAluguel);
	}

	public void CompraCasa (Player player, CasaTabuleiro casa)
	{
		removeSaldo (player, casa.valorCompra);
	}

	public bool TemSaldoNegativo (Player player)
	{
		return GetSaldo (player) < 0;
	}

	private void realizaMovimentacao (Player player, int valor)
	{
		contas [player] += valor;
	}

	private void removeSaldo (Player player, int quantidade)
	{
		realizaMovimentacao (player, -quantidade);
	}
}
