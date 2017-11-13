using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bankrupt;

public class Manager : MonoBehaviour
{
	public UpdateUIAposJogada listener;
	public DadoRoladoListener dado;
	GameManager gm;
	bool iniciou = false;
	// Use this for initialization
	void Start ()
	{
		ConfiguracoesJogo configuracoes = new ConfiguracoesJogo ();
		configuracoes.listaplayers = new List<Player> {
			new PlayerImpulsivo (),
			new PlayerExigente (),
			new PlayerCauteloso (),
			new PlayerAleatorio ()
		};
		configuracoes.casasTabuleiro = obtemCasasTabuleiroDoArquivo ("gameConfig.txt");
		listener.Init (configuracoes.listaplayers, configuracoes.casasTabuleiro);	
		gm = new GameManager (configuracoes);
		gm.AddFimJogadaListener (listener);
		gm.AddDadoRoladoListener (dado);
		iniciou = true;
	}


	public void BotaoProximaJogada ()
	{
		if (iniciou) {
			gm.ExecutaJogada ();
		}
	}

	public void BotaoComprar (bool opcao)
	{
		
	}



	private static List<CasaTabuleiro> obtemCasasTabuleiroDoArquivo (string nome)
	{
		List<CasaTabuleiro> casasLidas = new List<CasaTabuleiro> ();
		string[] linhas = System.IO.File.ReadAllLines (@nome);
		for (int i = 0; i < linhas.Length - 1; i++) {
			string[] valores = linhas [i].Split (new string[]{ " " }, System.StringSplitOptions.RemoveEmptyEntries);
			string valorCompra = valores [0].Trim ();
			string valorAluguel = valores [1].Trim ();

			casasLidas.Add (new CasaTabuleiro (int.Parse (valorCompra), int.Parse (valorAluguel)));
		}
		return casasLidas;
	}
}
