using UnityEngine;
using Parse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.UI;

public class ParseScript : MonoBehaviour {

	// Use this for initialization
	ParseObject newUser,currentUser;
	ParseUser tempUser = new ParseUser();
	IEnumerable<ParseObject> matchingUsers,totalUsers;

	Text usernameText, passwordText,pwCheckText,loginUsernameText,loginPasswordText,messageText,currencyText;
	GameObject usernameTextObj, passwordTextObj,pwCheckTextObj,loginPasswordTextObj,loginUsernameTextObj,messageTextObj,currencyTextObj;

	int GoldTemp;
	int Gold;
	string message;
	public bool isAuthenticated;
	bool setPlayerPrefs = false;
	int currency = 0;

	//List<ParseUser> matchingUserList;
	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
		//ParseUser.LogOutAsync();
		//currentUser = new ParseObject("GameVariables");
		messageTextObj = GameObject.Find("MessageText");
		usernameTextObj = GameObject.Find("UsernameSignUpText");
		passwordTextObj = GameObject.Find("PasswordSignUpText");
		loginUsernameTextObj = GameObject.Find("UsernameLogInText");
		loginPasswordTextObj = GameObject.Find("PasswordLogInText");
		currencyTextObj = GameObject.Find("CurrencyText");
		pwCheckTextObj = GameObject.Find("PWCheckText");
		usernameText = usernameTextObj.GetComponent<Text>();
		passwordText = passwordTextObj.GetComponent<Text>();
		pwCheckText = pwCheckTextObj.GetComponent<Text>();
		loginUsernameText = loginUsernameTextObj.GetComponent<Text>();
		loginPasswordText = loginPasswordTextObj.GetComponent<Text>();
		messageText = messageTextObj.GetComponent<Text>();
		currencyText = currencyTextObj.GetComponent<Text>();
		GameObject.Find("LogInPanel").SetActive(false);
		GameObject.Find("SignUpPanel").SetActive(false);
		if (ParseUser.CurrentUser != null)
		{
		    // Once user has been authenticated do stuff
			setPlayerPrefs = true;
			//UpdateUser();
			message = "Welcome! " + ParseUser.CurrentUser["username"];
		}
		else
		{
			//if(ParseUser.CurrentUser == null)
			//{
			//	Application.LoadLevel("LogIn");
			//}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		messageText.text = message;
		
		if(setPlayerPrefs == true)
		{
			SetPlayerPrefs();
		}
		if(ParseUser.CurrentUser != null)
		{
			currencyText.text = "Example Currency: " + currency;
		}
		else
		{
			ClearMessage();
		}
	}



	public void SetPlayerPrefs()
	{
		currency = ParseUser.CurrentUser.Get<int>("Currency");
		setPlayerPrefs = false;
	}
	public void CheckUsername()
	{
		message = "Checking Username...";
		ParseUser.Query.WhereEqualTo("username", usernameText.text).FirstAsync().ContinueWith(t =>
		{
			if(t.IsFaulted || t.IsCanceled)
			{
				message = "Username Avalilable!";
				if(pwCheckText.text == passwordText.text)
				{
					ParseUser user = new ParseUser()
					{
					    Username = usernameText.text,
					    Password = passwordText.text,
					    //Email = emailText.text
					};
					user["Currency"] = 0;
					user.SignUpAsync().ContinueWith(s =>
					{
					    if (s.IsFaulted || s.IsCanceled)
					    {
					        // The login failed. Check the error to see why.
					        message = "Sign Up Failed!\nUsername Taken";
					    }
					    else
					    {
					    	Debug.Log("Signed Up!");
							message = "Signed Up!\nLogging in...";
					        // Login was successful.
					        message = "Welcome! " + ParseUser.CurrentUser["username"];
					        setPlayerPrefs = true;
					    }
					});
				}
				else
				{
					message = "Password doesn't match, make sure you entered it correctly.";
				}

			}
			else
			{
				message = "Username Taken!";
			}
		});
	}
	public void CreateNewUser()
	{
		ParseUser user = new ParseUser()
		{
		    Username = usernameText.text,
		    Password = passwordText.text,
		    //Email = emailText.text
		};
		user.SignUpAsync().ContinueWith(t =>
		{
		    if (t.IsFaulted || t.IsCanceled)
		    {
		        // The login failed. Check the error to see why.
		        message = "Sign Up Failed!\nUsername Taken";
		    }
		    else
		    {
		    	Debug.Log("Signed Up!");
				message = "Signed Up!\nLogging in...";
		        // Login was successful.
		    }
		});
	}

	public void authenticateUser()
	{
		
		ParseUser.LogInAsync(loginUsernameText.text, loginPasswordText.text).ContinueWith(t =>
		{
		    if (t.IsFaulted || t.IsCanceled)
		    {
		        // The login failed. Check t.Exception to see why.
				isAuthenticated = false;
				message = "Login Failed! Check your internet connection or Username/Password";
				Debug.Log("Login Failed!");
		    }
		    else
		    {
		        // Login was successful.
				isAuthenticated = true;
				setPlayerPrefs = true;
				message = "Login Successful!";
				Debug.Log("Login Successful");
				message = "Welcome! " + ParseUser.CurrentUser["username"];
				//Application.LoadLevel("MainMenu");
		    }
		});
	}

	public void LogOut()
	{
		ParseUser.LogOutAsync();
	}

	public void UpdateUser()
	{
		// Now let's update it with some new data.  In this case, only cheatMode
		// and score will get sent to the cloud.  playerName hasn't changed.
	    //Debug.Log(ParseUser.CurrentUser.IsAuthenticated);
	    ParseUser.CurrentUser["Currency"] = currency;
	    try
	    {
			ParseUser.CurrentUser.SaveAsync().ContinueWith(t =>
			{
				
				if(t.IsFaulted || t.IsCanceled)
				{
					Debug.Log("Couldn't Update User!");
				}
				else
				{
					Debug.Log("Save Successful!");
					ParseUser.CurrentUser.SaveAsync();
				}

			});
	    }
	    catch(Exception e)
	    {
	    	Debug.Log("Broke It!");
	    }

	}
	public void ClearMessage()
	{
		message = "";
		currencyText.text = "";
	}

	public void GiveCurrency()
	{
		currency += 50;
		UpdateUser();
	}
}
