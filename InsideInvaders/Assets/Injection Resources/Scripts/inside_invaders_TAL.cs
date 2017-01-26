using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace inside_invaders_TAL
{
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
		ORG_EPAULE_DRIOTE,
		ORG_EPAULE_GAUCHE,
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
        MOD_AGAIN
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

        public Concept(concept_ID ID, string word)
        {
            this.ID = ID;
            word_asso = new List<string>();
            word_asso.Add(word);
            posX = posY = 0f;
        }

        public Concept(concept_ID ID, string word, float posX, float posY)
        {
            this.ID = ID;
            word_asso = new List<string>();
            word_asso.Add(word);
            this.posX = posX;
            this.posY = posY;
        }

        // liste de tous les concepts
        public static List<Concept> concepts;

        // ajout a la liste des concepts
        public static void addToConcepts(concept_ID ID, string word)
        {
            for(int iC=0; iC<concepts.Count; ++iC)
            {
                if(concepts[iC].ID == ID)
                {
                    if (concepts[iC].word_asso.Contains(word))
                        return;
                    concepts[iC].word_asso.Add(word);
                    return;
                }
            }
            concepts.Add(new Concept(ID, word));
        }

        public static void addToConcepts(concept_ID ID, string word, float posX, float posY)
        {
            for (int iC = 0; iC < concepts.Count; ++iC)
            {
                if (concepts[iC].ID == ID)
                {
                    if (concepts[iC].word_asso.Contains(word))
                        return;
                    concepts[iC].word_asso.Add(word);
                    return;
                }
            }
            concepts.Add(new Concept(ID, word, posX, posY));
        }

        // initialisation de la liste des concepts
        public static void concepts_init()
        {
			concepts = new List<Concept>();

			addToConcepts(concept_ID.ORG_COEUR, "coeur", 1405f, 520f);
			addToConcepts(concept_ID.ORG_POUMON_GAUCHE, "poumon", 1515f, 505f);
			addToConcepts(concept_ID.ORG_POUMON_DROIT, "poumon", 1285f, 480f);
			addToConcepts(concept_ID.ORG_FOIE, "foie", 1335f, 375f);
			addToConcepts(concept_ID.ORG_ESTOMAC, "estomac", 1465f, 360f);
			addToConcepts(concept_ID.ORG_INTESTIN_GRELE, "intestin", 1425f, 215f);
			addToConcepts(concept_ID.ORG_GROS_INTESTIN, "intestin", 1280f, 215f);
			addToConcepts(concept_ID.ORG_GORGE, "gorge", 1390f, 725f);
			addToConcepts(concept_ID.ORG_AORTE, "aorte", 1390f, 615f);

            addToConcepts(concept_ID.PLAYER, "je");
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
	        addToConcepts(concept_ID.MOD_AGAIN, "encore");

			addToConcepts(concept_ID.ORG_POUMON_GAUCHE, "gauche");
			addToConcepts(concept_ID.ORG_POUMON_DROIT, "droit");
			addToConcepts(concept_ID.ORG_INTESTIN_GRELE, "grele");
			addToConcepts(concept_ID.ORG_GROS_INTESTIN, "gros");
        }

        // recupere les concepts d'une phrase du joueur
        public static List<concept_ID> getConcepts(string sentence)
        {
            List<concept_ID> sentenceConcepts = new List<concept_ID>();
            string[] words = sentence.ToLower().Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
			string[] words2 = new string[words.Length];
			for(int iW=0; iW<words.Length; ++iW)
				words2[iW] = Regex.Replace(words[iW], "[^A-Za-z0-9]", "");
			foreach (Concept c in concepts) {
				bool containsAll = true;
				foreach (string wa in c.word_asso) {
					bool contains = false;
					foreach (string w in words2)
						if (w == wa)
							contains = true;
					if (!contains)
						containsAll = false;
				}
				if(containsAll)
					sentenceConcepts.Add(c.ID);
			}
            /*foreach(string w in words2)
                foreach (Concept c in concepts)
                {
                    if (c.word_asso.Contains(w))
                        sentenceConcepts.Add(c.ID);
                }*/
            return sentenceConcepts;
        }

		// retrouve le concept d'un ID dans la liste
		public static Concept getConceptByID(concept_ID ID){
			foreach (Concept c in concepts)
				if (c.ID == ID)
					return c;
			return null;
		}
    }

    // reponse du bot
    public class Response
    {
        // texte et concepts de la reponse
        public string text;
        public List<concept_ID> responseConcepts;

        public Response(string text, concept_ID ID)
        {
            this.text = text;
            responseConcepts = new List<concept_ID>();
            responseConcepts.Add(ID);
        }

        // liste des reponses possibles
        public static List<Response> responses;

        // ajout d'un concept a une reponse
        public static void addToResponses(string text, concept_ID ID)
        {
            for(int iR=0; iR<responses.Count; ++iR)
            {
                if(responses[iR].text == text)
                {
                    if (responses[iR].responseConcepts.Contains(ID))
                        return;
                    responses[iR].responseConcepts.Add(ID);
                    return;
                }
            }
            responses.Add(new Response(text, ID));
        }

        // initialisaiton de la liste des reponses
        public static void responses_init()
        {
            responses = new List<Response>();


        }

        // choisit la reponse la plus pertinente par rapport a une phrase de l'utilisateur
        public static string getBestResponse(List<concept_ID> sentenceConcepts, bool hasMoved)
        {
            if (hasMoved)
                return "Ici?";
            float[] responseConceptCount = new float[responses.Count];
			for (int iR = 0; iR < responses.Count; ++iR) {
				responseConceptCount [iR] = 0f;
				foreach (concept_ID c in sentenceConcepts)
					if (responses [iR].responseConcepts.Contains (c))
						++responseConceptCount [iR];
			}
            float max = 0f;
            int iMax = -1;
            for (int iR = 0; iR < responses.Count; ++iR)
                if (responseConceptCount[iR] > max)
                {
                    max = responseConceptCount[iR];
                    iMax = iR;
                }
            if (iMax > -1)
                return responses[iMax].text;
            return "Je n'ai pas compris.";
        }

        // decide le mouvement du viseur
		public static float[] getMovement(List<concept_ID> sentenceConcepts, float x0, float y0)
        {
            float[] dist = new float[3];
			dist [0] = x0;
			dist[1] = y0;

			float moveByX = 50;
			float moveByY = 35f;

			for (int iC = 0; iC < sentenceConcepts.Count; ++iC) {
				int i = (int)sentenceConcepts [iC];
				if (i >= 0 && i < 9) {
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
				dist [2] = 1;
			else
				dist [2] = -1;
            return dist;
        }
    }
}