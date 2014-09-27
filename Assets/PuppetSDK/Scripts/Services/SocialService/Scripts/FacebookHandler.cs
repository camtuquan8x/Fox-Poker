using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puppet.Services
{
	public sealed class FacebookHandler : SocialNetwork, ISocialNetwork
	{
		public Dictionary<string, object> friendsDictionary;
	    SocialType type = SocialType.Facebook;

	    public override string AccessToken
	    {
	        get { return FB.AccessToken; }
	    }

	    public override bool IsLoggedIn
	    {
	        get { return FB.IsLoggedIn; }
	    }

	    public override void SocialInit()
	    {
	        FB.Init(() => OnInit(), (bool isGameShown) => { if (!isGameShown) Time.timeScale = 0; else Time.timeScale = 1; });
	    }

	    public override void SocialLogin()
	    {
	        FB.Login("email,publish_actions", (FBResult response) => {
	            OnLoginCompleted(IsLoggedIn); 
	        });
	    }

	    public override void SocialLogout()
	    {
	        if (IsLoggedIn)
	            FB.Logout();
	    }

	    public override void checkPublishPermission(Action<bool> onGetFinishPermission)
	    {
	        if (UserId != null)
	        {
	            Debug.Log("Social id " + UserId);
	            FB.API(UserId + "/permissions", Facebook.HttpMethod.GET, getPermissionFacebookCallback);
	            this.callbackGetPermission = onGetFinishPermission;
	        }
	        else
	            onGetFinishPermission(false);
	    }
	    Action<bool> callbackGetPermission;
	    void getPermissionFacebookCallback(FBResult response)
	    {
	        bool hasPermission = false;
	        if (response != null)
	        {
	            if (!string.IsNullOrEmpty(response.Text))
	            {
	                Dictionary<string, object> permission = (Dictionary<string, object>)JsonUtil.Deserialize(response.Text);
	                List<object> resultSet = permission["data"] as List<object>;
	                Dictionary<string, object> detail = resultSet[0] as Dictionary<string, object>;

	                if (detail.ContainsKey("publish_actions"))
	                    hasPermission = Convert.ToBoolean(detail["publish_actions"]);
	            }
	        }
	        else
	            Debug.Log("Fb response is null");

	        if (callbackGetPermission != null)
	            callbackGetPermission(hasPermission);
	    }

	    public override void Publish(string content, string url, Action<bool> onShareComplete)
	    { 
	        FB.Feed(link: url, linkDescription: content, callback: (FBResult result) => {
	            if (onShareComplete != null)
	                onShareComplete(string.IsNullOrEmpty(result.Error));
	        });
	    }

	    private void OnInit()
	    {
	        Debug.Log("FB Init completed");
	        SocialService.Instance.DispathInitComplete(type);
	        if (!string.IsNullOrEmpty(AccessToken))
	        {
	            getCurrentUser();
	        }
	    }

	    void OnLoginCompleted(bool isSuccess)
	    {
	        Debug.Log("FB Login: " + isSuccess);
	        if (isSuccess)
	        {
	            if (!string.IsNullOrEmpty(AccessToken))
	            {
	                getCurrentUser();
					getFBFriends();
	            }
	            else
	                SocialService.Instance.DispathAccessTokenNotFound(type);
	        }
	        else
	            SocialService.Instance.SetOnLoginComplete(type, false);

	    }

	    void getCurrentUser()
	    {
	        FB.API("me", Facebook.HttpMethod.GET, (FBResult response) =>
	        {
	            if (response != null && !string.IsNullOrEmpty(response.Text))
	            {
	                Dictionary<string, object> profile = (Dictionary<string, object>)JsonUtil.Deserialize(response.Text);
	                UserId = profile["id"].ToString();
	            }
	            SocialService.Instance.SetOnLoginComplete(type, true);
	        });
	    }
	       
		public void getFBFriends()
		{
			FB.API("me/friends", Facebook.HttpMethod.GET, (FBResult response) =>
			{
				if (response != null && !string.IsNullOrEmpty(response.Text))
				{
					Debug.Log ("#### FRIENDS " + response.Text);
					friendsDictionary = (Dictionary<string, object>)JsonUtil.Deserialize(response.Text);
				}

			});
		}

		public Dictionary<string, object> GetFriendById(string id)
		{
			if (friendsDictionary != null && friendsDictionary.ContainsKey(id))
			{
				return (Dictionary<string, object>)friendsDictionary[id];
			}

			return null;
		}
	}
}