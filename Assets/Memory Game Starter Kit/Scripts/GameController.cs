using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour 
{


	public Sprite[] spriteCardsBack;//frente y tras de las cartas
	public Sprite[] spriteCardsFront;
	public Sprite spriteCardShadow;///es la sombra de las cartas
	
	// Only mostrarVentanaDeDescripcionDeCreencia it if needed.
	public bool mostrarVentanaDeDescripcionDeCreencia = false;
	public string cardInPair;
	//public Rect windowRect = new Rect (32, 32, 700, 500);

	private Rect windowRect = new Rect (32, 32, Screen.width - 64, Screen.height - 64);


	/// <summary>
	/// How fast to uncover a card, higher values = faster
	/// </summary>
	[Range(1f, 16f)]
	public float uncoverTime = 4f;
	
	/// <summary>
	/// how fast to deal one card, higher values = faster
	/// </summary>
	[Range(1f, 16f)]
	public float dealTime = 4f;
	
	[Range(0.1f, 10f)]
	public float checkPairTime = 0.5f;
	
	/// <summary>
	/// The Padding between 2 Cards
	/// </summary>
	[Range(2, 32)]
	public int cardsPadding = 4;
	
	int pairCount = 4;
	
	/// <summary>
	/// Create a fake shadow?
	/// </summary>
	public bool shadow = true;
	[Range(-32, 32)]
	public float shadowOffsetX = 4;
	[Range(-32, 32)]
	public float shadowOffsetY = -4;
	
	float shadOffsetX;
	float shadOffsetY;
	int incio, final;
	int chosenCardsBack = 0;
	int[] chosenCardsFront;
	
	Vector3 dealingStartPosition = new Vector3(-12800, -12800, -8);
	
	// move counters
	int totalMoves = 0;
	int bestMoves = 0;
	int uncoveredCards = 0;
	int tiempojugado=0;
	int segundos=0;
	int mejortiempo=0;
	int categoria;
	int Scategoria=0,Scategoria0=0,Scategoria4=0,Scategoria5=0;

	Transform[] selectedCards = new Transform[2];
	
	//bool memorySolved = false;
	
	int oldPairCount;
	
	// Input check
	bool isTouching = false;
	bool isUncovering = false;
	bool isDealing = false;
	bool isInGame = false;
	bool GameOver=false;
	
	// GUI Skin
	public GUISkin skin;
	
	// Soundeffects
	public AudioClip soundDealCard;
	public AudioClip soundButtonClick;
	public AudioClip soundUncoverCard;
	public AudioClip soundFoundPair;
	public AudioClip soundNoPair;
	public int fontSize;
	// Use this for initialization
	void Start () 
	{
//		float tableScale = (GameObject.Find("Table") == null) ? 1f : GameObject.Find("Table").transform.localScale.x;

		//GameObject.Find ("Table").transform.localScale.x = (Screen.width / 10) * 4;
	}
	
	void OnGUI()//interfaz de usuario grafico
	{
		fontSize = Screen.height /20;

		if(skin != null)
			GUI.skin = skin;

		if (mostrarVentanaDeDescripcionDeCreencia)
		{
			GUIStyle style = new GUIStyle(GUI.skin.window);
			style.fontSize= fontSize;
			string messageToShow="";
			switch(cardInPair)
			{
				case "playcardsFront_3":
				messageToShow = "EL HIJO\n\n  Jesús se hizo humano para salvarnos. \n Por su medio -nuestro ayudador, \nabogado y Redentor-\n podemos comenzar de nuevo. \nNos está preparando el cielo,\n y regresará a llevarnos con Él.";
				break;
				case "playcardsFront_2":
				messageToShow = "EL PADRE\n\n Dios el Padre es la\n fuente de todo amor y vida. \nEnvió a su Hijo\n para salvarnos de nuestros pecados\n y de nosotros mismos,\n y para mostrarnos cómo es Él.";
				break;
				case "playcardsFront_1":
				messageToShow = "LA TRINIDAD\n\n  Dios, el Inmortal, Todopoderoso \ny plenamente amante, \nes una relación del Padre, el Hijo y \nel Espíritu Santo. Es el único ser \ndigno de nuestra adoración. \nDios es nuestro Creador, Redentor y Amigo.";
				break;
				case "playcardsFront_0":
				messageToShow = "LAS SAGRADAS ESCRITURAS\n\n  Antigua, intemporal y una obra \nmaestra de la literatura, \nnos revela la función de Dios \nen la historia humana, \nnuestro lugar dentro\n del plan de Dios y la verdad, \npara guiarnos y protegernos del engaño.";
				break;
				case "playcardsFront_7":
				messageToShow = "EL GRAN CONFLICTO\n\n  Satanás acusó a Dios de no ser \ndigno de confianza y \nde ser injusto. Dios nos \ndio la libertad de escoger, y \nla historia humana\n muestra el resultado de la rebelión,\n y el increíble poder del amor \nde Dios para salvarnos.";
				break;
				case "playcardsFront_6":
				messageToShow = "LA NATURALEZA DEL HOMBRE\n\n  Aunque moldeado a la imagen\n de Dios, el ser humano, \nahora quebrantado por el pecado,\n necesitó de un Salvador perfecto\n para experimentar la reconciliación.\n El Espíritu restaura\n el reflejo de Dios en \nnosotros para que Dios \npueda obrar por nuestro medio.";
				break;
				case "playcardsFront_5":
				messageToShow = "LA CREACIÓN\n\n  Dios creó a nuestro mundo \ncon creatividad brillante y \ntierno cuidado. Creó a la\n humanidad para que\n cuidara y se deleitara en el planeta, \ny para que el resto de la creación \ngozara de un equilibrio perfecto.";
				break;
				case "playcardsFront_4":
				messageToShow = "EL ESPIRITU SANTO\n\n  El Espíritu Santo nos \ninspira, nos capacita y guía nuestra comprensión.\n El Espíritu toca nuestro corazón y nos transforma, \nrenovando en nosotros la imagen de \nDios con la cual fuimos creados.";
				break;
				case "playcardsFront_11":
				messageToShow = "LA IGLESIA\n\nLa iglesia es la familia de Dios\n en la Tierra, que sirve, \ncelebra, estudia y adora junta a Dios. \nAl mirar a Jesús \ncomo su líder y\n Redentor, la iglesia es llamada a llevar\n a todas las personas las \nbuenas nuevas de salvación.";
				break;
				case "playcardsFront_10":
				messageToShow = "CRECIMIENTO EN CRISTO\n\n  La salvación transforma nuestra \nmanera de ver el mundo. \nYa no tememos el pasado o el futuro,\n sino que abrazamos un presente \nlleno de esperanza, amor, entusiasmo y alabanza, \nporque el Espíritu vive en nosotros.";
				break;
				case "playcardsFront_9":
				messageToShow = "LA EXPERIENCIA DE LA SALVACIÓN\n\n El Espíritu Santo revela nuestra \nnecesidad de Cristo y, \ncuando aceptamos la gracia y la salvación de Dios, \nnos hace nuevas criaturas. \nEl Espíritu edifica nuestra \nfe y nos ayuda a dejar atrás una\n vida quebrantada.";
				break;
				case "playcardsFront_8":
				messageToShow = "LA VIDA, MUERTE, Y RESURECCIÓN DE CRISTO\n\n  Dios envío a Jesús, su Hijo,\n para vivir una vida perfecta \n y para morir la muerte que nos merecíamos. \nCuando aceptamos el sacrificio de Cristo, \ntenemos acceso a la vida eterna.";
				break;
				case "playcardsFront_15":
				messageToShow = "LA CENA DEL SEÑOR\n\n La Cena del Señor simboliza\n nuestra aceptación del cuerpo\n y la sangre de Cristo, que fue \nderramado y quebrantado \npor nosotros. Al escudriñar nuestros corazones, \nnos lavamos mutuamente los pies, recordando el \nhumilde ejemplo de servicio de Jesús.";
				break;
				case "playcardsFront_14":
				messageToShow = "EL BAUTISMO\n\nEl bautismo simboliza y declara \nnuestra nueva fe en Cristo\n y nuestra confianza en su perdón. \nSomos sepultados \nen el agua para levantarnos a una \nnueva vida en Cristo, capacitados\n por el Espíritu Santo.";
				break;
				case "playcardsFront_13":
				messageToShow = "LA UNIDAD EN EL CUERPO DE CRISTO\n\nEl cuerpo humano sirve\n como la perfecta \nmetáfora del pueblo de Dios en la Tierra. \nEstá compuesto por muchas partes \nque son muy diferentes entre sí, \npero como resultado del Espíritu Santo \nen nosotros, se produce \nuna armonía de voces y la unidad en la misión.";
				break;
				case "playcardsFront_12":
				messageToShow = "EL REMANENTE Y SU MISION\n\n En el fin del tiempo, Dios \nllama a su pueblo para que \nregrese a las verdades fundamentales.\n Al declarar el pronto\n regreso de Cristo, el remanente destaca \na Dios como Creador, \nel juicio celestial y el peligro del\n compromiso espiritual.";
				break;
				case "playcardsFront2_17":
				messageToShow = "LOS DONES Y MINISTERIOS ESPIRITUALES\n\n Ya sea en las artes o la enseñanza \no al escuchar una predicación, \nel Espíritu Santo brinda a cada \nuno capacidades y talentos \nque podemos usar para gloria de Dios\n y la misión de la iglesia.";
				break;
				case "playcardsFront2_18":
				messageToShow = "EL DON DE PROFECÍA\n\nComo en los tiempos bíblicos,\n en los últimos días, el Espíritu Santo\n ha bendecido al pueblo de Dios \ncon el don de profecía. \nAlguien que demostró ese don fue \nElena G. White, una de\n las fundadoras de la Iglesia Adventista del Séptimo Día.";
				break;
				case "playcardsFront2_19":
				messageToShow = "LA LEY DE DIOS\n\nLos Diez mandamientos \nnos muestran la voluntad \ny el amor de Dios por nosotros. \nSus consejos nos dicen\n cómo relacionarnos con Dios y los demás. \nJesús vivió la \nley cómo nuestro ejemplo y \nperfecto sustituto.";
				break;
				case "playcardsFront2_20":
				messageToShow = "EL SÁBADO\n\nEl Sábado es el don que\n Dios nos ha dado,\n un momento para el descanso \ny la restauración\n de nuestra conexión con Dios\n y nuestro prójimo.\n Nos recuerda de la creación de Dios\n y la gracia de Cristo.";
				break;
				case "playcardsFront2_21":
				messageToShow = "MAYORDOMÍA\n\nDios nos hace responsables de nosotros, \nel mundo, nuestros prójimos\n y los recursos materiales. \nCuando vivimos para Él, \nDios bendice nuestros esfuerzos.";
				break;
				case "playcardsFront2_22":
				messageToShow = "CONDUCTA CRISTIANA\n\nDios nos llama para que \nvivamos a la luz de su gracia, \nsabiendo el costo infinito que \nDios pagó para salvarnos. \nMediante el Espíritu glorificamos\n a Dios con nuestra mente, \ncuerpo y espíritu.";
				break;
				case "playcardsFront2_23":
				messageToShow = "EL MATRIMONIO Y LA FAMILIA\n\n  El hombre y la mujer,\n creados a imagen de Dios, \nestán diseñados para vivir\n en relación. El matrimonio es el \nideal divino para vivir en armonía, \ny para que los niños \ncrezcan en seguridad y amor.";
				break;
				case "playcardsFront2_24":
				messageToShow = "EL MINISTERIO DE CRISTO EN EL SANTUARIO CELESTIAL\n\n El sacrificio último de Cristo\n nos da la confianza de\n acercarnos a Dios, \nsabiendo que somos perdonados. \nAhora Jesús está repasando nuestra\n vida antes de su regreso, \npara que no haya dudas de que sus juicios son \npronunciados en amor.";
				break;
				case "playcardsFront2_25":
				messageToShow = "LA SEGUNDA VENIDA DE CRISTO\n\nAguardamos con ansias el\n regreso prometido de Cristo, \ncuando Él resucitará a sus hijos \nsalvados y los \nllevará al cielo. \nAunque no podemos saber con exactitud\n cuándo regresará, podemos\n vivir con la alegría de \nesa expectativa.";
				break;
				case "playcardsFront2_26":
				messageToShow = "LA MUERTE Y RESURECCIÓN\n\nEl pecado y la muerte nos\n separa del Dios de la vida, \npero la victoria de Cristo\n sobre la muerte significa \nque los salvados pueden aguardar \nla resurrección y la vida eterna. \n";
				break;
				case "playcardsFront2_27":
				messageToShow = "EL MILENIO Y EL FIN DEL PECADO\n\n Mientras los salvados se \nreconectan con Dios, Satanás \ny sus seguidores están \natrapados en este planeta. Después de mil años, \nDios resucitará a los perdidos\n para el juicio final, antes\n de destruir el pecado y los pecadores.";
				break;
				case "playcardsFront2_28":
				messageToShow = "LA TIERRA NUEVA\n\n Dios recreará nuestro mundo\n una vez manchado por el pecado,\n y vivirá con nosotros para siempre. Podremos\n alcanzar finalmente \nnuestro verdadero potencial, \nviviendo en el amor y el \ngozo para el cual Dios nos ha creado.";
				break;

			}
			windowRect = GUI.Window (0, windowRect, DialogWindow, messageToShow,style);
		}

		GUI.BeginGroup(new Rect(30, 32, (Screen.width/10)*3, Screen.height - 64)); //tamaño y veentana donde se dibuja
		{
			//GUI.Box(new Rect(0, 0, (Screen.width/10)*3, Screen.height - 64), "");//cuadro de jugar y botonos
			


			//JUGAR
			// Play clears the Game Field and Deals a new set of Cards
			//GUI.color = Color.yellow;
			if(GUI.Button(new Rect(16, 16, (Screen.width/10)*3-32, (Screen.height - 64)/6), "Jugar") && !(isDealing || isUncovering))
			{
				isInGame = true;


				if(!mostrarVentanaDeDescripcionDeCreencia)
				{
					if(soundButtonClick != null)
						GetComponent<AudioSource>().PlayOneShot(soundButtonClick);
					
					CreateDeck();
				}
			}


			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontSize = fontSize;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GUI.skin.label.fontSize = fontSize;
			
			
			string[] clases = new string[6] {"Dios", "Humanidad", "Salvacion", "Iglesia", "Vida diaria", "Apocalipsis"};

			//GUI.color = Color.green;
			//GUI.Label(new Rect(16, (Screen.height - 64)/6+16, (Screen.width/10)*3-32, (Screen.height - 64)/12), "" + clases[categoria]);//para las letras posiciones de label
			
			//categoria = (int)GUI.HorizontalSlider(new Rect(16, (Screen.height - 64)/6+32+fontSize, (Screen.width/10)*3-32, (Screen.height - 64)/6), categoria, 0, 5);

			switch(categoria){
			case '0':
				Scategoria0=5;
				isInGame = false;
				GameOver =false;
				tiempojugado=0;
				pairCount=5;
				incio=0;
				final=5;
				break;
			case '1':
				Scategoria=2;
				GameOver =false;
				isInGame = false;
				tiempojugado=0;
				pairCount =2;
				incio=5;
				final=6;
				break;
			case '2':
				Scategoria=4;
				GameOver =false;
				tiempojugado=0;
				pairCount =4;
				incio=7;
				final=10;
				break;
			case '3':
				Scategoria=7;
				GameOver =false;
			pairCount =7;
				incio=11;
				final=17;
				break;
			case '4':
				Scategoria4=5;
				GameOver =false;
			pairCount =5;
				incio=18;
				final=22;
				break;
			case '5':
				Scategoria5=5;
				GameOver =false;
				isInGame = false;
				tiempojugado=0;
				pairCount =5;
				incio=23;
				final=27;
				break;
				
				
			}

			if(pairCount != oldPairCount)
			{
				oldPairCount = pairCount;


				bestMoves = PlayerPrefs.GetInt("Memory_" + Scategoria + "_Pairs", 0);
				mejortiempo = PlayerPrefs.GetInt("Memory_" + Scategoria + "segundos", 0);

			}
			
			//GUI.skin.label.alignment = TextAnchor.UpperLeft;
			//GUI.skin.label.fontSize = fontSize;

			//GUI.color = Color.yellow;
			//GUI.Label(new Rect(16, (Screen.height - 64)/3+(Screen.height - 64)/6, (Screen.width/10)*3-32, (Screen.height - 64)/6), "Movimientos: " + totalMoves);

			//GUI.skin.label.alignment = TextAnchor.UpperLeft;
			//GUI.skin.label.fontSize = fontSize;

			//GUI.color = Color.red;
			//GUI.Label(new Rect(16, (Screen.height - 64)/3+(Screen.height - 64)/3, (Screen.width/10)*3-32, (Screen.height - 64)/6), "Record: " + bestMoves);
		

			//GUI.skin.label.alignment = TextAnchor.UpperLeft;
			//GUI.skin.label.fontSize = fontSize;
			//GUI.Label(new Rect(16,(Screen.height - 64)/3+(Screen.height - 64)/3+(Screen.height - 64)/6, (Screen.width/10)*3-32,   (Screen.height - 64)/6), "Tiempo: " + segundos);
			//faltaba este label
			//GUI.skin.label.alignment = TextAnchor.UpperLeft;
			//GUI.skin.label.fontSize = fontSize;
			//GUI.Label(new Rect(16,(Screen.height - 64)/3, (Screen.width/10)*3-32, (Screen.height - 64)/6), "Mejor Tiempo:" + mejortiempo);
			
			
		}
		GUI.EndGroup();
	}
	
	void CreateDeck()//
	{
		isDealing = true;
		isInGame = false;
		// clear the game field and reset variables
		DestroyImmediate(GameObject.Find("DeckParent"));
		DestroyImmediate(GameObject.Find("Temp"));
		selectedCards = new Transform[2];
		totalMoves = 0;
		uncoveredCards = 0;
		tiempojugado=0;
		mejortiempo = 0;
		segundos=0;
		//memorySolved = false;
		
		// get personal best for this board size
		bestMoves = PlayerPrefs.GetInt("Memory_" + Scategoria + "_Pairs", 0);
		mejortiempo = PlayerPrefs.GetInt("Memory_" + Scategoria + "segundos", 0);
		//tiempojugado = 
		
		
		// randomly chose the back graphic of the cards
		chosenCardsBack = Random.Range(0, spriteCardsBack.Length);
		
		switch(categoria){
		case 0:
			Scategoria0=5;
			GameOver =false;
            isInGame = false;
			tiempojugado=0;
			pairCount=5;
			incio=0;
			final=5;
			break;
		case 1:
			Scategoria=2;
			GameOver =false;
			 isInGame = false;
			tiempojugado=0;
			pairCount =2;
			incio=5;
			final=7;
			break;
		case 2:
			Scategoria=4;
			GameOver =false;
			isInGame = false;
			tiempojugado=0;
			pairCount =4;
			incio=7;
			final=11;
			break;
		case 3:
			Scategoria=7;
			GameOver =false;
			isInGame = false;
			tiempojugado=0;
			pairCount =7;
			incio=11;
			final=18;
			break;
		case 4:
			Scategoria4=5;
			GameOver =false;
			isInGame = false;
			tiempojugado=0;
			pairCount =5;
			incio=18;
			final=23;
			break;
		case 5:
			Scategoria5=5;
			GameOver =false;
			isInGame = false;
			tiempojugado=0;
			pairCount =5;
			incio=23;
			final=28;
			break;
			
			
		}
		
		
		List<int> tmp = new List<int>();
		//lista de sprites
		List<Sprite> tmpSprite = new List<Sprite>();
		
		for(int i = 0; i < spriteCardsFront.Length; i++)
		{
			tmp.Add(i);
			tmpSprite.Add(spriteCardsFront[i]);
		}
		
		
		
		//tmp.Shuffle();
		
		chosenCardsFront = tmp.GetRange(incio, pairCount).ToArray();
		
		GameObject deckParent = new GameObject("DeckParent"); // this holds all the cards
		GameObject temp = new GameObject("Temp");
		
		int cur = 0;
		
		float minX = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity;
		float minY = Mathf.Infinity;
		float maxY = Mathf.NegativeInfinity;
		
		// calculate columns and rows needed for the selected pair count
		int cCards = pairCount * 2;
		int cols = (int)Mathf.Sqrt(cCards);
		int rows = (int)Mathf.Ceil(cCards / (float)cols);
		
		List<int> deck = new List<int>();
		for(int i = 0; i < pairCount; i++)
		{
			deck.AddRange(new int[] {i, i});
		}
		deck.Shuffle();
		
		int cardWidth = 0;
		int cardHeight = 0;
		
		for(int x = 0; x < rows; x++)
		{
			for(int y = 0; y < cols; y++)
			{
				if(cur > cCards-1)
					break;
				
				// Create the Card
				GameObject card = new GameObject("Card"); // parent object
				GameObject cardFront = new GameObject("CardFront");
				GameObject cardBack = new GameObject("CardBack");
				GameObject destination = new GameObject("Destination");
				
				cardFront.transform.parent = card.transform; // make front child of card
				cardBack.transform.parent = card.transform; // make back child of card
				
				// front (motive)
				cardFront.AddComponent<SpriteRenderer>();
				cardFront.GetComponent<SpriteRenderer>().sprite = spriteCardsFront[chosenCardsFront[deck[cur]]];
				cardFront.GetComponent<SpriteRenderer>().sortingOrder = -1;
				
				// back
				cardBack.AddComponent<SpriteRenderer>();
				cardBack.GetComponent<SpriteRenderer>().sprite = spriteCardsBack[chosenCardsBack];
				cardBack.GetComponent<SpriteRenderer>().sortingOrder = 1;
				
				cardWidth = (int)spriteCardsBack[chosenCardsBack].rect.width;
				cardHeight = (int)spriteCardsBack[chosenCardsBack].rect.height;
				
				// now add the Card GameObject to the Deck GameObject "deckParent"
				card.tag = "Card";
				card.transform.parent = deckParent.transform;
				card.transform.position = dealingStartPosition;
				card.AddComponent<BoxCollider2D>();
				card.GetComponent<BoxCollider2D>().size = new Vector2(cardWidth, cardHeight);
				card.AddComponent<CardProperties>().Pair = deck[cur];
				card.GetComponent<CardProperties>().Nombre = spriteCardsFront[chosenCardsFront[deck[cur]]];
				
				destination.transform.parent = temp.transform;
				destination.tag = "Destination";
				destination.transform.position = new Vector3(x * (cardWidth + cardsPadding), y * (cardHeight + cardsPadding));
				
				if(shadow)
				{
					GameObject cardShadow = new GameObject("CardShadow");
					
					cardShadow.tag = "CardShadow";
					cardShadow.transform.parent = deckParent.transform;
					cardShadow.transform.position = dealingStartPosition;
					cardShadow.AddComponent<SpriteRenderer>();
					cardShadow.GetComponent<SpriteRenderer>().sprite = spriteCardShadow;
					cardShadow.GetComponent<SpriteRenderer>().sortingOrder = -2;
				}
				cur++;
				
				// determine positions for the camera helper objects
				Vector3 pos = destination.transform.position;
				minX = Mathf.Min(minX, pos.x - cardWidth);
				minY = Mathf.Min(minY, pos.y - cardHeight);
				maxX = Mathf.Max(maxX, pos.x + cardWidth + shadowOffsetX);
				maxY = Mathf.Max(maxY, pos.y + cardHeight + shadowOffsetY);
			}
		}
		
		// scale to fit onto the "table"
		float tableScale = (GameObject.Find("Table") == null) ? 1f : GameObject.Find("Table").transform.localScale.x;
		float scale = tableScale / (maxX + cardsPadding);
		
		Vector2 point = LineIntersectionPoint(
			new Vector2(minX, maxY),
			new Vector2(maxX, minY),
			new Vector2(minX, minY),
			new Vector2(maxX, maxY)
			);
		
		temp.transform.position -= new Vector3(point.x * scale, point.y * scale);
		
		shadOffsetX = shadowOffsetX * scale;
		shadOffsetY = shadowOffsetY * scale;
		
		deckParent.transform.localScale = new Vector3(scale, scale, scale);
		temp.transform.localScale = new Vector3(scale, scale, scale);
		
		DealCards ();
	}

	void DealCards()
	{
		StartCoroutine (dealCards ());
	}
	
	IEnumerator dealCards()
	{
		GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
		GameObject[] cardsShadow = GameObject.FindGameObjectsWithTag("CardShadow");
		GameObject[] destinations = GameObject.FindGameObjectsWithTag("Destination");
		
		for(int i = 0; i < cards.Length; i++)
		{
			float t = 0; 
			
			if(soundDealCard != null)
				GetComponent<AudioSource>().PlayOneShot(soundDealCard);
			
			while(t < 1f)
			{
				t += Time.deltaTime * dealTime;
				
				cards[i].transform.position = Vector3.Lerp(
					dealingStartPosition, destinations[i].transform.position, t);
				
				if(cardsShadow.Length > 0)
				{
					cardsShadow[i].transform.position = Vector3.Lerp(
						dealingStartPosition, 
						destinations[i].transform.position + new Vector3(shadOffsetX, shadOffsetY), t);
				}
				
				yield return null;
			}
			yield return null;
		}
		
		isDealing = false;
		isInGame = true;
		yield return 0;
	}
	
	void UncoverCard(Transform card)
	{
		StartCoroutine (uncoverCard(card, true));
	}
	
	IEnumerator uncoverCard(Transform card, bool uncover)
	{
		isUncovering = true;
		
		float minAngle = uncover ? 0 : 180;
		float maxAngle = uncover ? 180 : 0; 
		
		float t = 0;
		bool uncovered = false;
		
		if(soundUncoverCard != null)
			GetComponent<AudioSource>().PlayOneShot(soundUncoverCard);
		
		// find the shadow for the selected card
		var shadow = GameObject.FindGameObjectsWithTag("CardShadow").Where(
			g => (g.transform.position == card.position + new Vector3(shadOffsetX, shadOffsetY))).FirstOrDefault();
		
		while(t < 1f)
		{
			t += Time.deltaTime * uncoverTime;
			
			float angle = Mathf.LerpAngle(minAngle, maxAngle, t);
			card.eulerAngles = new Vector3(0, angle, 0);
			
			if(shadow != null)
				shadow.transform.eulerAngles = new Vector3(0, angle, 0);
			
			if( ( (angle >= 90 && angle < 180) || 
			     (angle >= 270 && angle < 360) ) && !uncovered)
			{
				uncovered = true;
				for(int i = 0; i < card.childCount; i++)
				{
					// reverse sorting order to mostrarVentanaDeDescripcionDeCreencia the otherside of the card
					// otherwise you would still see the same sprite because they are sorted 
					// by order not distance (by default)
					Transform c = card.GetChild(i);
					c.GetComponent<SpriteRenderer>().sortingOrder *= -1;
					
					yield return null;
				}
			}
			
			yield return null;
		}
		
		// check if we uncovered 2 cards
		if(uncoveredCards == 2)
		{
			// if so compare the cards
			if(selectedCards[0].GetComponent<CardProperties>().Pair !=
			   selectedCards[1].GetComponent<CardProperties>().Pair)
			{
				if(soundNoPair != null)
					GetComponent<AudioSource>().PlayOneShot(soundNoPair);
				
				// if they are not equal cover back
				yield return new WaitForSeconds(checkPairTime);
				StartCoroutine (uncoverCard(selectedCards[0], false));
				StartCoroutine (uncoverCard(selectedCards[1], false));
			}
			else
			{
				if(soundFoundPair != null)
					GetComponent<AudioSource>().PlayOneShot(soundFoundPair);
				//seleccionamos que creencia resolvimos el par

				cardInPair= selectedCards[0].GetComponent<CardProperties>().Nombre.name;
				mostrarVentanaDeDescripcionDeCreencia = true;
				isInGame = false;




				// set as solved
				selectedCards[0].GetComponent<CardProperties>().Solved = true;
				selectedCards[1].GetComponent<CardProperties>().Solved = true;
			}
			selectedCards[0].GetComponent<CardProperties>().Selected = false;
			selectedCards[1].GetComponent<CardProperties>().Selected = false;
			uncoveredCards = 0;
			totalMoves++;
			
			yield return new WaitForSeconds(0.3f);
		}
		
		// check if the memory is solved checa si el memorama esta resuelto
		if(IsSolved())
		{

			isInGame = false;
			GameOver=true;

			int score = PlayerPrefs.GetInt("Memory_" + Scategoria + "_Pairs", 0);
			int scoretime = PlayerPrefs.GetInt("Memory_" + Scategoria  + "segundos", 0);
			
			
			if(score > totalMoves || score == 0)//para guardar el mejor puntaje
			{
				bestMoves = totalMoves;// variable temporal
			}


			PlayerPrefs.SetInt("Memory_" + Scategoria + "_Pairs", bestMoves);
			
			if(scoretime > segundos || scoretime == 0)//para guardar el mejor tiempo
			{//lo tenian al revez
				mejortiempo = segundos;// variable temporal
			}

			PlayerPrefs.SetInt("Memory_" + Scategoria + "segundos", mejortiempo);//aqui tenian pairs
			segundos=0;


			//memorySolved = true;
		}
		
		isUncovering = false;
		yield return 0;
	}
	// This is the actual window.
	void DialogWindow (int windowID)
	{
	

		if(GUI.Button(new Rect(18,windowRect.height-60, windowRect.width - 30, 40), "Ok"))
				{
			GUI.color = Color.red;
			isInGame = true;
			mostrarVentanaDeDescripcionDeCreencia = false;
		}
	}
	bool IsSolved()
	{
					
				foreach(GameObject g in GameObject.FindGameObjectsWithTag("Card"))
		{

			if(!g.GetComponent<CardProperties>().Solved)
				return false;
		}

		
		return true;
	}
	
	// Update is called once per frame
	void Update () 
	{



		if(isDealing)
			return;//para salir del metodo

	
		///control del tiempo
		if (isInGame == true) {
						tiempojugado += 1;
						segundos = tiempojugado / 270;
				} 		


		if (GameOver == true) {
			tiempojugado = 0;
			segundos = 0;
		} 		



		if((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !isTouching && !isUncovering && uncoveredCards < 2 && !mostrarVentanaDeDescripcionDeCreencia)
		{
			isTouching = true;
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
			
			// we hit a card
			if (hit.collider != null)
			{
				if(!hit.collider.GetComponent<CardProperties>().Selected)
				{
					// and its not one of the already solved ones
					if(!hit.collider.GetComponent<CardProperties>().Solved)
					{
						// uncover it
						UncoverCard(hit.collider.transform);
						selectedCards[uncoveredCards] = hit.collider.transform;
						selectedCards[uncoveredCards].GetComponent<CardProperties>().Selected = true;
						uncoveredCards += 1;
					}
				}
			}
		}
		else
		{
			isTouching = false;

		}
	}
	
	Vector2 LineIntersectionPoint(Vector2 ps1, Vector2 pe1, Vector2 ps2, Vector2 pe2)
	{
		// Get A,B,C of first line - points : ps1 to pe1
		float A1 = pe1.y-ps1.y;
		float B1 = ps1.x-pe1.x;
		float C1 = A1*ps1.x+B1*ps1.y;
		
		// Get A,B,C of second line - points : ps2 to pe2
		float A2 = pe2.y-ps2.y;
		float B2 = ps2.x-pe2.x;
		float C2 = A2*ps2.x+B2*ps2.y;
		
		// Get delta and check if the lines are parallel
		float delta = A1*B2 - A2*B1;
		if(delta == 0)
			return new Vector2();
		
		// now return the Vector2 intersection point
		return new Vector2(
			(B2*C1 - B1*C2)/delta,
			(A1*C2 - A2*C1)/delta
			);
	}
}
