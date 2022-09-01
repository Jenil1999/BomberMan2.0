using UnityEngine;
using UnityEngine.UI;

public class ShowPopUp: MonoBehaviour {
	[SerializeField] string _setTitle;
	[SerializeField] string _setMsg;
	[SerializeField] string _setLButton;
	[SerializeField] string _setRButton;
	[SerializeField] Button showPopUpButton;
	[SerializeField] Button HidePopUPButton;
	bool Clicked = false;

    private void Update()
	{
		ShowPopup();
	 	HidePopUp();
	}
    
	void ShowPopup()
    {
		showPopUpButton.onClick.RemoveAllListeners();  //for Resolve MemoryLeak Issue...& also Avoid Assign Event more than one...
		showPopUpButton.onClick.AddListener(() => {
			if (string.IsNullOrEmpty(_setLButton) && !string.IsNullOrEmpty(_setRButton))
			{
				PopUp.Instance.LeftButton.gameObject.SetActive(false);
				PopUp.Instance.RightButton.gameObject.SetActive(true);
				Debug.Log("AddLeft");
			}

			else if(string.IsNullOrEmpty(_setRButton) && !string.IsNullOrEmpty(_setLButton))
			{
				PopUp.Instance.RightButton.gameObject.SetActive(false);
				PopUp.Instance.LeftButton.gameObject.SetActive(true);
				Debug.Log("AddRight");
			}

			else if(!string.IsNullOrEmpty(_setLButton) && !string.IsNullOrEmpty(_setRButton))
            {
				PopUp.Instance.LeftButton.gameObject.SetActive(true);
				PopUp.Instance.RightButton.gameObject.SetActive(true);
			}

			else 
            {
				PopUp.Instance.LeftButton.gameObject.SetActive(false);
				PopUp.Instance.RightButton.gameObject.SetActive(false);
			}

			//Debug.Log("Show PopUp Window");
			PopUp.Instance.SetTitle(_setTitle)
				.SetMsg(_setMsg)
				.SetLeftButtonText(_setLButton)
				.SetRightButtonText(_setRButton)
				//.OnPositiveButtonClick(()=> Debug.Log("PositiveResponse"))
				//.OnNagetiveButtonClick(() => Debug.Log("NagetiveResponse"))
				.Show((CallBack) => {

					if(CallBack)
                    {
						Debug.Log("PositiveResponce");
                    }

					else
                    {
						Debug.Log("NagetiveResponce");
                    }
				});
			
			Clicked = true;
		});
	}
    void HidePopUp()
    {
		if (Clicked)
		{
			HidePopUPButton.onClick.RemoveAllListeners();
			HidePopUPButton.onClick.AddListener(() =>
			{
				PopUp.Instance.Hide();
				//Debug.Log("HidePopUp");
				Clicked = false;
			});
		}
	}

	


}
