using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract class IDialog
{
    private string title;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Content
    {
        get { return content; }
        set { content = value; }
    }
    private string content;
}

