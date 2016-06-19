using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Flow
    {

        private FlowItem[] itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public FlowItem[] Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItem
    {

        private FlowItemSequence sequenceField;

        private string titleField;

        private string idField;

        /// <remarks/>
        public FlowItemSequence Sequence
        {
            get
            {
                return this.sequenceField;
            }
            set
            {
                this.sequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItemSequence
    {

        private object[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Call", typeof(FlowItemSequenceCall))]
        [System.Xml.Serialization.XmlElementAttribute("Connect", typeof(FlowItemSequenceConnect))]
        [System.Xml.Serialization.XmlElementAttribute("Disconnect", typeof(FlowItemSequenceDisconnect))]
        [System.Xml.Serialization.XmlElementAttribute("SetPort", typeof(FlowItemSequenceSetPort))]
        [System.Xml.Serialization.XmlElementAttribute("SetProxy", typeof(FlowItemSequenceSetProxy))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItemSequenceCall
    {

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItemSequenceConnect
    {

        private string deviceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string device
        {
            get
            {
                return this.deviceField;
            }
            set
            {
                this.deviceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItemSequenceDisconnect
    {

        private string deviceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string device
        {
            get
            {
                return this.deviceField;
            }
            set
            {
                this.deviceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItemSequenceSetPort
    {

        private string ipField;

        private byte portField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ip
        {
            get
            {
                return this.ipField;
            }
            set
            {
                this.ipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte port
        {
            get
            {
                return this.portField;
            }
            set
            {
                this.portField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class FlowItemSequenceSetProxy
    {

        private string proxyField;

        private bool enabledField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string proxy
        {
            get
            {
                return this.proxyField;
            }
            set
            {
                this.proxyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool enabled
        {
            get
            {
                return this.enabledField;
            }
            set
            {
                this.enabledField = value;
            }
        }
    }    
}
