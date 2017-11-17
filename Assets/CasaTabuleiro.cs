﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe representando uma casa do tabuleiro, que contem o valor de compra e de aluguel.
/// </summary>
public class CasaTabuleiro
{
	public int valorCompra;
	public int valorAluguel;

	public CasaTabuleiro (int valorCompra, int valorAluguel)
	{
		this.valorCompra = valorCompra;
		this.valorAluguel = valorAluguel;	
	}
}

