using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour {
	[System.Serializable]
	public class ShopItem{
		public GameObject item;
		public string name;
		public string description;
		public int price;
		public Vector3 rot;
		public GameObject buy_effect;
	}

	public enum SHOPSTATE
	{
		SHOPPING = 0,
		OPENINGCRATE,
		CRATEOPEN
	}
	private SHOPSTATE shopState;

	public List<ShopItem> shopItems;
	private int shopIndex;

	private GameObject currShopItem;
	public Text itemName;
	public Text itemDesc;
	public Text itemPrice;
	public Text overallScoreText;
	public float crateTimer;

	public Text buyTxt;
	public GameObject buyEffect;
	public GameObject buyEffectInstance;
	private float buyEffectEndTime;
	private float buyEffectTimer;

	public Canvas m_cShopCanvas;
	public Canvas m_cOpenCrateCanvas;
	public Text m_cRewardName;
	public Text m_cRewardDescription;

	private ShopCrate.Reward openedReward;

	// Use this for initialization
	void Start () {
		shopIndex = 0;
		ChangeShopItem (shopIndex);
		overallScoreText.text = PlayerPrefsManager.Get().TotalScore + "";
		buyEffectTimer = 0f;
		shopState = SHOPSTATE.SHOPPING;
		crateTimer = 0f;
		buyEffectEndTime = 4f;
		m_cOpenCrateCanvas.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		if (currShopItem != null) {
			currShopItem.transform.RotateAround (currShopItem.transform.position, new Vector3 (0, 0, 1), 20f * Time.deltaTime);
		}
		switch (shopState) {
		case SHOPSTATE.SHOPPING:
			break;
		case SHOPSTATE.OPENINGCRATE:
			buyEffectTimer += Time.deltaTime;
			if (buyEffectTimer > buyEffectEndTime) {
				shopState = SHOPSTATE.CRATEOPEN;
				ShopCrate.Reward reward = currShopItem.GetComponent<ShopCrate> ().GetRandomReward ();
				string effect = currShopItem.GetComponent<ShopCrate> ().ExecuteRewardFunc (reward);

				Destroy (buyEffectInstance);
				Destroy (currShopItem);
				ChangeShopView (false, true);
				openedReward = reward;
				currShopItem = Instantiate (reward.obj, transform.position, reward.obj.transform.localRotation, transform);
				m_cRewardName.text = reward.name;
				m_cRewardDescription.text = reward.description;
				buyEffectTimer = 0f;

				//simple, add on some score
				overallScoreText.text = PlayerPrefsManager.Get().TotalScore + "";
				m_cRewardDescription.text += "\n " + effect;
			}
			break;
		case SHOPSTATE.CRATEOPEN:
			//nothing
			break;
		}
	}

	public void ChangeShopView(bool shopCanvasVisible, bool rewardCanvasVisible){
		m_cShopCanvas.enabled = shopCanvasVisible;
		m_cOpenCrateCanvas.enabled = rewardCanvasVisible;
	}

	private void ChangeShopItem(int index){
		if(currShopItem != null)
			Destroy (currShopItem);

		if (index > shopItems.Count)
			return;

		currShopItem = Instantiate (shopItems [index].item, transform.position, new Quaternion(0, 0, 0, 1), transform);
		currShopItem.transform.RotateAround (currShopItem.transform.position, new Vector3 (1, 0, 0), shopItems[shopIndex].rot.x);
		currShopItem.transform.RotateAround (currShopItem.transform.position, new Vector3 (0, 1, 0), shopItems[shopIndex].rot.y);
		currShopItem.transform.RotateAround (currShopItem.transform.position, new Vector3 (0, 0, 1), shopItems[shopIndex].rot.z);

		itemName.text = shopItems [index].name;
		itemDesc.text = shopItems [index].description;
		itemPrice.text = shopItems [index].item.GetComponent<ShopCrate>().value + "";
	}

	public int GetCurrentShopIndex(){
		return shopIndex;
	}

	public void ChangeIndex(int i){
		shopIndex += i;

		if (shopIndex >= shopItems.Count)
			shopIndex = 0;

		if (shopIndex < 0)
			shopIndex = shopItems.Count;

		ChangeShopItem (shopIndex);
	}

	public ShopItem GetCurrentShopItem(){
		return shopItems [shopIndex];
	}

	public void BuyShopItem(){
		ShopItem si = shopItems [shopIndex];
		int points = PlayerPrefsManager.Get().TotalScore;

		if (points < si.price)
			return;

		PlayerPrefsManager.Get().AddScore(-si.price);
		overallScoreText.text = PlayerPrefsManager.Get().TotalScore + "";
		buyEffectTimer = 0f;
		shopState = SHOPSTATE.OPENINGCRATE;
		//disable canvas now
		buyEffectInstance = Instantiate (buyEffect, transform.position, buyEffect.transform.localRotation, transform);
		ChangeShopView (false, false);
	}

	public void BackToMainShop(){
		m_cOpenCrateCanvas.enabled = false;
		m_cShopCanvas.enabled = true;
		m_cRewardName.text = m_cRewardDescription.text = "";
		openedReward = null;
		ChangeShopItem (shopIndex);
	}
}
