/*
Historically accurate educational video game based in 1830s Lausanne.
Copyright (C) 2021  GameLab UNIL-EPFL

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using Godot;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

public struct CharacterInfo_t {
	public CharacterInfo_t(
		string pnm="", string nm="", string addr="", int numb=0,
		string cnjt="", int enfts=0, string mtr=""
	) {
		prenom = pnm;
		nom = nm;
		adresse = addr;
		num = numb;
		conjoint = cnjt;
		enfants = enfts;
		metier = mtr;
		valid = true;
	}
	public CharacterInfo_t(int _illegalVal) {
		prenom = "";
		nom = "";
		adresse = "";
		num = _illegalVal;
		conjoint = "";
		enfants = _illegalVal;
		metier = "";
		valid = false;
	}
	public bool valid;
	
	public string prenom;
	public string nom;
	public string adresse;
	public int num;
	public string conjoint;
	public int enfants;
	public string metier;

	public void _ClearEntry(int i) {
		switch(i) {
			case 0: 
				prenom = "";
				break;
			case 1:
				nom = "";
				break;
			case 2:
				adresse = "";
				break;
			case 3:
				num = 0;
				break;
			case 4:
				conjoint = "";
				break;
			case 5:
				enfants = -0;
				break;
			case 6:
				metier = "";
				break;
			case -1:
				valid = false;
				break;
			default:
				throw new IndexOutOfRangeException();
		}
	}
	
	public bool IsValid() {
		return valid;
	}
};

//////////////////////////////////////////////////////////////////////////////
/////////////////////////////////InfoValue_t//////////////////////////////////
//////////////////////////////////////////////////////////////////////////////

public struct InfoValue_t {
	public InfoValue_t(bool initVal0, bool initVal1, bool initVal2, 
						bool initVal3, bool initVal4, bool initVal5, bool initVal6, bool initValid=false) {
		prenomCorrect = initVal0;
		nomCorrect = initVal1;
		adresseCorrect = initVal2;
		numCorrect = initVal3;
		conjointCorrect = initVal4;
		enfantsCorrect = initVal5;
		metierCorrect = initVal6;
		valid = initValid;
	}
	
	public InfoValue_t(bool initVal) {
		prenomCorrect = initVal;
		nomCorrect = initVal;
		adresseCorrect = initVal;
		numCorrect = initVal;
		conjointCorrect = initVal;
		enfantsCorrect = initVal;
		metierCorrect = initVal;
		valid = false;
	}
	public bool valid;
	public bool prenomCorrect;
	public bool nomCorrect;
	public bool adresseCorrect;
	public bool numCorrect;
	public bool conjointCorrect;
	public bool enfantsCorrect;
	public bool metierCorrect;

	// Indexer for the struct values
	public bool this[int i] {
		get {
			switch(i) {
				case 0: 
					return prenomCorrect;
				case 1:
					return nomCorrect;
				case 2:
					return adresseCorrect;
				case 3:
					return numCorrect;
				case 4:
					return conjointCorrect;
				case 5:
					return enfantsCorrect;
				case 6:
					return metierCorrect;
				case -1:
					return valid;
				default:
					throw new IndexOutOfRangeException();
			}
		}
		set {
			switch(i) {
				case 0: 
					prenomCorrect = value;
					break;
				case 1:
					nomCorrect = value;
					break;
				case 2:
					adresseCorrect = value;
					break;
				case 3:
					numCorrect = value;
					break;
				case 4:
					conjointCorrect = value;
					break;
				case 5:
					enfantsCorrect = value;
					break;
				case 6:
					metierCorrect = value;
					break;
				case -1:
					valid = value;
					break;
				default:
					throw new IndexOutOfRangeException();
			}
		}
	}
	
	public bool IsCorrect() {
		return prenomCorrect && nomCorrect && adresseCorrect &&
			numCorrect && conjointCorrect && enfantsCorrect && metierCorrect;
	}

	public InfoValue_t Or(InfoValue_t other) {
		return new InfoValue_t(
			prenomCorrect || other.prenomCorrect,
			nomCorrect || other.nomCorrect,
			adresseCorrect || other.adresseCorrect,
			numCorrect || other.numCorrect,
			conjointCorrect || other.conjointCorrect,
			enfantsCorrect || other.enfantsCorrect,
			metierCorrect || other.metierCorrect,
			valid || other.valid
		);
	}
	
	public bool IsValid() {
		return valid;
	}
	
	// Returns the attribute names of all attributes that were found
	public List<string> FoundAttributes() {
		List<string> res = new List<string>();
		if(prenomCorrect) {
			res.Add("prenom");
		}
		if(nomCorrect) {
			res.Add("nom");
		}
		if(adresseCorrect) {
			res.Add("adresse");
		}
		if(numCorrect) {
			res.Add("num");
		}
		if(conjointCorrect) {
			res.Add("conjoint");
		}
		if(enfantsCorrect) {
			res.Add("enfants");
		}
		if(metierCorrect) {
			res.Add("metier");
		}
		return res;
	}
	
	public List<string> Outliers() {
		List<string> res = new List<string>();
		if(!prenomCorrect) {
			res.Add("prenom");
		}
		if(!nomCorrect) {
			res.Add("nom");
		}
		if(!adresseCorrect) {
			res.Add("adresse");
		}
		if(!numCorrect) {
			res.Add("num");
		}
		if(!conjointCorrect) {
			res.Add("conjoint");
		}
		if(!enfantsCorrect) {
			res.Add("enfants");
		}
		if(!metierCorrect) {
			res.Add("metier");
		}
		return res;
	}
};

/////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////XMLAttributeInfo_t//////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////

public struct XMLAttributeInfo_t {
	public XMLAttributeInfo_t(string _name, string _val) {
		name = _name;
		val = _val;
	}
	
	public string name;
	public string val;
};

///////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////QUEST_CONTROLLER//////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////

public class QuestController : Node {

	[Signal]
	public delegate void EndQuest(Quests q);
	
	public static int TALK_TO_QUEST_NPC_OBJECTIVE = -1;
	public static int OPEN_NOTEBOOK_OBJECTIVE = 0;
	public static int CONFIRM_OPEN_NOTEBOOK_OBJECTIVE = 1;
	public static int COMPLETE_PAGE_OBJECTIVE = 2;

	//File at which the scene's dialogue is stored
	[Export]
	public string SceneCharacterFileName = "/characters/notebookCharacterList.xml";
	public string SceneCharacterFileBasePath = "res://db/";
	public string SceneCharacterFilePath;
	
	//Files related to the Quest dialogue
	public string QuestDialogueFile;
	
	//Local XDocument containing a parsed version of the dialogue
	private XDocument characterList;
	private XDocument QuestDialogue;
	private XDocument notebookInfo;
		
	//Buffer storing the dialogue being used by a questNPC
	private Stack<string> QuestBuffer = new Stack<string>();
	
	//Id to track quest progression
	private int QuestDialogueId = 0;
	private int QuestDialogueIdOnObjectiveSuccess = -1;
	
	private Context context;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		context = GetNode<Context>("/root/Context");

		QuestDialogueFile = "res://db/" + context._GetLanguageAbbrv() + "/dialogues/xml/QuestNPC.xml";
		
		SceneCharacterFilePath = SceneCharacterFileBasePath + 
			context._GetLanguageAbbrv() + SceneCharacterFileName;
		DialogueController._ParseXML(ref characterList, SceneCharacterFilePath);
		DialogueController._ParseXML(ref QuestDialogue, QuestDialogueFile);
		DialogueController._ParseXML(ref notebookInfo, SceneCharacterFileBasePath + context._GetLanguageAbbrv() + SceneCharacterFileName);
		

		if(context._GetLocation() == Locations.INTRO) {
			Notebook NB = GetNode<Notebook>("../Notebook");
			NB.Connect("CloseNotebook", this, "_On_Notebook_Close");
			NB.Connect("PageComplete", this, "_On_Notebook_Page_End");
		}
	}
	
	public void _InitBuffer(string[] text) {
		for(int i = 0; i < text.Length; ++i) {
			QuestBuffer.Push(text[text.Length - i - 1]);
		}
	}
	
	public string _NextLine() {
		if(QuestBuffer.Count == 0) {
			return null;
		}
		return QuestBuffer.Pop();
	}
	
	public InfoValue_t _CompareCharInfo(CharacterInfo_t solution, CharacterInfo_t val) {
		InfoValue_t res = new InfoValue_t(
			solution.prenom.Equals(val.prenom),
			solution.nom.Equals(val.nom),
			solution.adresse.Equals(val.adresse),
			solution.num == val.num,
			solution.conjoint.Equals(val.conjoint),
			solution.enfants == val.enfants,
			solution.metier.Equals(val.metier)
		);
		
		return res;
	}
	
	/**
	 * @brief Queries the local XDocument for a given solution
	 * @param tabId, the id of the tab for which we want a solution
	 * @param l, the language in which the solution should be
	 * @return a CharacterInfo_t struct containing all of the solution data
	 */
	public CharacterInfo_t _QueryQuestSolution(int tabId) {
		//Query a new solution from the notbookInfo
		var querySolution = from solution in notebookInfo.Root.Descendants("solution")
							where Int32.Parse(solution.Attribute("id").Value) == tabId
							select solution.Attributes();
		
		// Extract attribute name and value
		var solutionList = querySolution.Select(x => 
			x.Select(y => new XMLAttributeInfo_t(y.Name.LocalName, y.Value))
		);
		
		CharacterInfo_t res = new CharacterInfo_t(-1);
		
		foreach(var e in solutionList) {
			foreach(var xmlAttr in e) {
				var inf = xmlAttr.val;
				switch(xmlAttr.name) {
					case "prenom":
						res.prenom = inf;
						break;
					case "nom":
						res.nom = inf;
						break;
					case "adresse":
						res.adresse = inf;
						break;
					case "num":
						res.num = Int32.Parse(inf);
						break;
					case "conjoint":
						res.conjoint = inf;
						break;
					case "enfant":
						res.enfants = Int32.Parse(inf);
						break;
					case "metier":
						res.metier = inf;
						break;
					default:
						break;
				}
			}
		}
		
		return res;
	}

	/**
	 * @brief Queries the local XDocument for a given solution
	 * @param tabId, the id of the tab for which we want a solution
	 * @param l, the language in which the solution should be
	 * @return a CharacterInfo_t struct containing all of the solution data
	 */
	public static CharacterInfo_t _QueryQuestSolution(int tabId, Language l) {
		XDocument xmlDB = null;
		string path = string.Format("res://db/{0}/characters/notebookCharacterList.xml", Context._GetLanguageAbbrv(l));
		DialogueController._ParseXML(
			ref xmlDB,
			path
		);

		//Query a new solution from the notbookInfo
		var querySolution = from solution in xmlDB.Root.Descendants("solution")
							where Int32.Parse(solution.Attribute("id").Value) == tabId
							select solution.Attributes();
		
		// Extract attribute name and value
		var solutionList = querySolution.Select(x => 
			x.Select(y => new XMLAttributeInfo_t(y.Name.LocalName, y.Value))
		);
		
		CharacterInfo_t res = new CharacterInfo_t(-1);
		
		foreach(var e in solutionList) {
			foreach(var xmlAttr in e) {
				var inf = xmlAttr.val;
				switch(xmlAttr.name) {
					case "prenom":
						res.prenom = inf;
						break;
					case "nom":
						res.nom = inf;
						break;
					case "adresse":
						res.adresse = inf;
						break;
					case "num":
						res.num = Int32.Parse(inf);
						break;
					case "conjoint":
						res.conjoint = inf;
						break;
					case "enfant":
						res.enfants = Int32.Parse(inf);
						break;
					case "metier":
						res.metier = inf;
						break;
					default:
						break;
				}
			}
		}
		
		return res;
	}
	
	public void _StartQuest(Quests q) {
		//Only start an available quest
		if(context._GetQuest() == q && context._GetQuestStatus() == QuestStatus.NOT_STARTED) {
			context._UpdateQuestStatus(QuestStatus.ON_GOING);
		}
	}
	
	public void _EndQuest(Quests q) {
		//Only end an ongoing quest
		if(context._GetQuest() == q && context._GetQuestStatus() == QuestStatus.ON_GOING) {
			context._UpdateQuestStatus(QuestStatus.COMPLETE);
		}
	}

	private bool CheckObjective(int objId) {
		return context._GetQuestStateId() >= objId;
	}
	
	private string TutorialQuest() {
		//Check current dialogue state
		var dialogueQuery = from qd in QuestDialogue.Root.Descendants("text")
							where int.Parse(qd.Attribute("id").Value) == QuestDialogueId
							select qd;

		//Check for successful objective to avoid repitition
		if(QuestDialogueIdOnObjectiveSuccess >= QuestDialogueId) {
			var successDialogueQuery = from sq in QuestDialogue.Root.Descendants("text")
								   where int.Parse(sq.Attribute("id").Value) == QuestDialogueIdOnObjectiveSuccess
								   select sq;

			//Check for successful objective
			foreach(XElement txt in successDialogueQuery) {
				if(txt.Attribute("signal") != null) {
					EmitSignal(txt.Attribute("signal").Value, Quests.TUTORIAL);
					_EndQuest(Quests.TUTORIAL);
					return null;
				}

				if(txt.Attribute("objectif") != null) {
					int objId = int.Parse(txt.Attribute("objectif").Value);

					//Check if the objective was met
					if(CheckObjective(objId)) {
						//Update the QuestDialogueId
						QuestDialogueId = QuestDialogueIdOnObjectiveSuccess + 1;

						//If so return the bojective text
						return txt.Value;
					} 
				}
			}
		}
		
		//Check for objective or replay attributes
		foreach(XElement txt in dialogueQuery) {
			if(txt.Attribute("signal") != null) {
				EmitSignal(txt.Attribute("signal").Value, Quests.TUTORIAL);
				_EndQuest(Quests.TUTORIAL);
				return null;
			}

			//Update the QuestDialogueId
			QuestDialogueId++;

			if(txt.Attribute("objectif") != null) {
				int objId = int.Parse(txt.Attribute("objectif").Value);

				//Check if the objective was met
				if(!CheckObjective(objId)) {
					//Find the replay Id
					if(txt.Attribute("replay") != null) {
						//Store current id in case of quest id
						QuestDialogueIdOnObjectiveSuccess = int.Parse(txt.Attribute("id").Value);
						//Set next id to the replay id
						QuestDialogueId = int.Parse(txt.Attribute("replay").Value);
					}
					//End conversation for now
					return null;
				} 
			}

			//Return the obtained string
			return txt.Value;
		}
		//if no text obtained in query, just return null
		return null;
	}
	
	public string _QuestInteraction() {
		//Check for an on-going quest
		if(context._GetQuestStatus() == QuestStatus.ON_GOING) {
			switch(context._GetQuest()) {
				case Quests.TUTORIAL:
					return TutorialQuest();
				default:
					return null;
			}
		}
		return null;
	}

	public void _On_Notebook_Close() {
		if(context._GetQuestStatus() == QuestStatus.ON_GOING) {
			if(context._GetQuest() == Quests.TUTORIAL) {
				//Check for progress
				int id = context._GetQuestStateId();
				//First stage of the tutorial is done
				context._UpdateQuestStateId((id < OPEN_NOTEBOOK_OBJECTIVE) ? OPEN_NOTEBOOK_OBJECTIVE : id);
			}
		}
	}
	
	//Triggers objective update
	public void _On_Notebook_Page_End() {
		//Check for correct quest
		if(context._GetQuestStatus() == QuestStatus.ON_GOING) {
			if(context._GetQuest() == Quests.TUTORIAL) {
				//Check for progress
				int id = context._GetQuestStateId();
				//Second stage of the tutorial is done
				context._UpdateQuestStateId((id < COMPLETE_PAGE_OBJECTIVE) ? COMPLETE_PAGE_OBJECTIVE : id);
			}
		}
	}
}
