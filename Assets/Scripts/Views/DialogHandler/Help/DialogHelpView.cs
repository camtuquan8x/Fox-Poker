using UnityEngine;
using System.Collections;
using Puppet.Service;
using Puppet;
namespace Puppet.Service
{
    [PrefabAttribute(Name = "Prefabs/Dialog/DialogHelp", Depth = 7, IsAttachedToCamera = true, IsUIPanel = true)]
    public class DialogHelpView : BaseDialog<DialogHelp, DialogHelpView>
    {

        #region UnityEditor
        public UIToggle btnFAQ, btnRule, btnExp;
        public UniWebView webView;
        public UISprite foreground;
        #endregion
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDelegate.Add(btnFAQ.onChange, OnBtnFAQChanged);
            EventDelegate.Add(btnRule.onChange, OnBtnRuleChanged);
            EventDelegate.Add(btnExp.onChange, OnBtnEXPChanged);
            webView.OnReceivedMessage += OnReceivedMessage;
            webView.OnLoadComplete += OnLoadComplete;
            webView.OnWebViewShouldClose += OnWebViewShouldClose;
            webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            EventDelegate.Remove(btnFAQ.onChange, OnBtnFAQChanged);
            EventDelegate.Remove(btnRule.onChange, OnBtnRuleChanged);
            EventDelegate.Remove(btnExp.onChange, OnBtnEXPChanged);
            webView.OnReceivedMessage -= OnReceivedMessage;
            webView.OnLoadComplete -= OnLoadComplete;
            webView.OnWebViewShouldClose -= OnWebViewShouldClose;
            webView.OnEvalJavaScriptFinished -= OnEvalJavaScriptFinished;
        }
        public override void ShowDialog(DialogHelp data)
        {
            base.ShowDialog(data);
            SetWebView();
        }
        private void SetWebView()
        {
            //int uiFactor = UniWebViewHelper.RunningOnRetinaIOS() ? 2 : 1;
            int uiFactor = 1;
            UIRoot mRoot = NGUITools.FindInParents<UIRoot>(gameObject);
            float ratio = ((float)mRoot.activeHeight / Screen.height) * uiFactor;
            int width = Mathf.FloorToInt(UniWebViewHelper.screenWidth * ratio / uiFactor);
            int height = Mathf.FloorToInt(UniWebViewHelper.screenHeight * ratio / uiFactor);

            //UISliceBackgroundPopup backgroundPopup = gameObject.GetComponentInChildren<UISliceBackgroundPopup>();
            int webMarginWidth = Mathf.FloorToInt(width - (foreground.width));
            int webMarginHeight = Mathf.FloorToInt(height - (foreground.height));

            int leftRight = Mathf.FloorToInt(webMarginWidth / (2 * ratio));

            int topbottom = Mathf.RoundToInt((webMarginHeight / 2));
            webView.insets = new UniWebViewEdgeInsets(topbottom, leftRight, topbottom, leftRight);

        }
        private void OnBtnFAQChanged()
        {
            if (btnFAQ.value)
            {
                webView.url = "http://vnexpress.net/";
                webView.Load();
            }
        }
        private void OnBtnRuleChanged()
        {
            if (btnExp.value)
            {
                webView.url = "http://www.24h.com.vn/";
                webView.Load();
            }
        }
        private void OnBtnEXPChanged()
        {
            if (btnExp.value)
            {
                webView.url = "http://www.baomoi.com/";
                webView.Load();
            }
        }

        private void OnEvalJavaScriptFinished(UniWebView webView, string result)
        {
            throw new System.NotImplementedException();
        }

        bool OnWebViewShouldClose(UniWebView webView)
        {
            if (this.webView == webView)
            {
                this.webView = null;
                GameObject.Destroy(gameObject);
                return true;
            }
            return false;
        }

        private void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
        {
            if (success)
            {
                webView.Show();
            }
            else
            {
                Debug.Log("Something wrong in webview loading: " + errorMessage);
                //_errorMessage = errorMessage;
            }
        }

        private void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
    public class DialogHelp : AbstractDialogData
    {

        public override void ShowDialog()
        {
            DialogHelpView.Instance.ShowDialog(this);
        }
    }
}