using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Configuration;

public class ShortCodes
{
    #region "Property"
    private static string _controlDirectory = "~/controls/{0}.ascx";
    private static string ControlDirectory
    {
        get { return _controlDirectory; }
        set { _controlDirectory = value;  }
    }

    private static string _codePattern = @"\[sc (.*?)]";
    private static string CodePattern
    {
        get { return _codePattern; }
        set { _codePattern = value; }
    }
    #endregion

    public ShortCodes()
    {
        ControlDirectory = ConfigurationManager.AppSettings["PluginControlDirectory"];
        CodePattern = ConfigurationManager.AppSettings["PluginRegexPattern"];
    }

    public static string Render(string data)
    {
        new ShortCodes();
        Regex objRegex = new Regex(@CodePattern);
        MatchCollection objCol = objRegex.Matches(data);
        foreach (Match item in objCol)
            data = data.Replace(item.Groups[0].Value, Read(item.Groups[1].Value.ToString()));
        return data;
    }

    private static string Read(string str)
    {
        string[] strVal = str.Split(' ');
        ArrayList strValArr = new ArrayList();
        foreach (string a in strVal)
            strValArr.Add(a);
        if (strValArr.Count < 2)
            strValArr.Add("");
        return LoadControl(string.Format(ControlDirectory, strValArr[0]), strValArr[1]);
    }

    private static string LoadControl(string UserControlPath, params object[] constructorParameters)
    {
        if (!File.Exists(HttpContext.Current.Server.MapPath(UserControlPath)))
            return "Invalid Short Codes!";

        FormlessPage page = new FormlessPage();
        page.EnableViewState = false;

        List<Type> constParamTypes = new List<Type>();
        foreach (object constParam in constructorParameters)
            constParamTypes.Add(constParam.GetType());

        UserControl userControl = (UserControl)page.LoadControl(UserControlPath);

        ConstructorInfo constructor = userControl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());
        if (constructor == null)
            return "The requested constructor was not found on : " + userControl.GetType().BaseType.ToString();
        else
            constructor.Invoke(userControl, constructorParameters);

        page.Controls.Add(userControl);

        StringWriter textWriter = new StringWriter();
        HttpContext.Current.Server.Execute(page, textWriter, false);
        return textWriter.ToString();
    }

    public class FormlessPage : Page
    {
        public override void VerifyRenderingInServerForm(Control control)
        { }
    }
}

