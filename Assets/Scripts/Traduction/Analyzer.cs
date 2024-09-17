using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;



public enum TradTag
{
    unknown,
    stat,
    percent,
    damage
}
public enum TradAttribute
{
    value,
    target,
    stat,
    type
}
public enum TradStat
{
    FA
}
public enum AnalexState
{
    TagState,
    AttributeState,
    ValueState,
    ErrorState,
    EndState
}
public enum TradTokenTag
{
    UnkownToken,
    Tag,
    Value,
    Target
}
public class TagAttribute
{
    public TradTokenTag Attribute;
    public string Value;
}
public class Analyzer : MonoBehaviour
{
    const string LEFTBRACKET = "{";
    const string RIGHTBRACKET = "}";

    const string PERCENTPATTERN = "{percent value=(?<value>[0-9]+) target=(?<target>[A-Za-z]+)}";
    const string STATPATTERN = "{stat value=(?<value>[A-Za-z]+)}";
    const string DAMAGEPATTERN = "{damage type=(?<type>(direct|percent)) value=(?<value>[0-9]+)( stat=(?<stat>[A-Za-z]+))?}";

    [SerializeField]
    private ClairvoyanceIconData _clairvoyanceIconData;


    private Regex percentRegex = new Regex(PERCENTPATTERN, RegexOptions.IgnoreCase);
    private Regex statRegex = new Regex(STATPATTERN, RegexOptions.IgnoreCase);
    private Regex damageRegex = new Regex(DAMAGEPATTERN, RegexOptions.IgnoreCase);
    //Exemple de balise
    // {stat value=FA}
    // {percent value=60 target=FA}
    // {damage type=direct value=10}
    // {damage type=percent value=60 stat=FA}

    // Update is called once per frame
    void Update()
    {

    }

    #region Regex
    public string Execute(string stringToRead)
    {

        Match currentMatchPercent = percentRegex.Match(stringToRead);
        while (currentMatchPercent.Success)
        {
            Dictionary<TradAttribute, string> attributes = new Dictionary<TradAttribute, string>
            {
                { TradAttribute.value, currentMatchPercent.Groups["value"].Captures[0].ToString() },
                { TradAttribute.target, currentMatchPercent.Groups["target"].Captures[0].ToString() }
            };
            string replacement = ApplyTag(TradTag.percent, attributes);
            stringToRead = stringToRead.Replace(currentMatchPercent.Groups[0].ToString(), replacement);
            currentMatchPercent = currentMatchPercent.NextMatch();
        }

        Match currentMatchStat = statRegex.Match(stringToRead);
        while (currentMatchStat.Success)
        {
            Dictionary<TradAttribute, string> attributes = new Dictionary<TradAttribute, string>
            {
                { TradAttribute.value, currentMatchStat.Groups["value"].Captures[0].ToString() }
            };
            string replacement = ApplyTag(TradTag.stat, attributes);
            stringToRead = stringToRead.Replace(currentMatchStat.Groups[0].ToString(), replacement);
            currentMatchStat = currentMatchStat.NextMatch();
        }

        Match currentMatchDamage = damageRegex.Match(stringToRead);
        while (currentMatchDamage.Success)
        {
            Dictionary<TradAttribute, string> attributes = new Dictionary<TradAttribute, string>
            {
                { TradAttribute.value, currentMatchDamage.Groups["value"].Captures[0].ToString() },
                { TradAttribute.type, currentMatchDamage.Groups["type"].Captures[0].ToString() }
            };
            if (currentMatchDamage.Groups["stat"].Success)
            {
                attributes.Add(TradAttribute.stat, currentMatchDamage.Groups["stat"].Captures[0].ToString());
            }
            string replacement = ApplyTag(TradTag.damage, attributes);
            stringToRead = stringToRead.Replace(currentMatchDamage.Groups[0].ToString(), replacement);
            currentMatchDamage = currentMatchDamage.NextMatch();
        }


        return stringToRead;
    }
    #endregion

    #region Analyzer
    public string Analyze(string stringToEvaluate)
    {
        int ind = stringToEvaluate.IndexOf(LEFTBRACKET);
        while (ind != -1)
        {
            int endIndex = stringToEvaluate.IndexOf(RIGHTBRACKET, ind);
            if (endIndex == -1)
            {
                Debug.LogError($"Syntaxique error in {stringToEvaluate} : missing right bracket.");
            }
            else
            {
                string tagString = stringToEvaluate.Substring(ind + 1, endIndex - (ind + 1));
                string replacementString = EvaluateTag(tagString);
                stringToEvaluate = stringToEvaluate.Replace("{" + tagString + "}", replacementString);
            }
            ind = stringToEvaluate.IndexOf(LEFTBRACKET, ind + 1);
        }
        return stringToEvaluate;
    }

    private TradTokenTag GetNextTokenType(string strToEvaluate)
    {
        if (string.IsNullOrEmpty(strToEvaluate))
            return TradTokenTag.UnkownToken;

        if (strToEvaluate.StartsWith(TradTag.percent.ToString(), System.StringComparison.InvariantCultureIgnoreCase)
            || strToEvaluate.StartsWith(TradTag.stat.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
        {
            return TradTokenTag.Tag;
        }
        else if (strToEvaluate.StartsWith(TradAttribute.target.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
        {
            return TradTokenTag.Target;
        }
        else if (strToEvaluate.StartsWith(TradAttribute.value.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
        {
            return TradTokenTag.Value;
        }
        else
        {
            return TradTokenTag.UnkownToken;
        }
    }
    private TradTag ReadTag(string stringToRead)
    {
        if (stringToRead.Equals(TradTag.stat.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
        {
            return TradTag.stat;
        }
        else if (stringToRead.Equals(TradTag.percent.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
        {
            return TradTag.percent;
        }
        else return TradTag.unknown;
    }
    /// <summary>
    /// Read value from string pattern of : «value=value», all other type of pattern will result on an empty string.
    /// </summary>
    /// <param name="strToRead">the string to read</param>
    /// <returns>return the field of the value attribute, or empty if the string is incorrect</returns>
    private string ReadValue(string strToRead)
    {
        if (string.IsNullOrEmpty(strToRead) || !strToRead.StartsWith(TradAttribute.value.ToString(), System.StringComparison.InvariantCultureIgnoreCase))
            return string.Empty;
        return strToRead.Split("=")[1];
    }
    private string ApplyTag(TradTag tag, List<TagAttribute> attributes)
    {
        StringBuilder strb = new StringBuilder();
        switch (tag)
        {
            case TradTag.stat:
                strb.Append("<sprite name =\"");
                if (attributes[0].Value.Equals("FA", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append(_clairvoyanceIconData.StatForceDame.name);
                strb.Append("\">");

                break;
            case TradTag.percent:
                ;
                break;

        }
        return strb.ToString();
    }
  
    private string EvaluateTag(string tag)
    {
        var stringTab = tag.Split(' ');

        AnalexState currentState = AnalexState.TagState;

        TradTag currentTag = TradTag.unknown;
        int currentIndex = 0;

        List<TagAttribute> currenAtttributes = new List<TagAttribute>();
        TagAttribute currentAttribute = null;

        while (currentState != AnalexState.EndState)
        {
            switch (currentState)
            {
                case AnalexState.TagState:
                    currentTag = ReadTag(stringTab[currentIndex]);
                    if (currentTag == TradTag.unknown)
                    {
                        Debug.LogError($"Error reading tag, {stringTab[0]} unknown tag");
                        return string.Empty;
                    }
                    else
                    {
                        currentIndex++;
                        TradTokenTag nextTokenType = GetNextTokenType(stringTab[currentIndex]);
                        currentAttribute = new TagAttribute();
                        currentAttribute.Attribute = nextTokenType;
                        if (nextTokenType == TradTokenTag.Value)
                            currentState = AnalexState.ValueState;
                        else if (nextTokenType == TradTokenTag.Target)
                            currentState = AnalexState.AttributeState;
                        else
                            currentState = AnalexState.ErrorState;
                    }
                    break;
                case AnalexState.AttributeState:
                    Debug.Log("perdu");
                    return string.Empty;
                    break;
                case AnalexState.ValueState:
                    currentAttribute.Value = ReadValue(stringTab[currentIndex]);
                    currenAtttributes.Add(currentAttribute);
                    currentIndex++;
                    if (currentIndex >= stringTab.Length)
                    {
                        currentState = AnalexState.EndState;
                    }
                    break;
                case AnalexState.ErrorState:
                    Debug.LogError($"Syntaxe error in trad files for tag : {tag}");
                    return string.Empty;


            }
        }
        return ApplyTag(currentTag, currenAtttributes);

    }
    #endregion
    private string ApplyTag(TradTag tag, Dictionary<TradAttribute, string> attributes)
    {
        StringBuilder strb = new StringBuilder();
        switch (tag)
        {
            case TradTag.stat:
                strb.Append("<sprite name=\"");

                if (attributes[TradAttribute.value].Equals("FA", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append((_clairvoyanceIconData.StatForceDame == null) ? "FA" : _clairvoyanceIconData.StatForceDame.name);
                else if (attributes[TradAttribute.value].Equals("CL", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append((_clairvoyanceIconData.StatClairvoyance == null) ? "CL" : _clairvoyanceIconData.StatClairvoyance.name);
                else if (attributes[TradAttribute.value].Equals("CONV", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append((_clairvoyanceIconData.StatConviction == null) ? "CONV" : _clairvoyanceIconData.StatConviction.name);
                else if (attributes[TradAttribute.value].Equals("RAD", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append((_clairvoyanceIconData.StatRadiance == null) ? "RAD" : _clairvoyanceIconData.StatRadiance.name);
                else if (attributes[TradAttribute.value].Equals("VIT", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append((_clairvoyanceIconData.StatVitesse == null) ? "VIT" : _clairvoyanceIconData.StatVitesse.name);
                else if (attributes[TradAttribute.value].Equals("RES", System.StringComparison.InvariantCultureIgnoreCase))
                    strb.Append((_clairvoyanceIconData.StatResilience == null) ? "RES" : _clairvoyanceIconData.StatResilience.name);
                strb.Append("\">");

                break;
            case TradTag.percent:

                int value = int.Parse(attributes[TradAttribute.value]);
                float numericValue = 0f;
                string spriteName = "";
                if (attributes[TradAttribute.target].Equals("FA", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (GameManager.instance.playerStat != null)
                    {
                        numericValue = GameManager.instance.playerStat.ForceAme * (value / 100f);
                    }
                    spriteName = _clairvoyanceIconData.StatForceDame.name;
                }
                else if (attributes[TradAttribute.target].Equals("RAM", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (GameManager.instance.playerStat != null)
                    {
                        numericValue = GameManager.instance.playerStat.RadianceMax * (value / 100f);
                    }
                    spriteName = (_clairvoyanceIconData.StatRadiance == null) ? "RAM" : _clairvoyanceIconData.StatRadiance.name;
                }
                else if (attributes[TradAttribute.target].Equals("RAD", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    if (GameManager.instance.playerStat != null)
                    {
                        numericValue = GameManager.instance.playerStat.Radiance * (value / 100f);
                    }
                    spriteName = (_clairvoyanceIconData.StatRadiance == null) ? "RAM" : _clairvoyanceIconData.StatRadiance.name;
                }
                
                strb.Append(Mathf.FloorToInt(numericValue));
                strb.Append(" <sprite name=\"");
                strb.Append(spriteName);
                strb.Append("\">");
                break;
            case TradTag.damage:
                float damageValue = 0;
                if (attributes[TradAttribute.type].Equals("direct", StringComparison.InvariantCultureIgnoreCase))
                {
                    damageValue = int.Parse(attributes[TradAttribute.value]);
                }
                else if (attributes[TradAttribute.type].Equals("percent", StringComparison.InvariantCultureIgnoreCase))
                {
                    int percentValue = int.Parse(attributes[TradAttribute.value]);
                    if (attributes[TradAttribute.stat].Equals("FA", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (GameManager.instance.playerStat != null)
                        {
                            damageValue = GameManager.instance.playerStat.ForceAme * (percentValue / 100f);
                        }
                    }
                    else if (attributes[TradAttribute.stat].Equals("RAM", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (GameManager.instance.playerStat != null)
                        {
                            numericValue = GameManager.instance.playerStat.RadianceMax * (percentValue / 100f);
                        }
                    }
                }
                spriteName = (_clairvoyanceIconData.Damage == null) ? "DMG" : _clairvoyanceIconData.Damage.name;
                strb.Append(Mathf.FloorToInt(damageValue));
                strb.Append(" <sprite name=\"");
                strb.Append(spriteName);
                strb.Append("\">");
                break;

        }
        return strb.ToString();
    }
}
