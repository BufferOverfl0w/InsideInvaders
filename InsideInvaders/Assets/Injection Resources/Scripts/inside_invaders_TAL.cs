using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace inside_invaders_TAL
{
	public enum operatorLogic
	{
		OR,
		AND
	}
    public enum concept_ID
	{
		// organes et parties du corps
		ORG_COEUR,
		ORG_POUMON_GAUCHE,
		ORG_POUMON_DROIT,
		ORG_FOIE,
		ORG_ESTOMAC,
		ORG_INTESTIN_GRELE,
		ORG_GROS_INTESTIN,
		ORG_GORGE,
		ORG_AORTE,

		// joueur
        PLAYER,

        // personnalite du bot
        PERS_HAB,   // operateur habituel
        PERS_NOV,   // nouvel operateur
        PERS_DDM,   // dame de menage

        // positions
        POS_UP,
        POS_DOWN,
        POS_LEFT,
        POS_RIGHT,
        POS_BETWEEN,

        // modifieurs
        MOD_LESS,
        MOD_MORE,
        MOD_NEG,
        MOD_POS,
        MOD_AGAIN,

		// confirmation
		POS_HERE,

        // conversation
        NOT_UNDERSTOOD,
        GREETING,
        THANK,
        QUESTION,

		//injecter
		INJECT

    }

    // concept
    public class Concept
    {
        // ID et mots associes au concept
        public concept_ID ID;
        public List<string> word_asso;
        // position pour les organes
        public float posX;
        public float posY;
		public operatorLogic logicOP;
		public int weight;

		public void addWord(string word){
			this.word_asso.Add (word);
			if(logicOP.Equals(operatorLogic.AND))
				weight++;
			else
				weight = 1;
		}
		public bool isValide(string [] phrase){
			List<string> newphrase = new List<string>(phrase);
			if (logicOP.Equals (operatorLogic.OR)) {
				foreach (string word in word_asso) {
					if (newphrase.Contains(word))
						return true;  // on est dans le cas d'un "ou logique" un match suffit 
				}
			} else {
				// cas d'un "et logique"
				foreach (string word in word_asso) {
					if (!newphrase.Contains(word))
						return false; 
				}
				return true;
			}
			return false;

		}

		public Concept(concept_ID ID, string word,operatorLogic op)
        {
            this.ID = ID;
            word_asso = new List<string>();
            word_asso.Add(word);
            posX = posY = 0f;
			this.logicOP = op;
			weight = 1;
        }

		public Concept(concept_ID ID, string word, float posX, float posY,operatorLogic op)
        {
            this.ID = ID;
            word_asso = new List<string>();
            word_asso.Add(word);
            this.posX = posX;
            this.posY = posY;
			this.logicOP = op;
			weight = 1;
        }

        // liste de tous les concepts
        public static List<Concept> concepts;

        // ajout a la liste des concepts
		public static void addToConcepts(concept_ID ID, string word, operatorLogic op = operatorLogic.OR)
        {
            for(int iC=0; iC<concepts.Count; ++iC)
            {
                if(concepts[iC].ID == ID)
                {
                    if (concepts[iC].word_asso.Contains(word))
                        return;
                    //concepts[iC].word_asso.Add(word);
					concepts[iC].addWord(word);
                    return;
                }
            }
            concepts.Add(new Concept(ID, word,op));
        }

		public static void addToConcepts(concept_ID ID, string word, float posX, float posY, operatorLogic op = operatorLogic.OR)
        {
            for (int iC = 0; iC < concepts.Count; ++iC)
            {
                if (concepts[iC].ID == ID)
                {
                    if (concepts[iC].word_asso.Contains(word))
                        return;
                    //concepts[iC].word_asso.Add(word);
					concepts[iC].addWord(word);
                    return;
                }
            }
            concepts.Add(new Concept(ID, word, posX, posY,op));
        }

        // initialisation de la liste des concepts
        public static void concepts_init()
        {
			concepts = new List<Concept>();

			addToConcepts(concept_ID.ORG_COEUR, "coeur", 1405f, 520f);

			addToConcepts(concept_ID.ORG_POUMON_GAUCHE, "poumon", 1515f, 505f,operatorLogic.AND);
			addToConcepts(concept_ID.ORG_POUMON_GAUCHE, "gauche");

			addToConcepts(concept_ID.ORG_POUMON_DROIT, "poumon", 1285f, 480f,operatorLogic.AND);
			addToConcepts(concept_ID.ORG_POUMON_DROIT, "droit");

			addToConcepts(concept_ID.ORG_FOIE, "foie", 1335f, 375f);
			addToConcepts(concept_ID.ORG_ESTOMAC, "estomac", 1465f, 360f);

			addToConcepts(concept_ID.ORG_INTESTIN_GRELE, "intestin", 1425f, 215f,operatorLogic.AND);
			addToConcepts(concept_ID.ORG_INTESTIN_GRELE, "grele");

			addToConcepts(concept_ID.ORG_GROS_INTESTIN, "intestin", 1280f, 215f,operatorLogic.AND);
            addToConcepts(concept_ID.ORG_GROS_INTESTIN, "gros");
          
            addToConcepts(concept_ID.ORG_GORGE, "gorge", 1390f, 725f);
			addToConcepts(concept_ID.ORG_AORTE, "aorte", 1390f, 615f);

            addToConcepts(concept_ID.PLAYER, "je");
            addToConcepts(concept_ID.PLAYER, "moi");
            addToConcepts(concept_ID.PLAYER, "nous");

            addToConcepts(concept_ID.POS_UP, "haut");
            addToConcepts(concept_ID.POS_UP, "dessus");
            addToConcepts(concept_ID.POS_DOWN, "bas");
            addToConcepts(concept_ID.POS_DOWN, "dessous");
            addToConcepts(concept_ID.POS_LEFT, "gauche");
	        addToConcepts(concept_ID.POS_RIGHT, "droite");
	        addToConcepts(concept_ID.POS_BETWEEN, "entre");
            addToConcepts(concept_ID.POS_BETWEEN, "milieu");

            addToConcepts(concept_ID.MOD_LESS, "moins");
            addToConcepts(concept_ID.MOD_LESS, "peu");
            addToConcepts(concept_ID.MOD_MORE, "plus");
            addToConcepts(concept_ID.MOD_MORE, "beaucoup");
            addToConcepts(concept_ID.MOD_NEG, "non");
            addToConcepts(concept_ID.MOD_NEG, "pas");
            addToConcepts(concept_ID.MOD_POS, "oui");
            addToConcepts(concept_ID.MOD_POS, "ok");
            addToConcepts(concept_ID.MOD_POS, "si");
            addToConcepts(concept_ID.MOD_POS, "bien");
            addToConcepts(concept_ID.MOD_POS, "bon");
            addToConcepts(concept_ID.MOD_AGAIN, "encore");

            addToConcepts(concept_ID.GREETING, "bonjour");
            addToConcepts(concept_ID.GREETING, "salut");
            addToConcepts(concept_ID.GREETING, "bonsoir");

            addToConcepts(concept_ID.THANK, "merci");
            addToConcepts(concept_ID.THANK, "remercie");
            addToConcepts(concept_ID.THANK, "remercier");

			addToConcepts(concept_ID.INJECT, "go");
			addToConcepts(concept_ID.INJECT, "injecte");
			addToConcepts(concept_ID.INJECT, "parti");

        }

        // recupere les concepts d'une phrase du joueur
        public static List<concept_ID> getConcepts(string sentence)
        {
            List<concept_ID> sentenceConcepts = new List<concept_ID>();
            string[] words = sentence.ToLower().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
			string[] words2 = new string[words.Length];
			for(int iW=0; iW<words.Length; ++iW)
				words2[iW] = Regex.Replace(words[iW], "[^A-Za-z0-9]", "");

            foreach(Concept c in concepts)
				if(c.isValide(words2))
					sentenceConcepts.Add(c.ID);
			
            return sentenceConcepts;
        }
		// recupere le concepts le plus lourds d'une phrase du joueur
		public static List<concept_ID> getConcepts2(string sentence)
		{
			List<concept_ID> sentenceConcepts = new List<concept_ID>();
			List<concept_ID> sentenceConcepts2 = new List<concept_ID>();
			string[] words = sentence.ToLower().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
			string[] words2 = new string[words.Length];
			for(int iW=0; iW<words.Length; ++iW)
				words2[iW] = Regex.Replace(words[iW], "[^A-Za-z0-9]", "");

			foreach(Concept c in concepts)
				if(c.isValide(words2))
					sentenceConcepts.Add(c.ID);
//
//			if (sentenceConcepts != null) {
//				UnityEngine.Debug.Log ("===sentenceConcepts 1 ===");
//				foreach( concept_ID  val in sentenceConcepts ){
//					Debug.Log ("val1 : "+val.ToString());
//				}
//			}
			//Debug.Log ("Count : "+sentenceConcepts.Count);
			if (sentenceConcepts.Count > 1) {
				int max = 0;
				int iMax = -1;
				for (int iR = 0; iR < sentenceConcepts.Count; ++iR) {
					//Debug.Log ("ID : " + sentenceConcepts [iR].ToString ());
					Concept conc = Concept.getConceptByID (sentenceConcepts[iR]);
					int poidsConcept = 1;
					if (conc != null) {
						poidsConcept = conc.weight;
					}
					//Debug.Log ("weight : " + poidsConcept);
					if (poidsConcept > max) {
						max = poidsConcept;
						iMax = iR;
					}
				}
				if (iMax > -1) {
					sentenceConcepts2.Add (sentenceConcepts [iMax]);
					return sentenceConcepts2;
				}
			}


			return sentenceConcepts;
		}


		// retrouve le concept d'un ID dans la liste
		public static Concept getConceptByID(concept_ID ID){
			foreach (Concept c in concepts)
				if (c.ID == ID)
					return c;
			//Debug.Log (ID + "not found !");
			return null;
		}
    }

    // reponse du bot
    public class Response
    {
        // texte et concepts de la reponse
		public List<string> listeMotsPossibles;
        public List<concept_ID> responseConcepts;

        // liste des reponses possibles
        public static List<Response> responses;

        public Response(string word, concept_ID ID)
        {
			if((word==null) || (word.Equals(""))){
				throw new Exception ("Word can't be empty !");
			}
			listeMotsPossibles = new List<string>();
			listeMotsPossibles.Add (word);
            responseConcepts = new List<concept_ID>();
            responseConcepts.Add(ID);
        }

		public Response( List<string> words, concept_ID ID)
		{
			if((words==null) || (words.Count==0) || (words[0].Equals(""))){
				throw new Exception ("Word can't be empty !");
			}
			listeMotsPossibles = words;
            responseConcepts = new List<concept_ID>();
            responseConcepts.Add(ID);
        }

		public string getRandomText(){
			int valRdm = UnityEngine.Random.Range (0, listeMotsPossibles.Count);
			return listeMotsPossibles [valRdm];
		}

		// retrouve la Response d'un ID dans la liste
		public static Response getResponseByID(concept_ID ID){
			foreach (Response rep in responses)
				if (rep.responseConcepts.Contains(ID))
					return rep;
			return null;
		}

        // ajout d'un concept a une reponse
        public static void addToResponses(string text, concept_ID ID){
			foreach (Response rep  in responses) {
				if (rep.responseConcepts.Contains (ID)) {
					// Le concept est déjà dans la liste
					if (!rep.listeMotsPossibles.Contains(text))
						rep.listeMotsPossibles.Add (text);
					return;
				}
			}
			responses.Add(new Response(text, ID));
		}


        // initialisaiton de la liste des reponses
        public static void responses_init()
        {
            responses = new List<Response>();
            addToResponses("Je n'ai pas compris.", concept_ID.NOT_UNDERSTOOD);
            addToResponses("Je ne vois pas ce que vous voulez dire.", concept_ID.NOT_UNDERSTOOD);
            addToResponses("Comment?", concept_ID.NOT_UNDERSTOOD);

            addToResponses("C'est la bonne direction?", concept_ID.POS_LEFT);
            addToResponses("Ici?", concept_ID.POS_LEFT);
            addToResponses("Par là?", concept_ID.POS_LEFT);
            addToResponses("C'est bon ici?", concept_ID.POS_LEFT);

            addToResponses("C'est la bonne direction?", concept_ID.POS_RIGHT);
            addToResponses("Ici?", concept_ID.POS_RIGHT);
            addToResponses("Par là?", concept_ID.POS_RIGHT);
            addToResponses("C'est bon ici?", concept_ID.POS_RIGHT);

            addToResponses("On est a la bonne hauteur?", concept_ID.POS_UP);
            addToResponses("Ici?", concept_ID.POS_UP);
            addToResponses("Par là?", concept_ID.POS_UP);
            addToResponses("C'est bon ici?", concept_ID.POS_UP);

            addToResponses("On est a la bonne hauteur?", concept_ID.POS_DOWN);
            addToResponses("Ici?", concept_ID.POS_DOWN);
            addToResponses("Par là?", concept_ID.POS_DOWN);
            addToResponses("C'est bon ici?", concept_ID.POS_DOWN);

            addToResponses("Ici?", concept_ID.POS_HERE);
            addToResponses("Par là?", concept_ID.POS_HERE);
            addToResponses("C'est bon ici?", concept_ID.POS_HERE);

            addToResponses("Merci de m'indiquer où vous souhaitez être injecté.", concept_ID.GREETING);
            addToResponses("Veuiller me guider afin que je vous injecte au bon endroit.", concept_ID.GREETING);

            addToResponses("Tres bien.", concept_ID.MOD_POS);
            addToResponses("D'accord.", concept_ID.MOD_POS);

            addToResponses("Ah... D'accord.", concept_ID.MOD_NEG);
            addToResponses("Je vois.", concept_ID.MOD_NEG);
            addToResponses("Non?", concept_ID.MOD_NEG);

            addToResponses("De rien!", concept_ID.THANK);
            addToResponses("Je vous en prie.", concept_ID.THANK);

			addToResponses("C'est partie  ",concept_ID.INJECT);
			addToResponses("Je t'injecte  ",concept_ID.INJECT);
        }

        // choisit la reponse la plus pertinente par rapport a une phrase de l'utilisateur
        public static string getBestResponse(List<concept_ID> sentenceConcepts, bool hasMoved)
        {
			if (sentenceConcepts != null) {
				UnityEngine.Debug.Log ("===sentenceConcepts===");
				foreach( concept_ID  val in sentenceConcepts ){
					Debug.Log ("val : "+val.ToString());
				}
			}
			//Debug.Log ("hasMoved : "+hasMoved);
			if (hasMoved) {
                //return getResponseByID (concept_ID.POS_HERE).getRandomText ();
                // return "Ici?";
                sentenceConcepts.Add(concept_ID.POS_HERE);
			}

			//Debug.Log ("Count : "+sentenceConcepts.Count);
			if (sentenceConcepts.Count != 0) {
				int[] responseConceptCount = new int[responses.Count];

				for (int iR = 0; iR < responses.Count; ++iR) {
					responseConceptCount [iR] = 0;
					foreach (concept_ID c in sentenceConcepts) {
						Response rep = responses [iR];
						if (rep.responseConcepts.Contains (c)) {
							Concept conc = Concept.getConceptByID (c);
							int poidsConcept = 1;
							if (conc != null) {
								poidsConcept = conc.weight;
							}
							responseConceptCount [iR] = responseConceptCount [iR] + poidsConcept;
						}
					}
						
				}

				int max = 0;
				int iMax = -1;
				for (int iR = 0; iR < responses.Count; ++iR)
					if (responseConceptCount[iR] > max)
					{
						max = responseConceptCount[iR];
						iMax = iR;
					}
				if (iMax > -1)
					return responses[iMax].getRandomText();
			}

			
            return responses[0].getRandomText();
        }

        // decide le mouvement du viseur
		public static float[] getMovement(List<concept_ID> sentenceConcepts, float x0, float y0)
        {
            float[] dist = new float[3];
			dist [0] = x0;
			dist[1] = y0;

			float moveByX = 50f;
			float moveByY = 35f;

			for (int iC = 0; iC < sentenceConcepts.Count; ++iC) {
				int i = (int)sentenceConcepts [iC];
				if (i >= 0 && i < 8) {
					Concept c = Concept.getConceptByID (sentenceConcepts [iC]);
					dist [0] = c.posX;
					dist [1] = c.posY;
				}
			}

			for (int iC = 0; iC < sentenceConcepts.Count; ++iC) {
				switch (sentenceConcepts [iC]) {
				case concept_ID.MOD_LESS:
					moveByX /= 2;
					moveByY /= 2;
					break;
				case concept_ID.MOD_MORE:
					moveByX *= 2;
					moveByY *= 2;
					break;
				}
			}
			for (int iC = 0; iC < sentenceConcepts.Count; ++iC) {
				switch (sentenceConcepts [iC]) {
				case concept_ID.POS_UP:
					dist [1] += moveByY;
					break;
				case concept_ID.POS_DOWN:
					dist [1] -= moveByY;
					break;
				case concept_ID.POS_LEFT:
					dist [0] -= moveByX;
					break;
				case concept_ID.POS_RIGHT:
					dist [0] += moveByX;
					break;
				}
			}

			if (dist [0] != x0 || dist [1] != y0)
				dist [2] = 1f;
			else
				dist [2] = 0f;
            return dist;
        }
    }
}