using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bankrupt;

public class UpdateUIAposJogada : MonoBehaviour,FimJogadaListener
{
	List<Player> players;
	List<CasaTabuleiro> casas;
	Dictionary<Player,Transform> posicaoPorPlayer;
	Dictionary<CasaTabuleiro,Transform> posicaoPorCasa;

	public Transform casaTransform;
	public Transform playersTransform;
	public Text textoBanco;


	public void Init (List<Player> players, List<CasaTabuleiro> casas)
	{
		this.players = players;
		this.casas = casas;
		criaPlayers ();
		criaCasas ();
	}

	private void criaPlayers ()
	{
		posicaoPorPlayer = new Dictionary<Player, Transform> ();
		this.players.ForEach (p => posicaoPorPlayer.Add (p, criaPlayer (p).transform));
	}

	private void criaCasas ()
	{
		posicaoPorCasa = new Dictionary<CasaTabuleiro, Transform> ();
		this.casas.ForEach (c => posicaoPorCasa.Add (c, criaCasa (c).transform));
	}

	private Vector3 getPosicaoCasaPorIndice (int indice, int total)
	{
		const int width = 23;
		const int height = 18;
		const float heightOffset = 3f;
		float percentual = (float)indice / total;
		Vector3 posicao;
		float curWidth;
		float curHeight;
		if (percentual < 0.25f) {
			curWidth = width;
			curHeight = (0.25f - percentual) / 0.25f * height;
		} else if (percentual < 0.5f) {
			curWidth = (0.25f - (percentual - 0.25f)) / 0.25f * width;
			curHeight = -heightOffset;
		} else if (percentual < 0.75f) {
			curWidth = 0;
			curHeight = (percentual - 0.5f) / 0.25f * height;
		} else {
			curWidth = (percentual - 0.75f) / 0.25f * width;
			curHeight = height + heightOffset;
		}
		return new Vector3 (curWidth, curHeight, 0) - new Vector3 (width / 2, height / 2, 0);
		//return 15 * new Vector3 (Mathf.Sin (percentual * 2 * Mathf.PI), Mathf.Cos (percentual * 2 * Mathf.PI), 0);
	}

	private GameObject criaCasa (CasaTabuleiro casa)
	{
		GameObject go = GameObject.Instantiate (Resources.Load ("CasaTabuleiro"), casaTransform) as GameObject;
		go.name = casa.ToString ();
		go.transform.position = getPosicaoCasaPorIndice (casas.IndexOf (casa), casas.Count);
		go.GetComponent<UICasaTabueiro> ().Init (casa);
		return go;
	}

	public GameObject criaPlayer (Player p)
	{
		GameObject go = GameObject.Instantiate (Resources.Load ("Player"), playersTransform) as GameObject;
		go.GetComponent<SpriteRenderer> ().color = getCorByPlayer (p);
		go.name = p.ToString ();
		go.transform.position = go.transform.position + new Vector3 (players.IndexOf (p), 0, 0);
		return go;
	}

	private Color getCorByPlayer (Player p)
	{
		if (p != null) {
			return Color.HSVToRGB ((float)players.IndexOf (p) / players.Count, 1, 1);
		} else {
			return Color.white;
		}
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}


	public void NotificaFimJogada (Banco banco, Player player, TabuleiroManager tabuleiroManager)
	{
		updateBanco (banco);
		updateTabuleiro (tabuleiroManager);
	}

	private void updateBanco (Banco banco)
	{
		string texto = "";
		players.ForEach (p => texto += p.ToString () + ": " + banco.GetSaldo (p) + "\n");
		textoBanco.text = texto;
	}

	private void updateTabuleiro (TabuleiroManager tabuleiroManager)
	{
		Dictionary<Transform, Vector3> novasPosicoes = new Dictionary<Transform, Vector3> ();
		players.ForEach (p => {
			novasPosicoes.Add (posicaoPorPlayer [p], posicaoPorCasa [tabuleiroManager.GetCasaAtual (p)].position + new Vector3 (players.IndexOf (p) - players.Count / 2, 0, 0));
		});
		atualizaPosicoes (novasPosicoes);
		casas.ForEach (c => posicaoPorCasa [c].gameObject.GetComponent<SpriteRenderer> ().color = getCorByPlayer (tabuleiroManager.GetDonoDaCasa (c)));
	}

	private void atualizaPosicoes (Dictionary<Transform, Vector3> posicoes)
	{
		StopAllCoroutines ();
		StartCoroutine (rotinaAtualizaPosicao (posicoes));
	}

	IEnumerator rotinaAtualizaPosicao (Dictionary<Transform, Vector3> posicoes)
	{
		print (posicoes.Count);
		for (float i = 0; i < 1; i += 0.05f) {
			yield return new WaitForSeconds (0.005f);
			foreach (KeyValuePair<Transform,Vector3> kv in posicoes) {
				kv.Key.position = Vector3.Lerp (kv.Key.position, kv.Value, i);
			}
		}
	}
}
