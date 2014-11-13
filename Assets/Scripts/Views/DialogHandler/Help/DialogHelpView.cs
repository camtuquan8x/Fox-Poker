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
        public UIToggle btnFAQ, btnRule, btnExp,btnFeedBack;
        //public UniWebView webView;
        public UISprite foreground;
        public GameObject contentFeedBack;
        #endregion
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDelegate.Add(btnFAQ.onChange, OnBtnFAQChanged);
            EventDelegate.Add(btnRule.onChange, OnBtnRuleChanged);
            EventDelegate.Add(btnExp.onChange, OnBtnEXPChanged);
            EventDelegate.Add(btnFeedBack.onChange, OnBtnFeedBackChanged);
            //webView.OnReceivedMessage += OnReceivedMessage;
            //webView.OnLoadComplete += OnLoadComplete;
            //webView.OnWebViewShouldClose += OnWebViewShouldClose;
            //webView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
        }

    


        protected override void OnDisable()
        {
            base.OnDisable();
            EventDelegate.Remove(btnFAQ.onChange, OnBtnFAQChanged);
            EventDelegate.Remove(btnRule.onChange, OnBtnRuleChanged);
            EventDelegate.Remove(btnExp.onChange, OnBtnEXPChanged);
            EventDelegate.Remove(btnFeedBack.onChange, OnBtnFeedBackChanged);
            //webView.OnReceivedMessage -= OnReceivedMessage;
            //webView.OnLoadComplete -= OnLoadComplete;
            //webView.OnWebViewShouldClose -= OnWebViewShouldClose;
            //webView.OnEvalJavaScriptFinished -= OnEvalJavaScriptFinished;
        }
        public override void ShowDialog(DialogHelp data)
        {
            base.ShowDialog(data);
            SetWebView();
            btnFAQ.value = true;
        }
       
        private void SetWebView()
        {

            //int uiFactor = UniWebViewHelper.RunningOnRetinaIOS() ? 2 : 1;
            int uiFactor = 1;
            UIRoot mRoot = NGUITools.FindInParents<UIRoot>(gameObject);
            float ratioHeight = ((float)mRoot.activeHeight / UniWebViewHelper.screenHeight) * uiFactor;
            float ratioWidth = ((float)mRoot.manualWidth / UniWebViewHelper.screenWidth) * uiFactor;
            int width = Mathf.FloorToInt(UniWebViewHelper.screenWidth * ratioWidth / uiFactor);
            int height = Mathf.FloorToInt(UniWebViewHelper.screenHeight * ratioHeight / uiFactor);
            Logger.Log("========> mRoot " + mRoot.manualWidth + "/" + mRoot.manualHeight + "=====");
            Logger.Log("========> widthReal " + width + "/" + height + "=====");
            Logger.Log("========> foreground  " + foreground.width + "/" + foreground.height + "=====");
            Logger.Log("========> transparent  " + bkgTransparent.width + "/" + bkgTransparent.height + "=====");
            //UISliceBackgroundPopup backgroundPopup = gameObject.GetComponentInChildren<UISliceBackgroundPopup>();
            int webMarginWidth = Mathf.FloorToInt(width - (foreground.width));
            int webMarginHeight = Mathf.FloorToInt(height - (foreground.height));

            int leftRight = Mathf.FloorToInt(webMarginWidth / (2 * ratioWidth));

            int topbottom = Mathf.RoundToInt((webMarginHeight / (2 * ratioHeight)));
            //webView.insets = new UniWebViewEdgeInsets(Mathf.RoundToInt(topbottom + 130*ratioHeight), leftRight, Mathf.RoundToInt(topbottom - 130 * ratioHeight), leftRight);

        }
        private void OnBtnFeedBackChanged()
        {
            if (btnFeedBack.value)
            {
                if (!contentFeedBack.active)
                {
                    contentFeedBack.SetActive(true);
                    //webView.gameObject.SetActive(false);
                }
            }
        }
        private void OnBtnFAQChanged()
        {
            if (btnFAQ.value)
            {
                //webView.url = "http://vnexpress.net/";
                //webView.Load();
                //if (!webView.gameObject.active)
                //{
                //    contentFeedBack.SetActive(false);
                //    webView.gameObject.SetActive(true);
                //}
            }
        }
        private void OnBtnRuleChanged()
        {
            if (btnRule.value)
            {
                //webView.url = "http://www.24h.com.vn/";
                //webView.Load();
                //if (!webView.gameObject.active)
                //{
                //    contentFeedBack.SetActive(false);
                //    webView.gameObject.SetActive(true);
                //}
            }
        }
        private void OnBtnEXPChanged()
        {
            //if (btnExp.value)
            //{
            //    webView.url = "http://www.baomoi.com/";
            //    webView.Load();
            //    if (!webView.gameObject.active)
            //    {
            //        contentFeedBack.SetActive(false);
            //        webView.gameObject.SetActive(true);
            //    }
            //}
        }

        //private void OnEvalJavaScriptFinished(UniWebView webView, string result)
        //{
        //    throw new System.NotImplementedException();
        //}

        //bool OnWebViewShouldClose(UniWebView webView)
        //{
        //    if (this.webView == webView)
        //    {
        //        this.webView = null;
        //        GameObject.Destroy(gameObject);
        //        return true;
        //    }
        //    return false;
        //}

        //private void OnLoadComplete(UniWebView webView, bool success, string errorMessage)
        //{
        //    if (success)
        //    {
        //        webView.Show();
        //    }
        //    else
        //    {
        //        Debug.Log("Something wrong in webview loading: " + errorMessage);
        //        //_errorMessage = errorMessage;
        //    }
        //}

        //private void OnReceivedMessage(UniWebView webView, UniWebViewMessage message)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
    public class DialogHelp : AbstractDialogData
    {

        public override void ShowDialog()
        {
            DialogHelpView.Instance.ShowDialog(this);
        }
    }
}