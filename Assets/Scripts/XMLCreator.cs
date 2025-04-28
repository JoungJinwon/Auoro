using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLCreator : MonoBehaviour
{
    void Start()
    {
        CreateXml();
    }

    void CreateXml()
    {
        XmlDocument xmlDoc = new XmlDocument();
        // xml의 버전과 인코딩 방식을 지정해 준다.
        xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        // 루트 노드 생성
        XmlNode charInfo = xmlDoc.CreateNode(XmlNodeType.Element, "CharacterInfo", string.Empty);
        xmlDoc.AppendChild(charInfo);

        // 자식 노드 생성
        XmlNode child = xmlDoc.CreateNode(XmlNodeType.Element, "Character", string.Empty);
        charInfo.AppendChild(child);

        // 자식 노드에 들어갈 속성 생성
        XmlElement name = xmlDoc.CreateElement("Name");
        name.InnerText = "Auoro";
        child.AppendChild(name);

        XmlElement level = xmlDoc.CreateElement("Level");
        level.InnerText = "1";
        child.AppendChild(level);

        XmlElement exp = xmlDoc.CreateElement("Experience");
        exp.InnerText = "0";
        child.AppendChild(exp);

        // 대화 텍스트
        XmlNode dialogue = xmlDoc.CreateNode(XmlNodeType.Element, "Dialogue", string.Empty);
        xmlDoc.AppendChild(dialogue);

        XmlNode lily = xmlDoc.CreateNode(XmlNodeType.Element, "Lily", string.Empty);
        dialogue.AppendChild(lily);

        XmlElement first = xmlDoc.CreateElement("First");
        first.InnerText = "우로! 늦잠 잤네?";
        lily.AppendChild(first);

        xmlDoc.Save("./Assets/Resources/DialgueData/DialogueData.xml");
    }
}
