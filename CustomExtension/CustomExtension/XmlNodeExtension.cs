using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CustomExtension
{
    public static class XmlNodeExtension
    {

        public static string GetStringInnerText(this XmlNode node, string defaultValue)
        {
            if (!string.IsNullOrEmpty(node.InnerText))
                return node.InnerText.Trim();
            return defaultValue;
        }


        public static string GetStringAttribute(this XmlNode node, string key, string defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            if (attributes[key] != null
                && !string.IsNullOrEmpty(attributes[key].Value))
                return attributes[key].Value.Trim();
            return defaultValue;
        }



        public static int GetIntAttribute(this XmlNode node, string key, int defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            int val = defaultValue;

            if (attributes[key] != null
                && !string.IsNullOrEmpty(attributes[key].Value))
            {
                int.TryParse(attributes[key].Value, out val);
            }
            return val;
        }


        public static double GetDoubleAttribute(this XmlNode node, string key, double defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            double val = defaultValue;

            if (attributes[key] != null
                && !string.IsNullOrEmpty(attributes[key].Value))
            {
                double.TryParse(attributes[key].Value, out val);
            }
            return val;
        }

        public static decimal GetDecimalAttribute(this XmlNode node, string key, decimal defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            decimal val = defaultValue;

            if (attributes[key] != null
                && !string.IsNullOrEmpty(attributes[key].Value))
            {
                decimal.TryParse(attributes[key].Value, out val);
            }
            return val;
        }



        public static float GetFloatAttribute(this XmlNode node, string key, float defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            float val = defaultValue;

            if (attributes[key] != null
                && !string.IsNullOrEmpty(attributes[key].Value))
            {
                float.TryParse(attributes[key].Value, out val);
            }
            return val;
        }



        public static bool GetBoolAttribute(this XmlNode node, string key, bool defaultValue)
        {
            XmlAttributeCollection attributes = node.Attributes;
            bool val = defaultValue;

            if (attributes[key] != null
                && !string.IsNullOrEmpty(attributes[key].Value))
            {
                bool.TryParse(attributes[key].Value, out val);
            }
            return val;
        }


        public static bool GetBoolFromSubNote(this XmlNode node, string nodeName, bool defaultValue)
        {
            bool val = defaultValue;
            string stringValue = GetStringFromSubNode(node, nodeName, "");
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (bool.TryParse(stringValue, out val))
                    return val;
            }
            return val;
        }

        public static int GetIntFromSubNote(this XmlNode node, string nodeName, int defaultValue)
        {
            int val = defaultValue;
            string stringValue = GetStringFromSubNode(node, nodeName, "");
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (int.TryParse(stringValue, out val))
                    return val;
            }
            return val;
        }


        public static decimal GetDecimalFromSubNote(this XmlNode node, string nodeName, decimal defaultValue)
        {
            decimal val = defaultValue;
            string stringValue = GetStringFromSubNode(node, nodeName, "");
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (decimal.TryParse(stringValue, out val))
                    return val;
            }
            return val;
        }


        public static string GetStringFromSubNode(this XmlNode node, string nodeName, string defaultValue)
        {
            if (!string.IsNullOrEmpty(node.Name))
            {
                XmlNode subNode = GetsubNode(node, nodeName);
                if (subNode != null)
                {
                    return subNode.GetStringInnerText(defaultValue);
                }
            }
            return defaultValue;
        }

        private static XmlNode GetsubNode(XmlNode node, string nodeName)
        {
            XmlNode subNode;
            if (string.IsNullOrEmpty(node.NamespaceURI))
            {
                subNode = node.SelectSingleNode(nodeName);
            }
            else
            {
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(node.OwnerDocument.NameTable);
                nsMgr.AddNamespace(node.Prefix, node.NamespaceURI);
                subNode = node.SelectSingleNode(nodeName, nsMgr);
            }
            return subNode;
        }

        public static float GetFloatFromSubNote(this XmlNode node, string nodeName, float defaultValue)
        {
            float val = defaultValue;
            string stringValue = GetStringFromSubNode(node, nodeName, "");
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (float.TryParse(stringValue, out val))
                    return val;
            }
            return val;
        }

        public static double GetDoubleFromSubNote(this XmlNode node, string nodeName, double defaultValue)
        {
            double val = defaultValue;
            string stringValue = GetStringFromSubNode(node, nodeName, "");
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (double.TryParse(stringValue, out val))
                    return val;
            }
            return val;
        }


        public static DateTime GetDatetimeFromSubNote(this XmlNode node, string nodeName, DateTime defaultValue)
        {
            DateTime val = defaultValue;
            string stringValue = GetStringFromSubNode(node, nodeName, "");
            if (!string.IsNullOrEmpty(stringValue))
            {
                if (DateTime.TryParse(stringValue, out val))
                {
                    if (defaultValue.CompareTo(val) < 0)
                        return val;
                    return defaultValue;
                }
            }
            return val;
        }

    }
}
