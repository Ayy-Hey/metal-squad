using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;

public class Lang
{
	public Lang(string path, string language, bool web)
	{
		if (!web)
		{
			this.setLanguage(path, language);
		}
		else
		{
			this.setLanguageWeb(path, language);
		}
	}

	public void setLanguage(string path, string language)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(path);
		this.Strings = new Hashtable();
		XmlElement xmlElement = xmlDocument.DocumentElement[language];
		if (xmlElement != null)
		{
			foreach (object obj in xmlElement)
			{
				XmlElement xmlElement2 = (XmlElement)obj;
				this.Strings.Add(xmlElement2.GetAttribute("name"), xmlElement2.InnerText);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("The specified language does not exist: " + language);
		}
	}

	public void setLanguageWeb(string xmlText, string language)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(new StringReader(xmlText));
		this.Strings = new Hashtable();
		XmlElement xmlElement = xmlDocument.DocumentElement[language];
		if (xmlElement != null)
		{
			foreach (object obj in xmlElement)
			{
				XmlElement xmlElement2 = (XmlElement)obj;
				this.Strings.Add(xmlElement2.GetAttribute("name"), xmlElement2.InnerText);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("The specified language does not exist: " + language);
		}
	}

	public string getString(string name)
	{
		if (!this.Strings.ContainsKey(name))
		{
			UnityEngine.Debug.LogError("The specified string does not exist: " + name);
			return string.Empty;
		}
		return (string)this.Strings[name];
	}

	private Hashtable Strings;
}
