using System;

namespace Puppet.Service
{
	public interface ISocialNetwork
	{
	    string AccessToken { get; }
	    string UserId { get; set; }
	    bool IsLoggedIn { get; }

	    void SocialInit();
	    void SocialLogin();
	    void SocialLogout();
	    void checkPublishPermission(Action<bool> onGetFinishPermission);
	    void Publish(string content, string url, Action<bool> onShareComplete);
	}
}