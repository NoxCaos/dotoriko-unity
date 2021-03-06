// 
//  SerializableDictionary.cs
//  
//  Author:
//       Denis Oleynik <denis@meliorgames.com>
// 
//  Copyright (c) 2013 Melior Games Inc.
// 
//  
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace DotOriko.Data.Serialization
{
    [XmlRoot("Dictionary")]
	[Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable,ISerializable
    {
		private const string KEYS = "___keys";
		
		#region ISerializable implementation
		
		public new void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			List<TKey> keys = new List<TKey>();
			
			foreach(TKey key in this.Keys)
			{
				keys.Add(key);
				
				info.AddValue(key.ToString(),this[key]);
			}
			
			info.AddValue(KEYS, keys.ToArray());
		}
		public SerializableDictionary(){}
		public SerializableDictionary(SerializationInfo info, StreamingContext context)
		{
			TKey[] keys = (TKey[])info.GetValue(KEYS,typeof(TKey[]));
			
			foreach(TKey key in keys)
			{
				this.Add(key,(TValue)info.GetValue(key.ToString(),typeof(TValue)));
			}
		}
		#endregion

        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
			
            if (wasEmpty)
                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                this.Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }
			
			
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {

            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue value = this[key];

                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion

    }
}

